
namespace FormatValidatorTests.Unit
{
    using System.Collections.Generic;
    using System.Linq;
    using FormatValidator;
    using FormatValidator.Validators;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ValidatorTests
    {
        [TestMethod]
        public void Validator_Create()
        {
            Validator validator = new Validator();
        }

        [TestMethod]
        public void Validator_WhenCreatedFromJson_ReturnsAValidator()
        {
            string JSON = System.IO.File.ReadAllText(@"data\configuration\configuration.json");

            // act
            Validator created = Validator.FromJson(JSON);

            // assert
            List<ValidatorGroup> columns = created.GetColumnValidators();

            Assert.IsNotNull(created);
            Assert.AreEqual(3, columns.Count);
            Assert.AreEqual(2, columns[0].Count());
            Assert.AreEqual(1, columns[1].Count());
            Assert.AreEqual(1, columns[2].Count());
        }

        [TestMethod]
        public void Validator_WhenValidating_Validates()
        {
            string INPUTFILE = @"data\simplefile.csv";
            string JSON = System.IO.File.ReadAllText(@"data\configuration\configuration.json");

            Validator validator = Validator.FromJson(JSON);
            FileSourceReader reader = new FileSourceReader(INPUTFILE);

            List<RowValidationError> errors = new List<RowValidationError>(validator.Validate(reader));
        }

        [TestMethod]
        public void Validator_WhenRowInvalid_ShouldStoreRowInError()
        {
            string[] ROWS = {
                @"this1,",
                @"this2,"
            };

            FakeSourceReader source = new FakeSourceReader(ROWS);
            ValidatorConfiguration configuration = new ValidatorConfiguration();
            configuration.Columns.Add(2, new ColumnValidatorConfiguration { IsRequired = true });

            Validator validator = Validator.FromConfiguration(configuration);

            List<RowValidationError> errors = validator.Validate(source).ToList();

            Assert.AreEqual(1, errors[0].Row);
            Assert.AreEqual(2, errors[1].Row);
        }

        public class FakeSourceReader : ISourceReader
        {
            private string[] _rows;

            public FakeSourceReader(string[] rows)
            {
                _rows = rows;
            }

            public IEnumerable<string> ReadLines(string rowSeperator)
            {
                return _rows;
            }
        }
    }
}
