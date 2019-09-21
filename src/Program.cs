using System;
using System.Collections.Generic;

namespace ConsoleMusic
{
    class Program
    {
        static void Main(string[] args)
        {
            ToneCulator tc = new ToneCulator();
            tc.ReadFile("sound.cmu");
            tc.Play();
        }
    }

    struct Note
    {
        private int tone;
        private int duration;
        public Note(int tone, int duration)
        {
            this.tone = tone;
            this.duration = duration;
        }

        public int Tone { get { return tone; } }
        public int Duration { get { return duration; } }
    }

    class ToneCulator
    {
        /// <summary>
        /// Contains notes in the 12-scale and their corresponding index
        /// </summary>
        private static Dictionary<string, int> noteIndices = new Dictionary<string, int>
        {
            { "A", 1 },
            { "A#", 2 }, { "Bb", 2 },
            { "B", 3 },
            { "C", -8 },
            { "C#", -7 }, { "Db", -7 },
            { "D", -6 },
            { "D#", -5 }, { "Eb", -5 },
            { "E", -4 },
            { "F", -3 },
            { "F#", -2 }, { "Gb", -2 },
            { "G", -1 },
            { "G#", 0 }, { "Ab", 0 }
        };
        private int bpm;
        private readonly List<Note> notes; // Why can I still add values now? Or is it about the values within being immutable?

        public ToneCulator() { notes = new List<Note>(); }
        /// <summary>
        /// Takes a string that denotes a note, splits it up into the the base note and the octave and uses that to calculate the proper frequency
        /// </summary>
        /// <param name="note">Note, such as "C#4", to  be translated into a frequency</param>
        /// <returns>Frequency of given note. For example, for C#4, it's 277</returns>
        private int CalculateFrequency(string note)
        {
            int index = 0;
            for (int i = 0; i < note.Length; ++i)
            {
                if (note[i] >= '0' && note[i] <= '9')
                {
                    index = noteIndices[note.Substring(0, i)] + int.Parse(note.Substring(i, note.Length - i)) * 12;
                    break;
                }
            }
            int f = (int)(440 * (Math.Pow((Math.Pow(2.0, 1.0 / 12.0)), (index - 49)))); // 12-step octave frequency formula
            Console.WriteLine(note + " " + f + " Hz");
            return f;
        }
        /// <summary>
        /// Play the loaded song.
        /// </summary>
        public void Play()
        {
            foreach(Note n in notes)
                Console.Beep(n.Tone, n.Duration);
        }
        /// <summary>
        /// Convert the 
        /// </summary>
        /// <param name="file">The (path and) name of the file that contains the music to played</param>
        public void ReadFile(string file)
        {
            // TODO: strip out spaces and newlines, garbage in general, error handling (incorrect input)
            string[] lines = System.IO.File.ReadAllLines(file);
            bpm = int.Parse(lines[0]);
            string note = "";
            string duration;
            bool is1stPt = true;
            int startindex; // See two lines above.
            for (int i = 1; i < lines.Length; ++i)
            {
                startindex = 0;
                for (int j = 0; j < lines[i].Length; ++j)
                {
                    if (lines[i][j] == ';') // note complete, next one. id should be false: save duration and put note and duration in dict
                    {
                        duration = lines[i].Substring(startindex, j - startindex);
                        notes.Add(new Note(CalculateFrequency(note), (int)(60000/((double)bpm/4*int.Parse(duration)))));
                        startindex = j + 1;
                        is1stPt = !is1stPt;
                        if (!is1stPt) ; // something's wrong
                    }
                    if (lines[i][j] == ':') // switch from note to duration: save note and set id to false, startindex to current+1
                    {
                        note = lines[i].Substring(startindex, j - startindex);
                        startindex = j + 1;
                        is1stPt = !is1stPt;
                        if (is1stPt) ; // something's wrong
                    }
                }
            }
        }
    }

}
