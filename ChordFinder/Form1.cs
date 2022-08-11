namespace ChordFinder
{
    public partial class Form1 : Form
    {
        readonly PianoKeyboard _Keyboard;
        readonly Font[] _MatchFonts = new Font[] {
            new("Segoe UI Symbol", 11f, FontStyle.Bold),
            new("Segoe UI Symbol", 11f, FontStyle.Italic)
        };

        readonly List<ListViewItem> _ChordLVIs = new();

        public Form1()
        {
            InitializeComponent();
            _Keyboard = new() { Dock = DockStyle.Fill };
            splitContainer1.Panel1.Controls.Add(_Keyboard);

            foreach (var c in Chord.Chords)
            {
                var lvi = lvChords.Items.Add(c.ToString());
                lvi.SubItems.Add(c.Tonic);
                lvi.SubItems.Add(c.ChordType);
                lvi.Tag = c.Notes;
                _ChordLVIs.Add(lvi);
            }

            cbKeyTonic.Items.AddRange(MusicalKey.Keys);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cbKeyTonic.SelectedIndex = 0;

            _Keyboard.SelectedNotesChanged += FilterChords;
        }

        private void lvChords_DrawItem_1(object sender, DrawListViewItemEventArgs e)
        {
            int key = (int)((MusicalKey)cbKeyTonic.SelectedItem).Notes;
            int sel = (int)_Keyboard.SelectedNotes;
            int notes = (int)e.Item.Tag;

            e.Graphics.FillRectangle(e.Item.Selected ? Brushes.PaleGreen : Brushes.White, e.Bounds);
            e.Graphics.DrawString(e.Item.Text, _MatchFonts[(key & notes) == notes ? 0 : 1], Brushes.Black, e.Bounds);
        }

        private void lvChords_SelectedIndexChanged(object sender, EventArgs e)
        {
            int n = 0;
            foreach (ListViewItem i in lvChords.SelectedItems) n |= (int)i.Tag;
            _Keyboard.HighlightedNotes = (Notes)n;
        }

        private void lvChords_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            lvChords.SelectedItems.Clear();
        }

        private void cbKeyTonic_SelectedIndexChanged(object sender, EventArgs e)
        {
            lvChords.Invalidate();
        }

        private void FilterChords(object? sender, EventArgs e)
        {
            var x = lvChords.SelectedItems.OfType<ListViewItem>().FirstOrDefault();
            lvChords.SelectedIndices.Clear();
            lvChords.Items.Clear();

            int sn = (int)_Keyboard.SelectedNotes;
            int i = 0;
            int si = -1;
            
            foreach (var lvi in _ChordLVIs)
            {
                if (sn == 0 || (sn & (int)lvi.Tag) == sn)
                {
                    i++;
                    lvChords.Items.Add(lvi);
                    if (lvi == x) si = i;
                }
            }

            if (si >= 0)
            {
                lvChords.SelectedIndices.Add(si);
            }

            lvChords.Invalidate();
        }
    }
}