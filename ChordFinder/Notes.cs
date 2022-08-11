using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChordFinder
{
    [Flags]
    public enum Notes
    {
        None = 0,
        C = 1 << 0,
        CSharp = 1 << 1,
        D = 1 << 2,
        EFlat = 1 << 3,
        E = 1 << 4,
        F = 1 << 5,
        GFlat = 1 << 6,
        G = 1 << 7,
        AFlat = 1 << 8,
        A = 1 << 9,
        BFlat = 1 << 10,
        B = 1 << 11
    }

    public static class FriendlyNoteToString
    {
        readonly static string[] _NoteNames = new string[] { "C", "C♯", "D", "D♭", "E", "F", "G♭", "G", "G♭", "A", "A♭", "B", "B♭" };

        public static string NoteString(this Notes n)
        {
            var s = new StringBuilder(10);
            int n2 = (int)n;

            for (int i = 0; i <_NoteNames.Length; i++)
            {
                if ((n2 & (1 << i)) != 0)
                {
                    if (s.Length > 0) s.Append(", ");
                    s.Append(_NoteNames[i]);
                }
            }
            
            return s.Length > 0 ? s.ToString() : "(none)";
        }
    }
}
