
namespace FormatValidator.Input
{
    using System.Collections.Generic;
    using System.IO;

    public class FileSourceReader : ISourceReader
    {
        private string _file;

        public FileSourceReader(string file)
        {
            _file = file;
        }

        public IEnumerable<string> ReadLines(string rowSeperator)
        {
            // We are not sure if a line (terminated by a \r\n or \n) character is a line
            // for validation purposes. It is possible that some files will not contain new
            // lines in the row terminater, so we need to continue to read data from teh file
            // until the row seperator is located or the end of the file is reached.

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
