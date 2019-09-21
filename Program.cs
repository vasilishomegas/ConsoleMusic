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
        //private static Dictionary<string, int> tones = new Dictionary<string, int> {
        //    {"D1", 6 }, {"D#1", 7},  {"Eb1", 7},  {"E1", 8},  {"F1", 9}, { "F#1", 10}, { "Gb1", 10}, {"G1", 11},
        //    {"G#1", 12}, { "Ab1", 12}, {"A1", 13}, {"A#1", 14}, {"Bb1", 14}, {"B1", 15}, { "C2", 16}, {"C#2", 17}, {"Db2", 17},
        //    {"D2", 18}, {"D#2", 19}, {"Eb2", 19}, {"E2", 20}, {"F2", 21}, {"F#2", 22}, {"Gb2", 22}, {"G2", 23},
        //    {"G#2", 24 }, {"Ab2", 24}, {"A2", 25}, {"A#2", 26}, {"Bb2", 26}, {"B2", 27}, {"C3", 28 }, {"C#3", 29}, {"Db3", 29},
        //    {"D3", 30}, {"D#3", 31}, {"Eb3", 31}, {"E3", 32}, {"F3", 33}, {"F#3", 34}, {"Gb3", 34}, {"G3", 35},
        //    {"G#3", 36}, {"Ab3", 36}, {"A3", 37}, {"A#3", 38 }, {"Bb3", 38}, {"B3", 39}, {"C4", 40}, {"C#4", 41}, {"Db4", 41 },
        //    {"D4", 42}, {"D#4", 43}, {"Eb4", 43}, {"E4", 44}, {"F4", 45}, {"F#4", 46}, {"Gb4", 46}, {"G4", 47},
        //    {"G#4", 48}, {"Ab4", 48}, {"A4", 49}, {"A#4", 50}, {"Bb4", 50}, {"B4", 51}, {"C5", 52}, {"C#5", 53}, {"Db5", 53},
        //    {"D5", 54}, {"D#5", 55}, {"Eb5", 55}, {"E5", 56}, {"F5", 57}, {"F#5", 58}, {"Gb5", 58}, {"G5", 59},
        //    {"G#5", 60}, {"Ab5", 60}, {"A5", 61}, {"A#5", 62}, {"Bb5", 62}, {"B5", 63}, {"C6", 64}, {"C#6", 65}, {"Db6", 65},
        //    {"D6", 66}, {"D#6", 67}, {"Eb6", 67}, {"E6", 68}, {"F6", 69}, {"F#6", 70}, {"Gb6", 70}, {"G6", 71},
        //    {"G#6", 72}, {"Ab6", 72}, {"A6", 73}, {"A#6", 74}, {"Bb6", 74}, {"B6", 75}, {"C7", 76}, {"C#7", 77}, {"Db7", 77},
        //    {"D7", 78}, {"D#7", 79}, {"Eb7", 79}, {"E7", 80}, {"F7", 81}, {"F#7", 82}, {"Gb7", 82}, {"G7", 83},
        //    {"G#7", 84}, {"Ab7", 84}, {"A7", 85}, {"A#7", 86}, {"Bb7", 86}, {"B7", 87}, {"C8", 88}, {"C#8", 89}, {"Db8", 89},
        //    {"D8", 90}, {"D#8", 91}, {"Eb8", 91}, {"E8", 92}, {"F8", 93}, {"F#8", 94}, {"Gb8", 94}, {"G8", 95},
        //    {"G#8", 96}, {"Ab8", 96}, {"A8", 97}, {"A#8", 98}, {"Bb8", 98}, {"B8", 99}, {"C9", 100}, {"C#9", 101}, {"Db9", 101 },
        //    {"D9", 102}, {"D#9", 103}, {"Eb9", 103}, {"E9", 104}, {"F9", 105}, {"F#9", 106}, {"Gb9", 106}, {"G9", 107},
        //    {"G#9", 108}, {"Ab9", 108}, {"A9", 109}, {"A#9", 110}, {"Bb9", 110}, {"B9", 111}, {"C10", 112}, {"C#10", 113}, {"Db10", 113}
        //};

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
