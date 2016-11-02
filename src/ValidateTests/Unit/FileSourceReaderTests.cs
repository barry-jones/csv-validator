
namespace FormatValidatorTests.Unit
{
    using FormatValidator.Input;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class FileSourceReaderTests
    {
        [TestMethod]
        public void FileSourceReader_Create()
        {
            const string FILE = @"data\simplefile.csv";
            const string ROW_SEPERATOR = "\r\n";

            FileSourceReader reader = new FileSourceReader(FILE);
            foreach(string line in reader.ReadLines(ROW_SEPERATOR))
            {
            }
        }
    }
}
