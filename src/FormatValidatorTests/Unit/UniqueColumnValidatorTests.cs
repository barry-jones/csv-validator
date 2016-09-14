using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FormatValidator.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FormatValidatorTests.Unit
{
    [TestClass]
    public class UniqueColumnValidatorTests
    {
        [TestMethod]
        public void UniqueColumnValidator_WhenTestingOnlyOnce_ReturnsTrue()
        {
            const string INPUT = "";
            const bool EXPECTED_RESULT = true;

            UniqueColumnValidator validator = new UniqueColumnValidator();

            bool result = validator.IsValid(INPUT);

            Assert.AreEqual(EXPECTED_RESULT, result);
            Assert.AreEqual(0, validator.GetErrors().Count);
        }

        [TestMethod]
        public void UniqueColumnValidator_WhenTestingSameEntryTwice_IsInvalid()
        {
            const string INPUT = "notunique";
            const bool EXPECTED_RESULT1 = true;
            const bool EXPECTED_RESULT2 = false;

            UniqueColumnValidator validator = new UniqueColumnValidator();

            bool result1 = validator.IsValid(INPUT);
            bool result2 = validator.IsValid(INPUT);

            Assert.AreEqual(EXPECTED_RESULT1, result1, "first");
            Assert.AreEqual(EXPECTED_RESULT2, result2, "second");
        }
    }
}
