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
            _validator = new RowValidator(",");
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
            ValidationErrorHelper.CheckError(16, "Could not convert 'row' to a number.", errors.Errors[0]);
        }

        [TestMethod]
        public void RowValidator_WhenValidatingMultipleRows_ErrorsDontStackUp()
        {
            const string ROW1 = @"this5,notnull,a,row";
            const string ROW2 = @"this5,notnull,a,row";
            const int EXPECTED_ERRORCOUNT = 2;
            int count = 0;

            _validator.AddColumnValidator(1, new StringLengthValidator(4));

            count += CountAndClearErrors(ROW1);
            count += CountAndClearErrors(ROW2);

            Assert.AreEqual(EXPECTED_ERRORCOUNT, count);
        }

        [TestMethod]
        public void RowValidator_WhenValidatingMultipleRowsAndUnique_ErrorsDontStackUp()
        {
            const string ROW1 = @"this1,notnull,a,notuniqueid";
            const string ROW2 = @"this2,notnull,a,notuniqueid";
            const string ROW3 = @"this3,notnull,a,notuniqueid";
            const string ROW4 = @"this4,notnull,a,notuniqueid";
            const int EXPECTED_ERRORCOUNT = 7;

            int count = 0;
            
            _validator.AddColumnValidator(1, new StringLengthValidator(4));
            _validator.AddColumnValidator(4, new UniqueColumnValidator());

            count += CountAndClearErrors(ROW1);
            count += CountAndClearErrors(ROW2);
            count += CountAndClearErrors(ROW3);
            count += CountAndClearErrors(ROW4);

            Assert.AreEqual(EXPECTED_ERRORCOUNT, count);
        }

        [TestMethod]
        public void RowValidator_WhenDataIsInvalid_PositionOfErrorIsProvided()
        {
            const string ROW = @"nine,12/09/1999,,far too long";
            const int ERROR_ONE = 1;
            const int ERROR_TWO = 6;
            const int ERROR_THREE = 17;
            const int ERROR_FOUR = 18;

            RowValidationError error = null;

            _validator.AddColumnValidator(1, new NumberValidator());
            _validator.AddColumnValidator(2, new TextFormatValidator(@"^\d\d\d\d-\d\d-\d\d$"));
            _validator.AddColumnValidator(3, new NotNullableValidator());
            _validator.AddColumnValidator(4, new StringLengthValidator(6));

            _validator.IsValid(ROW);
            error = _validator.GetError();

            Assert.AreEqual(ERROR_ONE, error.Errors[0].AtCharacter);
            Assert.AreEqual(ERROR_TWO, error.Errors[1].AtCharacter);
            Assert.AreEqual(ERROR_THREE, error.Errors[2].AtCharacter);
            Assert.AreEqual(ERROR_FOUR, error.Errors[3].AtCharacter);
        }

        private RowValidationError GetAndClearRowError(string contentToCheck)
        {
            RowValidationError error = null;

            _validator.IsValid(contentToCheck);
            error = _validator.GetError();
            _validator.ClearErrors();

            return error;
        }

        private int CountAndClearErrors(string contentToCheck)
        {
            int count = 0;

            _validator.IsValid(contentToCheck);
            count = _validator.GetError().Errors.Count();
            _validator.ClearErrors();

            return count;
        }
    }
}
