
namespace FormatValidatorTests.Integration
{
    using System.Collections.Generic;
    using System.Linq;
    using FormatValidator;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class FunctionalTests
    {
        const int EXPECTED_ERRORCOUNT = 3;
        const int EXPECTED_TOTALERRORS = 6;
        const string CONFIG = @"data\configuration\testfile-configuration.json";

        [TestMethod, TestCategory("Functional")]
        public void Functional_SoftwareSupportsTheCommaSeperator()
        {
            TestColumnSeperator("|", @"data\columndelimiter-testfile-1.csv");
            TestColumnSeperator(",", @"data\columndelimiter-testfile-2.csv");
            TestColumnSeperator("[#]", @"data\columndelimiter-testfile-3.csv");
        }

        [TestMethod]
        public void Functional_SoftwareSupportsRowSeperators()
        {
            TestRowSeperator("@@#@", @"data\rowterminator-testfile-1.csv");
            TestRowSeperator("\r\n", @"data\rowterminator-testfile-2.csv");
        }

        [TestMethod]
        public void Function_WhenHeaderRows_ValidatesCorrectly()
        {
            const string FILE = @"data\headers-testfile-1.csv";
            int totalErrorCount = 0;
            Validator validator = Validator.FromJson(System.IO.File.ReadAllText(CONFIG));
            FileSourceReader source = new FileSourceReader(FILE);

            List<RowValidationError> errors = new List<RowValidationError>(validator.Validate(source));
            totalErrorCount = GetTotalErrorCount(errors);

            Assert.AreEqual(EXPECTED_ERRORCOUNT, errors.Count);
            Assert.AreEqual(EXPECTED_TOTALERRORS, totalErrorCount);
        }

        [TestMethod]
        public void Functional_WhenStrictColumnsAndHeaderHasWrongCount_HeaderIsReported()
        {
            const string STRICT_CONFIG = @"data\configuration\strictcolumns-configuration.json";
            const string FILE = @"data\strictcolumns-badheader-testfile.csv";

            Validator validator = Validator.FromJson(System.IO.File.ReadAllText(STRICT_CONFIG));
            FileSourceReader source = new FileSourceReader(FILE);

            List<RowValidationError> errors = new List<RowValidationError>(validator.Validate(source));

            // the header (row 1) has 5 columns where 4 are expected; body rows are valid 4-column rows
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(1, errors[0].Row);
            Assert.AreEqual("Expected 4 columns but found 5.", errors[0].Errors[0].Message);
        }

        [TestMethod]
        public void Functional_WhenStrictColumnsOff_SurplusColumnsPassSilently()
        {
            // The standard testfile-configuration.json does not enable strictColumns.
            // The headers-testfile-1.csv has a 5-column header/rows where 4 columns
            // are configured; behaviour must be identical to today (no count errors).
            Validator validator = Validator.FromJson(System.IO.File.ReadAllText(CONFIG));
            FileSourceReader source = new FileSourceReader(@"data\headers-testfile-1.csv");

            List<RowValidationError> errors = new List<RowValidationError>(validator.Validate(source));
            int totalErrorCount = GetTotalErrorCount(errors);

            Assert.AreEqual(EXPECTED_ERRORCOUNT, errors.Count);
            Assert.AreEqual(EXPECTED_TOTALERRORS, totalErrorCount);
        }

        [TestMethod, TestCategory("Functional")]
        public void Functional_WhenStrictColumnsAndConfigKeysOutOfOrder_UsesHighestIndexAsWidth()
        {
            // The expected width is the highest configured column index, not the
            // order the columns happen to be declared in. With strictColumns on,
            // a correctly-sized file (header + body) must report no count errors
            // even when the column keys are declared out of order. Guards both the
            // header and body paths against deriving width from declaration order.
            const string CONFIG_JSON = @"{
                ""columnSeperator"": "","",
                ""rowSeperator"": ""\n"",
                ""hasHeaderRow"": true,
                ""strictColumns"": true,
                ""columns"": { ""1"": {}, ""2"": {}, ""4"": {}, ""3"": {} }
            }";
            const string DATA = "h1,h2,h3,h4\na,b,c,d\n1,2,3,4";

            Validator validator = Validator.FromJson(CONFIG_JSON);
            using (System.IO.MemoryStream stream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(DATA)))
            {
                StreamSourceReader source = new StreamSourceReader(stream);

                List<RowValidationError> errors = new List<RowValidationError>(validator.Validate(source));

                Assert.AreEqual(0, errors.Count);
            }
        }

        private void TestColumnSeperator(string seperator, string testfile)
        {
            int totalErrorCount = 0;
            Validator validator = Validator.FromJson(System.IO.File.ReadAllText(CONFIG));
            FileSourceReader source = new FileSourceReader(testfile);

            validator.SetColumnSeperator(seperator);
            validator.SetRowSeperator("\r\n");

            List<RowValidationError> errors = new List<RowValidationError>(validator.Validate(source));
            totalErrorCount = GetTotalErrorCount(errors);

            Assert.AreEqual(EXPECTED_ERRORCOUNT, errors.Count);
            Assert.AreEqual(EXPECTED_TOTALERRORS, totalErrorCount);
        }

        private void TestRowSeperator(string seperator, string testfile)
        {
            int totalErrorCount = 0;
            Validator validator = Validator.FromJson(System.IO.File.ReadAllText(CONFIG));
            FileSourceReader source = new FileSourceReader(testfile);

            validator.SetRowSeperator(seperator);

            List<RowValidationError> errors = new List<RowValidationError>(validator.Validate(source));
            totalErrorCount = GetTotalErrorCount(errors);

            Assert.IsFalse(errors[0].Content.EndsWith(seperator));
            Assert.AreEqual(EXPECTED_ERRORCOUNT, errors.Count);
            Assert.AreEqual(EXPECTED_TOTALERRORS, totalErrorCount);
        }

        private int GetTotalErrorCount(List<RowValidationError> errors)
        {
            int count = 0;
            foreach(RowValidationError current in errors)
            {
                count += current.Errors.Count();
            }
            return count;
        }
    }
}
