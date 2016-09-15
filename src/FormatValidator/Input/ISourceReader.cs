using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormatValidator.Input
{
    interface ISourceReader
    {
        IEnumerable<string> ReadLines();
    }
}
