
namespace FormatValidatorTests.Unit
{
    using System.Collections.Generic;
    using FormatValidator.Validators;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AllowedValuesValidatorTests
    {
        private static readonly IReadOnlyList<string> AllowedList = new List<string> { "YES", "NO", "MAYBE" };

        [TestMethod]
        public void AllowedValuesValidator_WhenValueIsInAllowedList_IsValid()
        {
            AllowedValuesValidator validator = new AllowedValuesValidator(AllowedList);

            bool result = validator.IsValid("YES");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AllowedValuesValidator_WhenValueIsNotInAllowedList_IsNotValid()
        {
            AllowedValuesValidator validator = new AllowedValuesValidator(AllowedList);

            bool result = validator.IsValid("PERHAPS");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void AllowedValuesValidator_WhenValueIsNotInAllowedList_ProducesCorrectErrorMessage()
        {
            const string EXPECTED_MESSAGE = "Value 'PERHAPS' is not in the list of allowed values [YES, NO, MAYBE].";
            AllowedValuesValidator validator = new AllowedValuesValidator(AllowedList);

            validator.IsValid("PERHAPS");

            Assert.AreEqual(EXPECTED_MESSAGE, validator.GetErrors()[0].Message);
        }

        [TestMethod]
        public void AllowedValuesValidator_WhenValueIsEmpty_IsValid()
        {
            AllowedValuesValidator validator = new AllowedValuesValidator(AllowedList);

            bool result = validator.IsValid(string.Empty);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AllowedValuesValidator_WhenValueIsNull_IsValid()
        {
            AllowedValuesValidator validator = new AllowedValuesValidator(AllowedList);

            bool result = validator.IsValid(null);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AllowedValuesValidator_WhenValueMatchesExactCase_IsValid()
        {
            AllowedValuesValidator validator = new AllowedValuesValidator(AllowedList);

            bool result = validator.IsValid("NO");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AllowedValuesValidator_WhenValueDiffersByCase_IsNotValid()
        {
            AllowedValuesValidator validator = new AllowedValuesValidator(AllowedList);

            bool result = validator.IsValid("yes");

            Assert.IsFalse(result);
        }
    }
}
