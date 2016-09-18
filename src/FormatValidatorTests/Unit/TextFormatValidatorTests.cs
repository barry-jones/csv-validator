using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FormatValidator.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FormatValidatorTests.Unit
{
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
    }
}
