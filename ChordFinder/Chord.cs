using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChordFinder
{
    public readonly struct Chord
    {
        public Notes Notes { get; }
        public string Tonic { get; }
        public string ChordType { get; }

        public Chord(string type, Notes tonic, int firstInterval, int secondInterval)
        {
            int n1 = (int)tonic;
            int n2 = _ApplyInterval(n1, firstInterval);
            int n3 = _ApplyInterval(n2, secondInterval);

            Tonic = tonic.NoteString();
            ChordType = type;
            Notes = (Notes)(n1 | n2 | n3);
        }

        public Chord(string type, Notes tonic, int firstInterval, int secondInterval, int thirdInterval)
        {
            int n1 = (int)tonic;
            int n2 = _ApplyInterval(n1, firstInterval);
            int n3 = _ApplyInterval(n2, secondInterval);
            int n4 = _ApplyInterval(n3, thirdInterval);

            Tonic = tonic.NoteString();
            ChordType = type;
            Notes = (Notes)(n1 | n2 | n3 | n4);
        }

        private static int _ApplyInterval(int n, int interval)
        {
            n <<= interval;
            if (n > (int)Notes.B) n >>= 12;
            return n;
        }

        public static readonly Chord[] Chords = _InitChords().ToArray();

        private static IEnumerable<Chord> _InitChords()
        {
            foreach (var tonic in Enum.GetValues<Notes>())
            {
                if (tonic != 0)
                {
                    string ns = tonic.NoteString();
                    yield return new Chord("", tonic, 4, 3);
                    yield return new Chord("m", tonic, 3, 4);
                    yield return new Chord("aug", tonic, 4, 4);
                    yield return new Chord("dim", tonic, 3, 3);
                    yield return new Chord("7", tonic, 4, 3, 3);
                    yield return new Chord("Maj7", tonic, 4, 3, 4);
                    yield return new Chord("m7", tonic, 3, 4, 3);
                    yield return new Chord("mMaj7", tonic, 3, 4, 4);
                    yield return new Chord("dim7", tonic, 3, 3, 3);
                }
            }
        }

        public override string ToString()
        {
            return $"{Tonic}{ChordType}";
        }
    }
}
