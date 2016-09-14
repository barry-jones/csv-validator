using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FormatValidator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FormatValidatorTests.Helpers
{
    public static class ValidationErrorHelper
    {
        public static void CheckError(int expectedAt, string expectedMessage, ValidationError error)
        {
            Assert.AreEqual(expectedAt, error.At);
            Assert.AreEqual(expectedMessage, error.Message);
        }
    }
}
