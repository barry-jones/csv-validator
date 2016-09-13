using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FormatValidator.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FormatValidatorTests.Unit
{
    [TestClass]
    public class NumberValidatorTests
    {
        [TestMethod]
        public void NumberValidator_Create()
        {
            NumberValidator validator = new NumberValidator();
        }

        [TestMethod]
        public void NumberValidator_WhenNumeric_IsValid()
        {
            const string INPUT = "1";
            const bool EXPECTED_RESULT = true;

            NumberValidator validator = new NumberValidator();

            bool result = validator.IsValid(INPUT);

            Assert.AreEqual(EXPECTED_RESULT, result);
        }

        [TestMethod]
        public void NumberValidator_WhenLargeNumber_IsValid()
        {
            const string INPUT = "1234567890123456789012345678901234567890";
            const bool EXPECTED_RESULT = true;

            NumberValidator validator = new NumberValidator();

            bool result = validator.IsValid(INPUT);

            Assert.AreEqual(EXPECTED_RESULT, result);
        }

        [TestMethod]
        public void NumberValidator_WhenFloatingPointNumber_IsValid()
        {
            const string INPUT = "1234567890123456789012345.678901234567890";
            const bool EXPECTED_RESULT = true;

            NumberValidator validator = new NumberValidator();

            bool result = validator.IsValid(INPUT);

            Assert.AreEqual(EXPECTED_RESULT, result);
        }
    }
}
