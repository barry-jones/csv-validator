﻿
namespace FormatValidatorTests.Integration
{
    using System.Collections.Generic;
    using FormatValidator;
    using FormatValidator.Validators;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ErrorInformationTests
    {
        private RowValidator _rowValidator;

        [TestInitialize]
        public void Setup()
        {
            _rowValidator = new RowValidator(",");
        }

        [TestMethod]
        public void WhenValidatingARow_ErrorsHaveRowContentInformation()
        {
            string[] ROWS = new string[] {
                "1,a name",
                "2,another name",
                "1,a name",
                "3,someone different"
            };

            _rowValidator.AddColumnValidator(1, new UniqueColumnValidator());
            List<RowValidationError> errors = ValidateRows(ROWS);

            Assert.AreEqual(ROWS[2], errors[0].Content);
        }

        private List<RowValidationError> ValidateRows(string[] rowsToValidate)
        {
            List<RowValidationError> errors = new List<RowValidationError>();

            foreach (string row in rowsToValidate)
            {
                if (!_rowValidator.IsValid(row))
                {
                    errors.Add(_rowValidator.GetError());
                    _rowValidator.ClearErrors();
                }
            }

            return errors;
        }
    }
}
