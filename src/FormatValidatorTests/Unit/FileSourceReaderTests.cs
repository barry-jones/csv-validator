using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FormatValidator.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FormatValidatorTests.Unit
{
    [TestClass]
    public class FileSourceReaderTests
    {
        [TestMethod]
        public void FileSourceReader_Create()
        {
            const string FILE = @"d:\development\personal\formatvalidator\src\formatvalidatortests\data\simplefile.csv";
            const string SEPERATOR = "\r\n";

            FileSourceReader reader = new FileSourceReader(FILE, SEPERATOR);
            foreach(string line in reader.ReadLines())
            {
            }
        }
    }
}
