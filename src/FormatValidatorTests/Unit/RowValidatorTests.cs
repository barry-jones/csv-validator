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
        private RowValidator _validator;

        [TestInitialize]
        public void Setup()
        {
            _validator = new RowValidator(',');
        }

        [TestMethod]
        public void RowValidator_WhenGettingColumnValidators_ReturnsValidators()
        {
            _validator.AddColumnValidator(1, new ValidatorGroup(new List<IValidator>() { new StringLengthValidator(3) }));
            _validator.AddColumnValidator(2, new NotNullableValidator());
            _validator.AddColumnValidator(4, new StringLengthValidator(4));

            List<ValidatorGroup> columnValidators = _validator.GetColumnValidators();

            Assert.AreEqual(4, columnValidators.Count);
        }

        [TestMethod]
        public void RowValidator_ValidatesFirstColumn_IsInvalid()
        {
            const string ROW = @"this,is,a,row";
            const bool EXPECTED_RESULT = false;

            _validator.AddColumnValidator(1, new ValidatorGroup(new List<IValidator>() { new StringLengthValidator(3) }));

            bool result = _validator.IsValid(ROW);

            Assert.AreEqual(EXPECTED_RESULT, result);
        }

        [TestMethod]
        public void RowValidator_ValidatesFirstColumn_IsValid()
        {
            const string ROW = @"this,is,a,row";
            const bool EXPECTED_RESULT = true;

            _validator.AddColumnValidator(1, new ValidatorGroup(new List<IValidator>() { new StringLengthValidator(4) }));

            bool result = _validator.IsValid(ROW);

            Assert.AreEqual(EXPECTED_RESULT, result);
        }

        [TestMethod]
        public void RowValidator_ValidatesOnlySecondColumn_IsInvalid()
        {
            const string ROW = @"this,,a,row";
            const bool EXPECTED_RESULT = false;

            _validator.AddColumnValidator(2, new NotNullableValidator());
            _validator.AddColumnValidator(4, new StringLengthValidator(4));

            bool result = _validator.IsValid(ROW);

            Assert.AreEqual(EXPECTED_RESULT, result);
        }

        [TestMethod]
        public void RowValidator_ValidatesLastColumnOnly_IsValid()
        {
            const string ROW = @"this,,a,row";
            const bool EXPECTED_RESULT = true;

            _validator.AddColumnValidator(4, new StringLengthValidator(3));

            bool result = _validator.IsValid(ROW);

            Assert.AreEqual(EXPECTED_RESULT, result);
        }

        [TestMethod]
        public void RowValidator_ValidatesAllColumns_IsInvalid()
        {
            const string ROW = @"this,,a,row";
            const bool EXPECTED_RESULT = false;
            const int EXPECTED_ERRORCOUNT = 3;

            _validator.AddColumnValidator(1, new StringLengthValidator(5));
            _validator.AddColumnValidator(2, new NotNullableValidator());
            _validator.AddColumnValidator(3, new TextFormatValidator(@"[b]"));
            _validator.AddColumnValidator(4, new NumberValidator());

            bool result = _validator.IsValid(ROW);
            RowValidationError errors = _validator.GetError();

            Assert.AreEqual(EXPECTED_RESULT, result);
            Assert.AreEqual(EXPECTED_ERRORCOUNT, errors.Errors.Count);
        }

        [TestMethod]
        public void RowValidator_ValidatesAllColumns_HasMultipleErrors()
        {
            const string ROW = @"this,,a,row";
            const bool EXPECTED_RESULT = false;
            const int EXPECTED_ERRORCOUNT = 4;

            _validator.AddColumnValidator(1, new StringLengthValidator(2));
            _validator.AddColumnValidator(2, new NotNullableValidator());
            _validator.AddColumnValidator(3, new TextFormatValidator(@"[b]"));
            _validator.AddColumnValidator(4, new NumberValidator());

            bool result = _validator.IsValid(ROW);
            RowValidationError errors = _validator.GetError();

            Assert.AreEqual(EXPECTED_RESULT, result);
            Assert.AreEqual(EXPECTED_ERRORCOUNT, errors.Errors.Count);
        }

        [TestMethod]
        public void RowValidator_ValidatesAllColumns_HasAnErrorOnLastColumn()
        {
            const string ROW = @"this,notnull,a,row";
            const bool EXPECTED_RESULT = false;
            const int EXPECTED_ERRORCOUNT = 1;

            _validator.AddColumnValidator(1, new StringLengthValidator(4));
            _validator.AddColumnValidator(2, new NotNullableValidator());
            _validator.AddColumnValidator(3, new TextFormatValidator(@"[a]"));
            _validator.AddColumnValidator(4, new NumberValidator());

            bool result = _validator.IsValid(ROW);
            RowValidationError errors = _validator.GetError();

            Assert.AreEqual(EXPECTED_RESULT, result);
            Assert.AreEqual(EXPECTED_ERRORCOUNT, errors.Errors.Count);
            ValidationErrorHelper.CheckError(0, "Could not convert 'row' to a number.", errors.Errors[0]);
        }

        [TestMethod]
        public void RowValidator_WhenValidatingMultipleRows_ErrorsDontStackUp()
        {
            const string ROW1 = @"this5,notnull,a,row";
            const string ROW2 = @"this5,notnull,a,row";
            const int EXPECTED_ERRORCOUNT = 2;
            List<ValidationError> errors = new List<ValidationError>();

            _validator.AddColumnValidator(1, new StringLengthValidator(4));

            CountAndClearErrors(ROW1, errors);
            CountAndClearErrors(ROW2, errors);

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
            
            _validator.AddColumnValidator(1, new StringLengthValidator(4));
            _validator.AddColumnValidator(4, new UniqueColumnValidator());

            CountAndClearErrors(ROW1, errors);
            CountAndClearErrors(ROW2, errors);
            CountAndClearErrors(ROW3, errors);
            CountAndClearErrors(ROW4, errors);

            Assert.AreEqual(EXPECTED_ERRORCOUNT, errors.Count);
        }

        [TestMethod]
        public void RowValidator_WhenRowInvalid_ShouldStoreRowInError()
        {
            const string ROW1 = @"this1,notnull";
            const string ROW2 = @"this2,notnull";

            List<ValidationError> errors = new List<ValidationError>();

            _validator.AddColumnValidator(2, new UniqueColumnValidator());

            CountAndClearErrors(ROW1, errors);
            CountAndClearErrors(ROW2, errors);

            Assert.AreEqual(ROW2, errors[0].RowContent);
        }

        private void CountAndClearErrors(string input, List<ValidationError> errors)
        {
            _validator.IsValid(input);
            errors.AddRange(_validator.GetError().Errors);
            _validator.ClearErrors();
        }
    }
}
