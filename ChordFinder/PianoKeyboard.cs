using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChordFinder
{
    public class PianoKeyboard : Control
    {
        readonly Dictionary<Notes, Rectangle> _WhiteKeys;
        readonly Dictionary<Notes, Rectangle> _BlackKeys;

        // { base, highlighted, selected, both || white, black }
        static readonly Brush[] _Brushes = new Brush[] {
            Brushes.White, Brushes.Lime, Brushes.Blue, Brushes.Cyan,
            Brushes.Black, Brushes.Green, Brushes.Navy, Brushes.Teal
        };

        Notes _SelectedNotes;
        Notes _HighlightedNoes;

        public event EventHandler? SelectedNotesChanged;

        public Notes SelectedNotes
        {
            get => _SelectedNotes;
            set
            {
                _SelectedNotes = value;
                SelectedNotesChanged?.Invoke(this, EventArgs.Empty);
                Invalidate();
            }
        }

        public Notes HighlightedNotes
        {
            get => _HighlightedNoes;
            set
            {
                _HighlightedNoes = value;
                Invalidate();
            }
        }

        public PianoKeyboard()
        {
            DoubleBuffered = true;

            _WhiteKeys = new()
            {
                [Notes.C] = Rectangle.Empty,
                [Notes.D] = Rectangle.Empty,
                [Notes.E] = Rectangle.Empty,
                [Notes.F] = Rectangle.Empty,
                [Notes.G] = Rectangle.Empty,
                [Notes.A] = Rectangle.Empty,
                [Notes.B] = Rectangle.Empty
            };

            _BlackKeys = new()
            {
                [Notes.CSharp] = Rectangle.Empty,
                [Notes.EFlat] = Rectangle.Empty,
                [Notes.GFlat] = Rectangle.Empty,
                [Notes.AFlat] = Rectangle.Empty,
                [Notes.BFlat] = Rectangle.Empty
            };
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            int whiteKeyDiv = Width / 7;
            int blackKeyDiv = Width / 12;

            _SetKeyPosition(Notes.C, 0, whiteKeyDiv);
            _SetKeyPosition(Notes.D, 1, whiteKeyDiv);
            _SetKeyPosition(Notes.E, 2, whiteKeyDiv);
            _SetKeyPosition(Notes.F, 3, whiteKeyDiv);
            _SetKeyPosition(Notes.G, 4, whiteKeyDiv);
            _SetKeyPosition(Notes.A, 5, whiteKeyDiv);
            _SetKeyPosition(Notes.B, 6, whiteKeyDiv);

            _SetKeyPosition(Notes.CSharp, 1, blackKeyDiv);
            _SetKeyPosition(Notes.EFlat, 3, blackKeyDiv);
            _SetKeyPosition(Notes.GFlat, 6, blackKeyDiv);
            _SetKeyPosition(Notes.AFlat, 8, blackKeyDiv);
            _SetKeyPosition(Notes.BFlat, 10, blackKeyDiv);

            Invalidate();
        }

        private void _SetKeyPosition(Notes key, int x, int width)
        {
            if (_WhiteKeys.ContainsKey(key))
            {
                _WhiteKeys[key] = new Rectangle(x * width, 0, width, Height);
            }
            else if (_BlackKeys.ContainsKey(key))
            {
                _BlackKeys[key] = new Rectangle(x * width, 0, width, Height / 2);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);
            var outlinePen = new Pen(BackColor, 8);

            foreach (var k in _WhiteKeys)
            {
                _DrawKey(false, k.Key, k.Value, outlinePen, e.Graphics);
            }

            foreach (var k in _BlackKeys)
            {
                _DrawKey(true, k.Key, k.Value, outlinePen, e.Graphics);
            }

            base.OnPaint(e);
        }

        void _DrawKey(bool black, Notes note, Rectangle bounds, Pen outlinePen, Graphics g)
        {

            bool selected = (note & _SelectedNotes) != 0;
            bool highlighted = (note & _HighlightedNoes) != 0;
            int brushIndex = (black ? 4 : 0) | (selected ? 2 : 0) | (highlighted ? 1 : 0);

            g.FillRectangle(_Brushes[brushIndex], bounds);
            g.DrawRectangle(outlinePen, bounds);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            var note = _NoteFromArea(e.Location);
            SelectedNotes ^= note;

            base.OnMouseUp(e);
        }

        private Notes _NoteFromArea(Point location)
        {
            foreach (var k in _BlackKeys)
            {
                if (k.Value.Contains(location)) return k.Key;
            }

            foreach (var k in _WhiteKeys)
            {
                if (k.Value.Contains(location)) return k.Key;
            }

            return Notes.None;
        }
    }
}
