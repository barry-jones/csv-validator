using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FormatValidator.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FormatValidatorTests
{
    [TestClass]
    public class NotNullableValidatorTests
    {
        [TestMethod]
        public void NotNullableValidator_Create()
        {
            NotNullableValidator validator = new NotNullableValidator();
        }

        [TestMethod]
        public void NotNullableValidator_WhenEmptyString_IsInvalid()
        {
            const string INPUT = "";
            const bool EXPECTED_RESULT = false;

            NotNullableValidator validator = new NotNullableValidator();
            bool result = validator.IsValid(INPUT);

            Assert.AreEqual(EXPECTED_RESULT, result);
        }

        [TestMethod]
        public void NotNullableValidator_WhenNullString_IsInvalid()
        {
            const string INPUT = null;
            const bool EXPECTED_RESULT = false;

            NotNullableValidator validator = new NotNullableValidator();
            bool result = validator.IsValid(INPUT);

            Assert.AreEqual(EXPECTED_RESULT, result);
        }

        [TestMethod]
        public void NotNullableValidator_WhenNotNull_IsValid()
        {
            const string INPUT = " ";
            const bool EXPECTED_RESULT = true;

            NotNullableValidator validator = new NotNullableValidator();
            bool result = validator.IsValid(INPUT);

            Assert.AreEqual(EXPECTED_RESULT, result);
        }
    }
}
