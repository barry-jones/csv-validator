using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FormatValidator.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FormatValidatorTests.Unit
{
    [TestClass]
    public class ValidatorGroupTests
    {
        [TestMethod]
        public void Multiple_OnSingleValue_IsInvalid()
        {
            const string INPUT = "test this sting for a number";
            const bool EXPECTED_RESULT = false;

            ValidatorGroup validators = new ValidatorGroup();
            validators.Add(new StringLengthValidator(9));
            validators.Add(new NumberValidator());

            bool result = validators.IsValid(INPUT);

            Assert.AreEqual(EXPECTED_RESULT, result);
        }

        [TestMethod]
        public void Multiple_OnSingleValue_IsValid()
        {
            const string INPUT = "12345";
            const bool EXPECTED_RESULT = true;

            ValidatorGroup validators = new ValidatorGroup();
            validators.Add(new StringLengthValidator(9));
            validators.Add(new NumberValidator());

            bool result = validators.IsValid(INPUT);

            Assert.AreEqual(EXPECTED_RESULT, result);
        }

        [TestMethod]
        public void Multiple_WhenNoValidators_ReturnsTrue()
        {
            const string INPUT = "doesnt matter";
            const bool EXPECTED_VALUE = true;

            ValidatorGroup validators = new ValidatorGroup();

            bool result = validators.IsValid(INPUT);

            Assert.AreEqual(EXPECTED_VALUE, result);
        }
    }
}
