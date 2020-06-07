
namespace FormatValidator
{
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Provides configurable access to a file source.
    /// </summary>
    public class FileSourceReader : ISourceReader
    {
        private string _file;

        /// <summary>
        /// Initialises a new instance of the FileSourceReader.
        /// </summary>
        /// <param name="file">The path to the file to read.</param>
        public FileSourceReader(string file)
        {
            _file = file;
        }

        /// <summary>
        /// Reads a line from the file, terminating with the <paramref name="rowSeperator"/>
        /// string.
        /// </summary>
        /// <param name="rowSeperator">The sequance of characters denoting a new line.</param>
        /// <returns>An enumerable list of rows from the file.</returns>
        public IEnumerable<string> ReadLines(string rowSeperator)
        {
            // read characters until row seperator string is located or the files ends.

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
