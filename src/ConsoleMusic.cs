using System;
using System.Collections.Generic;

namespace ConsoleMusic
{
    struct Note
    {
        private int tone;
        private int duration;
        /// <summary>
        /// Note constructor.
        /// </summary>
        /// <param name="tone">The frequency of the note in Hz</param>
        /// <param name="duration">The duration of the note in ms</param>
        public Note(int tone, int duration)
        {
            this.tone = tone;
            this.duration = duration;
        }

        public int Tone { get { return tone; } }
        public int Duration { get { return duration; } }
    }

    class ConsoleMusic
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

        /// <summary>
        /// Constructor. Initialise the list that will contain all notes to be played.
        /// </summary>
        public ConsoleMusic() { notes = new List<Note>(); }
        /// <summary>
        /// Takes a string that denotes a note, splits it up into the the base note and the octave and uses that to calculate the proper frequency
        /// </summary>
        /// <param name="note">Note, such as "C#4", to  be translated into a frequency</param>
        /// <returns>Frequency of given note. For example, for C#4, it's 277</returns>
        private int CalculateFrequency(string note)
        {
            try
            {
                int index = 0;
                for (int i = 0; i < note.Length; ++i)
                {
                    if (note[i] >= '0' && note[i] <= '9') // if we find a number, it means we have passed the part indicating the tone and reached the part indicating the octave
                    {
                        index = noteIndices[note.Substring(0, i)] + int.Parse(note.Substring(i, note.Length - i)) * 12; // look up index of note
                        break;
                    }
                }
                int f = (int)(440 * (Math.Pow((Math.Pow(2.0, 1.0 / 12.0)), (index - 49)))); // 12-step octave frequency formula
                Console.WriteLine(note + " " + f + " Hz");
                return f;
            }
            catch
            {
                throw new ArithmeticException("Incorrect note \"" + note + "\".");
            }
        }
        /// <summary>
        /// Play the loaded song.
        /// </summary>
        public void Play()
        {
            foreach (Note n in notes)
                Console.Beep(n.Tone, n.Duration);
        }
        /// <summary>
        /// Convert the 
        /// </summary>
        /// <param name="file">The (path and) name of the file that contains the music to played</param>
        public void ReadFile(string file)
        {
            try
            {
                string[] lines = System.IO.File.ReadAllLines(file);
                bpm = int.Parse(lines[0]);
                string note = "";
                string duration;
                bool is1stPt = true;
                int startindex;
                for (int i = 1; i < lines.Length; ++i)
                {
                    startindex = 0;
                    for (int j = 0; j < lines[i].Length; ++j)
                    {
                        if (lines[i][j] == ';') // note complete, next one. id should be false: save duration and put note and duration in dict
                        {
                            duration = lines[i].Substring(startindex, j - startindex).Replace(" ", "");
                            notes.Add(new Note(CalculateFrequency(note), (int)(60000 / ((double)bpm / 4 * int.Parse(duration)))));
                            startindex = j + 1;
                            is1stPt = !is1stPt;
                            if (!is1stPt) ; // something's wrong
                        }
                        if (lines[i][j] == ':') // switch from note to duration: save note and set id to false, startindex to current+1
                        {
                            note = lines[i].Substring(startindex, j - startindex).Replace(" ", "");
                            startindex = j + 1;
                            is1stPt = !is1stPt;
                            if (is1stPt) ; // something's wrong
                        }
                    }
                }
            }
            catch
            {
                throw new Exception("Something went wrong. File could not be read correctly.");
            }
        }
    }
}
