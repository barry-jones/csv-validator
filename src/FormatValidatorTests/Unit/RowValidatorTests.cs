using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FormatValidator;
using FormatValidator.Validators;
using FormatValidatorTests.Helpers;
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
            const int EXPECTED_ERRORCOUNT = 3;

            RowValidator validator = new RowValidator(',');

            validator.AddColumnValidator(1, new StringLengthValidator(5));
            validator.AddColumnValidator(2, new NotNullableValidator());
            validator.AddColumnValidator(3, new TextFormatValidator(@"[b]"));
            validator.AddColumnValidator(4, new NumberValidator());

            bool result = validator.IsValid(ROW);
            IList<ValidationError> errors = validator.GetErrors();

            Assert.AreEqual(EXPECTED_RESULT, result);
            Assert.AreEqual(EXPECTED_ERRORCOUNT, errors.Count);
        }

        [TestMethod]
        public void RowValidator_ValidatesAllColumns_HasMultipleErrors()
        {
            const string ROW = @"this,,a,row";
            const bool EXPECTED_RESULT = false;
            const int EXPECTED_ERRORCOUNT = 4;

            RowValidator validator = new RowValidator(',');

            validator.AddColumnValidator(1, new StringLengthValidator(2));
            validator.AddColumnValidator(2, new NotNullableValidator());
            validator.AddColumnValidator(3, new TextFormatValidator(@"[b]"));
            validator.AddColumnValidator(4, new NumberValidator());

            bool result = validator.IsValid(ROW);
            IList<ValidationError> errors = validator.GetErrors();

            Assert.AreEqual(EXPECTED_RESULT, result);
            Assert.AreEqual(EXPECTED_ERRORCOUNT, errors.Count);
        }

        [TestMethod]
        public void RowValidator_ValidatesAllColumns_HasAnErrorOnLastColumn()
        {
            const string ROW = @"this,notnull,a,row";
            const bool EXPECTED_RESULT = false;
            const int EXPECTED_ERRORCOUNT = 1;

            RowValidator validator = new RowValidator(',');

            validator.AddColumnValidator(1, new StringLengthValidator(4));
            validator.AddColumnValidator(2, new NotNullableValidator());
            validator.AddColumnValidator(3, new TextFormatValidator(@"[a]"));
            validator.AddColumnValidator(4, new NumberValidator());

            bool result = validator.IsValid(ROW);
            IList<ValidationError> errors = validator.GetErrors();

            Assert.AreEqual(EXPECTED_RESULT, result);
            Assert.AreEqual(EXPECTED_ERRORCOUNT, errors.Count);
            ValidationErrorHelper.CheckError(0, "Could not convert 'row' to a number.", errors[0]);
        }

        [TestMethod]
        public void RowValidator_WhenValidatingMultipleRows_ErrorsDontStackUp()
        {
            const string ROW1 = @"this5,notnull,a,row";
            const string ROW2 = @"this5,notnull,a,row";
            const int EXPECTED_ERRORCOUNT = 2;
            List<ValidationError> errors = new List<ValidationError>();

            RowValidator validator = new RowValidator(',');
            validator.AddColumnValidator(1, new StringLengthValidator(4));
            
            validator.IsValid(ROW1);
            errors.AddRange(validator.GetErrors());
            validator.ClearErrors();
            validator.IsValid(ROW2);
            errors.AddRange(validator.GetErrors());

            Assert.AreEqual(EXPECTED_ERRORCOUNT, errors.Count);
        }

        [TestMethod]
        public void RowValidator_WhenValidatingMultipleRowsAndUnique_ErrorsDontStackUp()
        {
            const string ROW1 = @"this1,notnull,a,notuniqueid";
            const string ROW2 = @"this2,notnull,a,notuniqueid";
            const string ROW3 = @"this3,notnull,a,notuniqueid";
            const string ROW4 = @"this4,notnull,a,notuniqueid";
            const int EXPECTED_ERRORCOUNT = 7;
            List<ValidationError> errors = new List<ValidationError>();

            RowValidator validator = new RowValidator(',');
            validator.AddColumnValidator(1, new StringLengthValidator(4));
            validator.AddColumnValidator(4, new UniqueColumnValidator());

            validator.IsValid(ROW1);
            errors.AddRange(validator.GetErrors());
            validator.ClearErrors();
            validator.IsValid(ROW2);
            errors.AddRange(validator.GetErrors());
            validator.ClearErrors();
            validator.IsValid(ROW3);
            errors.AddRange(validator.GetErrors());
            validator.ClearErrors();
            validator.IsValid(ROW4);
            errors.AddRange(validator.GetErrors());
            validator.ClearErrors();

            Assert.AreEqual(EXPECTED_ERRORCOUNT, errors.Count);
        }
    }
}
