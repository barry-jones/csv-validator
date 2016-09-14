using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FormatValidator.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FormatValidatorTests.Unit
{
    [TestClass]
    public class RowValidatorTests
    {
        [TestMethod]
        public void RowValidator_ValidatesFirstColumn_IsInvalid()
        {
            const string ROW = @"this,is,a,row";
            const bool EXPECTED_RESULT = false;

            RowValidator validator = new RowValidator(',');

            validator.AddColumnValidator(1, new ValidatorGroup(new List<IValidator>() { new StringLengthValidator(3) }));

            bool result = validator.IsValid(ROW);

            Assert.AreEqual(EXPECTED_RESULT, result);
        }

        [TestMethod]
        public void RowValidator_ValidatesFirstColumn_IsValid()
        {
            const string ROW = @"this,is,a,row";
            const bool EXPECTED_RESULT = true;

            RowValidator validator = new RowValidator(',');

            validator.AddColumnValidator(1, new ValidatorGroup(new List<IValidator>() { new StringLengthValidator(4) }));

            bool result = validator.IsValid(ROW);

            Assert.AreEqual(EXPECTED_RESULT, result);
        }

        [TestMethod]
        public void RowValidator_ValidatesOnlySecondColumn_IsInvalid()
        {
            const string ROW = @"this,,a,row";
            const bool EXPECTED_RESULT = false;

            RowValidator validator = new RowValidator(',');

            validator.AddColumnValidator(2, new NotNullableValidator());
            validator.AddColumnValidator(4, new StringLengthValidator(4));

            bool result = validator.IsValid(ROW);

            Assert.AreEqual(EXPECTED_RESULT, result);
        }

        [TestMethod]
        public void RowValidator_ValidatesLastColumnOnly_IsValid()
        {
            const string ROW = @"this,,a,row";
            const bool EXPECTED_RESULT = true;

            RowValidator validator = new RowValidator(',');

            validator.AddColumnValidator(4, new StringLengthValidator(3));

            bool result = validator.IsValid(ROW);

            Assert.AreEqual(EXPECTED_RESULT, result);
        }

        [TestMethod]
        public void RowValidator_ValidatesAllColumns_IsInvalid()
        {
            const string ROW = @"this,,a,row";
            const bool EXPECTED_RESULT = false;

            RowValidator validator = new RowValidator(',');

            validator.AddColumnValidator(1, new StringLengthValidator(5));
            validator.AddColumnValidator(2, new NotNullableValidator());
            validator.AddColumnValidator(3, new TextFormatValidator(@"[b]"));
            validator.AddColumnValidator(4, new NumberValidator());

            bool result = validator.IsValid(ROW);

            Assert.AreEqual(EXPECTED_RESULT, result);
        }
    }
}
