
namespace FormatValidatorTests.Unit
{
    using FormatValidator.Validators;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class NumberValidatorTests
    {
        private NumberValidator _validator;

        [TestInitialize]
        public void Setup()
        {
            _validator = new NumberValidator();
        }

        [TestMethod]
        public void NumberValidator_WhenNumeric_IsValid()
        {
            const string INPUT = "1";
            const bool EXPECTED_RESULT = true;

            bool result = _validator.IsValid(INPUT);

            Assert.AreEqual(EXPECTED_RESULT, result);
        }

        [TestMethod]
        public void NumberValidator_WhenLargeNumber_IsValid()
        {
            const string INPUT = "1234567890123456789012345678901234567890";
            const bool EXPECTED_RESULT = true;

            bool result = _validator.IsValid(INPUT);

            Assert.AreEqual(EXPECTED_RESULT, result);
        }

        [TestMethod]
        public void NumberValidator_WhenFloatingPointNumber_IsValid()
        {
            const string INPUT = "1234567890123456789012345.678901234567890";
            const bool EXPECTED_RESULT = true;

            bool result = _validator.IsValid(INPUT);

            Assert.AreEqual(EXPECTED_RESULT, result);
        }

        [TestMethod]
        public void NumberValidator_WhenNotNumeric_IsInvalid()
        {
            const string INPUT = "test";
            const bool EXPECTED_RESULT = false;

            bool result = _validator.IsValid(INPUT);

            Assert.AreEqual(EXPECTED_RESULT, result);
        }

        [TestMethod]
        public void NumberValidator_WhenEmptyStringOrWhitespace_IsValid()
        {
            const string INPUT = "  ";
            const bool EXPECTED_RESULT = true;

            bool result = _validator.IsValid(INPUT);

            Assert.AreEqual(EXPECTED_RESULT, result);
        }
    }
}
