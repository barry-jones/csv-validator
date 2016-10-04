using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FormatValidator;
using FormatValidator.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FormatValidatorTests.Unit
{
    [TestClass]
    public class StringLengthValidatorTests
    {
        [TestMethod]
        public void StringLengthValidator_WhenStringIsLongerThanMaxLength_IsValid()
        {
            const string INPUT = "a test string";
            const bool EXPECTED = false;

            StringLengthValidator validator = new StringLengthValidator(10);

            bool result = validator.IsValid(INPUT);

            Assert.AreEqual(EXPECTED, result);
        }

        [TestMethod]
        public void StringLengthValidator_WhenStringIsMaxLength_IsValid()
        {
            const string INPUT = "a test string";
            const bool EXPECTED = true;
            const int MAX_LENGTH = 13;

            StringLengthValidator validator = new StringLengthValidator(MAX_LENGTH);

            bool result = validator.IsValid(INPUT);

            Assert.AreEqual(EXPECTED, result);
        }

        [TestMethod]
        public void StringLengthValidator_WhenStringIsSmaller_IsValid()
        {
            const string INPUT = "a test";
            const bool EXPECTED = true;
            const int MAX_LENGTH = 13;

            StringLengthValidator validator = new StringLengthValidator(MAX_LENGTH);

            bool result = validator.IsValid(INPUT);

            Assert.AreEqual(EXPECTED, result);
        }
    }
}
