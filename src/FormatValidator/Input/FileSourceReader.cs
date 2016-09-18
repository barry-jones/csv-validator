using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormatValidator.Input
{
    public class FileSourceReader : ISourceReader
    {
        private string _file;

        public FileSourceReader(string file)
        {
            _file = file;
        }

        public IEnumerable<string> ReadLines(string rowSeperator)
        {
            /*
            foreach(string current in System.IO.File.ReadLines(_file))
            {
                yield return current;
            }*/

            List<char> readCharacters = new List<char>();
            Queue<char> seperatorCheckQueue = new Queue<char>();

            using (StreamReader reader = new StreamReader(System.IO.File.OpenRead(_file)))
            {
                while (reader.Peek() >= 0)
                {
                    char current = (char)reader.Read();
                    seperatorCheckQueue.Enqueue(current);

                    if(seperatorCheckQueue.Count > rowSeperator.Length)
                    {
                        readCharacters.Add(
                            seperatorCheckQueue.Dequeue()
                            );
                    }

                    if (new string(seperatorCheckQueue.ToArray()) == rowSeperator) // matches the rowSeperator
                    {
                        yield return new string(readCharacters.ToArray());
                        readCharacters.Clear();
                        seperatorCheckQueue.Clear();
                    }
                }

                // return the last set of characters as the last row may not contain the seperator
                if (readCharacters.Count > 0)
                    yield return new string(readCharacters.ToArray());
            }
        }
    }
}
