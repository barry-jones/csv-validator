
namespace FormatValidatorTests.Integration
{
    using System.Collections.Generic;
    using FormatValidator;
    using FormatValidator.Validators;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class RowValidatorTests
    {
        [TestMethod]
        public void Integration_SimpleFileCheck()
        {
            const int EXPECTED_ERRORCOUNT = 5;

            string[] fileContents = System.IO.File.ReadAllLines(@"data\simplefile.csv");

            List<ValidationError> errors = new List<ValidationError>();
            RowValidator validator = new RowValidator(",");

            validator.AddColumnValidator(1, new UniqueColumnValidator());
            // no validator on 2
            validator.AddColumnValidator(3, new TextFormatValidator(@"^\d\d\d\d-\d\d-\d\d$"));
            validator.AddColumnValidator(4, new NotNullableValidator());
            validator.AddColumnValidator(5, new NumberValidator());

            foreach(string currentRow in fileContents)
            {
                if (!validator.IsValid(currentRow))
                {
                    errors.AddRange(validator.GetError().Errors);
                    validator.ClearErrors();
                }
            }

            Assert.AreEqual(EXPECTED_ERRORCOUNT, errors.Count);
        }
    }
}
