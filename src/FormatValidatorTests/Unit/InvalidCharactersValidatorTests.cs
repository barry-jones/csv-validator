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
    public class InvalidCharactersValidatorTests
    {
        [TestMethod]
        public void InvalidCharactersValidator_WhenContains_IsNotValid()
        {
            const string INPUT = "a test |string";
            const bool EXPECTED = false;

            InvalidCharactersValidator validator = new InvalidCharactersValidator();
            validator.Characters = new List<Char> { '|' };

            bool result = validator.IsValid(INPUT);

            Assert.AreEqual(EXPECTED, result);
        }

        [TestMethod]
        public void InvalidCharactersValidator_WhenDoesntContain_IsValid()
        {
            const string INPUT = "a test string";
            const bool EXPECTED = true;

            InvalidCharactersValidator validator = new InvalidCharactersValidator();
            validator.Characters = new List<Char> { '|' };

            bool result = validator.IsValid(INPUT);

            Assert.AreEqual(EXPECTED, result);
        }

        [TestMethod]
        public void InvalidCharactersValidator_WhenHasInvalidCharsMultipleDefined_IsInvalid()
        {
            const string INPUT = "a te#st |string";
            const bool EXPECTED = false;

            InvalidCharactersValidator validator = new InvalidCharactersValidator();
            validator.Characters.AddRange(new char[] { '|', '#' });

            bool result = validator.IsValid(INPUT);

            Assert.AreEqual(EXPECTED, result);
        }

        [TestMethod]
        public void InvalidCharactersValidator_WhenInvalid_ReportsErrors()
        {
            const string INPUT = "a te#st |string";
            const bool EXPECTED = false;
            const int EXPECTED_ERRORCOUNT = 2;
            const string EXPECTED_ERRORS = "'#' at 5, '|' at 9";

            InvalidCharactersValidator validator = new InvalidCharactersValidator();
            validator.Characters.AddRange(new char[] { '#', '|' });

            bool result = validator.IsValid(INPUT);
            IList<ValidationError> errors = validator.GetErrors();

            Assert.AreEqual(EXPECTED, result);
            Assert.AreEqual(EXPECTED_ERRORCOUNT, errors.Count);

            Assert.AreEqual(5, errors[0].At);
            Assert.AreEqual("'#' invalid character found.", errors[0].Message);

            Assert.AreEqual(9, errors[1].At);
            Assert.AreEqual("'|' invalid character found.", errors[1].Message);
        }
    }
}
