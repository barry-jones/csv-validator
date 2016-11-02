
namespace FormatValidatorTests.Unit
{
    using System.Collections.Generic;
    using FormatValidator;
    using FormatValidator.Validators;
    using FormatValidatorTests.Helpers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class InvalidCharactersValidatorTests
    {
        private InvalidCharactersValidator _validator;

        [TestInitialize]
        public void Setup()
        {
            _validator = new InvalidCharactersValidator();
        }

        [TestMethod]
        public void InvalidCharactersValidator_WhenContains_IsNotValid()
        {
            const string INPUT = "a test |string";
            const bool EXPECTED = false;

            _validator.Add(new char[] { '|' });

            bool result = _validator.IsValid(INPUT);

            Assert.AreEqual(EXPECTED, result);
        }

        [TestMethod]
        public void InvalidCharactersValidator_WhenDoesntContain_IsValid()
        {
            const string INPUT = "a test string";
            const bool EXPECTED = true;

            _validator.Add(new char[] { '|' });

            bool result = _validator.IsValid(INPUT);

            Assert.AreEqual(EXPECTED, result);
        }

        [TestMethod]
        public void InvalidCharactersValidator_WhenHasInvalidCharsMultipleDefined_IsInvalid()
        {
            const string INPUT = "a te#st |string";
            const bool EXPECTED = false;

            _validator.Add(new char[] { '|', '#' });

            bool result = _validator.IsValid(INPUT);

            Assert.AreEqual(EXPECTED, result);
        }

        [TestMethod]
        public void InvalidCharactersValidator_WhenInvalid_ReportsErrors()
        {
            const string INPUT = "a te#st |string";
            const bool EXPECTED = false;
            const int EXPECTED_ERRORCOUNT = 2;

            _validator.Add(new char[] { '#', '|' });

            bool result = _validator.IsValid(INPUT);
            IList<ValidationError> errors = _validator.GetErrors();

            Assert.AreEqual(EXPECTED, result);
            Assert.AreEqual(EXPECTED_ERRORCOUNT, errors.Count);
            ValidationErrorHelper.CheckError(5, "'#' invalid character found.", errors[0]);
            ValidationErrorHelper.CheckError(9, "'|' invalid character found.", errors[1]);
        }
    }
}
