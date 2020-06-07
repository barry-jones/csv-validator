
namespace FormatValidator
{
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Reads CSV content from a Stream.
    /// </summary>
    /// <remarks>
    /// This reader will not close the stream, please manage resources.
    /// </remarks>
    public class StreamSourceReader : ISourceReader
    {
        private Stream _stream;

        public StreamSourceReader(Stream stream)
        {
            _stream = stream;
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

            using (StreamReader reader = new StreamReader(_stream))
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
