
namespace FormatValidatorTests.Helpers
{
    using FormatValidator;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public static class ValidationErrorHelper
    {
        public static void CheckError(int expectedAt, string expectedMessage, ValidationError error)
        {
            Assert.AreEqual(expectedAt, error.AtCharacter);
            Assert.AreEqual(expectedMessage, error.Message);
        }
    }
}
