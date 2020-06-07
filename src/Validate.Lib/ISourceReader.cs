
namespace FormatValidator
{
    using System.Collections.Generic;

    public interface ISourceReader
    {
        IEnumerable<string> ReadLines(string rowSeperator);
    }
}
