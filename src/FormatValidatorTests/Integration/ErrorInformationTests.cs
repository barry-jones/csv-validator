using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FormatValidator;
using FormatValidator.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FormatValidatorTests.Integration
{
    [TestClass]
    public class ErrorInformationTests
    {
        [TestMethod]
        public void WhenValidatingARow_ErrorsHaveRowInformation()
        {
            string[] ROWS = new string[] {
                "1",
                "1",
                "2",
                "1"
            };
            List<ValidationError> errors = new List<ValidationError>();

            RowValidator validator = new RowValidator(',');
            validator.AddColumnValidator(1, new UniqueColumnValidator());

            foreach (string current in ROWS)
            {
                if (!validator.IsValid(current))
                {
                    errors.AddRange(validator.GetErrors());
                    validator.ClearErrors();
                }
            }

            Assert.AreEqual(2, errors[0].Row);
            Assert.AreEqual(4, errors[1].Row);
        }
    }
}
