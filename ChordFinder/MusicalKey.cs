using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChordFinder
{
    internal class MusicalKey
    {
        public Notes Notes { get; }
        public string Name { get; }

        public MusicalKey(string name, int tonic, int key)
        {
            Name = name;

            int n = key << tonic;
            int n2 = n & 0b11111_1111111;
            n2 |= n >> 12;
            Notes = (Notes)n2;
        }


        public static readonly MusicalKey[] Keys = _Init().ToArray();

        private static IEnumerable<MusicalKey> _Init()
        {
            var scale = Enum.GetValues<Notes>().Where(n => n != Notes.None).ToArray();

            int majorKey =
                (int)Notes.C |
                (int)Notes.D |
                (int)Notes.E |
                (int)Notes.F |
                (int)Notes.G |
                (int)Notes.A |
                (int)Notes.B;

            int minorKey =
                (int)Notes.C |
                (int)Notes.EFlat |
                (int)Notes.E |
                (int)Notes.F |
                (int)Notes.G |
                (int)Notes.AFlat |
                (int)Notes.BFlat;

            yield return new MusicalKey("None", 0, scale.Aggregate(0, (x, y) => x | (int)y));

            for (int i = 0; i < scale.Length; i++)
            {
                yield return new MusicalKey($"{scale[i].NoteString()} Major", i, majorKey);
            }

            for (int i = 0; i < scale.Length; i++)
            {
                yield return new MusicalKey($"{scale[i].NoteString()} Minor", i, minorKey);
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
