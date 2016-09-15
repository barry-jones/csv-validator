using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormatValidator.Input
{
    public class FileSourceReader : ISourceReader
    {
        private string _file;
        private string _rowSeperator;

        public FileSourceReader(string file, string rowSeperator)
        {
            _file = file;
        }

        public IEnumerable<string> ReadLines()
        {
            return new List<string>();
        }
    }
}
