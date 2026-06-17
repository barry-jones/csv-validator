
namespace FormatValidatorTests.Unit
{
    using FormatValidator.Validators;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class StringMinLengthValidatorTests
    {
        [TestMethod]
        public void StringMinLengthValidator_WhenStringIsShorterThanMinLength_IsInvalid()
        {
            const string INPUT = "hi";
            const bool EXPECTED = false;

            StringMinLengthValidator validator = new StringMinLengthValidator(5);

            bool result = validator.IsValid(INPUT);

            Assert.AreEqual(EXPECTED, result);
        }

        [TestMethod]
        public void StringMinLengthValidator_WhenStringIsShorterThanMinLength_HasCorrectErrorMessage()
        {
            const string INPUT = "hi";
            const string EXPECTED_MESSAGE = "The content 'hi' is less than 5 characters";

            StringMinLengthValidator validator = new StringMinLengthValidator(5);
            validator.IsValid(INPUT);

            Assert.AreEqual(EXPECTED_MESSAGE, validator.GetErrors()[0].Message);
        }

        [TestMethod]
        public void StringMinLengthValidator_WhenStringIsExactlyMinLength_IsValid()
        {
            const string INPUT = "hello";
            const bool EXPECTED = true;

            StringMinLengthValidator validator = new StringMinLengthValidator(5);

            bool result = validator.IsValid(INPUT);

            Assert.AreEqual(EXPECTED, result);
        }

        [TestMethod]
        public void StringMinLengthValidator_WhenStringIsLongerThanMinLength_IsValid()
        {
            const string INPUT = "hello world";
            const bool EXPECTED = true;

            StringMinLengthValidator validator = new StringMinLengthValidator(5);

            bool result = validator.IsValid(INPUT);

            Assert.AreEqual(EXPECTED, result);
        }

        [TestMethod]
        public void StringMinLengthValidator_WhenStringIsEmpty_IsInvalid()
        {
            const string INPUT = "";
            const bool EXPECTED = false;

            StringMinLengthValidator validator = new StringMinLengthValidator(5);

            bool result = validator.IsValid(INPUT);

            Assert.AreEqual(EXPECTED, result);
        }

        [TestMethod]
        public void StringMinLengthValidator_WhenStringIsNull_IsInvalid()
        {
            const bool EXPECTED = false;

            StringMinLengthValidator validator = new StringMinLengthValidator(5);

            bool result = validator.IsValid(null);

            Assert.AreEqual(EXPECTED, result);
        }

        [TestMethod]
        public void StringMinLengthValidator_WhenStringIsNull_HasCorrectErrorMessage()
        {
            const string EXPECTED_MESSAGE = "The content '' is less than 5 characters";

            StringMinLengthValidator validator = new StringMinLengthValidator(5);
            validator.IsValid(null);

            Assert.AreEqual(EXPECTED_MESSAGE, validator.GetErrors()[0].Message);
        }
    }
}
