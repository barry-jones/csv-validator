
namespace FormatValidatorTests.Unit
{
    using FormatValidator;
    using FormatValidator.Validators;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TextFormatValidatorTests
    {
        [TestMethod]
        public void Format_WhenValidInput_IsValid()
        {
            const string INPUT = "2016-09-01 12:13:00";
            const string FORMAT = @"\d\d\d\d-\d\d-\d\d \d\d:\d\d:\d\d";
            const bool EXPECTED_RESULT = true;

            TextFormatValidator validator = new TextFormatValidator(FORMAT);
            bool result = validator.IsValid(INPUT);

            Assert.AreEqual(EXPECTED_RESULT, result);
        }

        [TestMethod]
        public void Format_ValidContentInInvalidString_IsInvalid()
        {
            const string INPUT = "set 2016-09-01 12:13:00";
            const string FORMAT = @"\A\d\d\d\d-\d\d-\d\d \d\d:\d\d:\d\d\z";
            const bool EXPECTED_RESULT = false;

            TextFormatValidator validator = new TextFormatValidator(FORMAT);
            bool result = validator.IsValid(INPUT);

            Assert.AreEqual(EXPECTED_RESULT, result);
        }

        [TestMethod]
        public void Format_WhenInvalidInput_IsInvalid()
        {
            const string INPUT = "2016-09-01";
            const string FORMAT = @"\A\d\d\d\d-\d\d-\d\d \d\d:\d\d:\d\d\z";
            const bool EXPECTED_RESULT = false;

            TextFormatValidator validator = new TextFormatValidator(FORMAT);
            bool result = validator.IsValid(INPUT);

            Assert.AreEqual(EXPECTED_RESULT, result);
        }

        [TestMethod]
        public void Format_WhenValidInputOverMultipleLines_IsInvalid()
        {
            const string INPUT = @"2016-09-01
00:12:13";
            const string FORMAT = @"\A\d\d\d\d-\d\d-\d\d \d\d:\d\d:\d\d\z";
            const bool EXPECTED_RESULT = false;

            TextFormatValidator validator = new TextFormatValidator(FORMAT);
            bool result = validator.IsValid(INPUT);

            Assert.AreEqual(EXPECTED_RESULT, result);
        }

        [TestMethod]
        public void Format_WhenEmptyInputProvided_IsValid()
        {
            const string INPUT = "";
            const string FORMAT = @"\d\d";
            const bool EXPECTED_RESULT = true;

            TextFormatValidator validator = new TextFormatValidator(FORMAT);
            bool result = validator.IsValid(INPUT);

            Assert.AreEqual(EXPECTED_RESULT, result);
        }

        [TestMethod]
        public void Format_WhenInvalid_MessageContainsFormat()
        {
            const string INPUT = "aa";
            const string FORMAT = @"\d\d";
            const string EXPECTED_RESULT = "String 'aa' was not in correct format [\\d\\d].";

            TextFormatValidator validator = new TextFormatValidator(FORMAT);
            validator.IsValid(INPUT);
            ValidationError error = validator.GetErrors()[0];

            Assert.AreEqual(EXPECTED_RESULT, error.Message);
        }
    }
}
