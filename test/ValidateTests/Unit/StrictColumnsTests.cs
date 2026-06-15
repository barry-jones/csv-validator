
namespace FormatValidatorTests.Unit
{
    using System.Collections.Generic;
    using FormatValidator;
    using FormatValidator.Validators;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class StrictColumnsTests
    {
        private RowValidator _validator;

        [TestInitialize]
        public void Setup()
        {
            _validator = new RowValidator(",");
            _validator.StrictColumns = true;

            // 4 columns defined (the largest configured index sets the width)
            _validator.AddColumnValidator(1, new ValidatorGroup(new List<IValidator>()));
            _validator.AddColumnValidator(2, new ValidatorGroup(new List<IValidator>()));
            _validator.AddColumnValidator(3, new ValidatorGroup(new List<IValidator>()));
            _validator.AddColumnValidator(4, new ValidatorGroup(new List<IValidator>()));
        }

        [TestMethod]
        public void StrictColumns_WhenRowHasTooManyColumns_ProducesError()
        {
            const string ROW = @"a,b,c,d,e"; // 5 columns, expected 4

            bool result = _validator.IsValid(ROW);
            RowValidationError error = _validator.GetError();

            Assert.IsFalse(result);
            Assert.AreEqual(1, error.Errors.Count);
            Assert.AreEqual("Expected 4 columns but found 5.", error.Errors[0].Message);
        }

        [TestMethod]
        public void StrictColumns_WhenRowHasTooFewColumns_ProducesError()
        {
            const string ROW = @"a,b,c"; // 3 columns, expected 4

            bool result = _validator.IsValid(ROW);
            RowValidationError error = _validator.GetError();

            Assert.IsFalse(result);
            Assert.AreEqual(1, error.Errors.Count);
            Assert.AreEqual("Expected 4 columns but found 3.", error.Errors[0].Message);
        }

        [TestMethod]
        public void StrictColumns_WhenRowHasExactColumnCount_ProducesNoCountError()
        {
            const string ROW = @"a,b,c,d"; // exactly 4

            bool result = _validator.IsValid(ROW);
            RowValidationError error = _validator.GetError();

            Assert.IsTrue(result);
            Assert.AreEqual(0, error.Errors.Count);
        }

        [TestMethod]
        public void StrictColumns_WhenTooWideRowAlsoHasContentErrors_ReportsBoth()
        {
            // column 1 must be numeric, so 'x' is a content error, AND the row is too wide
            RowValidator validator = new RowValidator(",");
            validator.StrictColumns = true;
            validator.AddColumnValidator(1, new NumberValidator());
            validator.AddColumnValidator(2, new ValidatorGroup(new List<IValidator>()));
            validator.AddColumnValidator(3, new ValidatorGroup(new List<IValidator>()));
            validator.AddColumnValidator(4, new ValidatorGroup(new List<IValidator>()));

            const string ROW = @"x,b,c,d,e"; // 5 columns AND column 1 not numeric

            bool result = validator.IsValid(ROW);
            RowValidationError error = validator.GetError();

            Assert.IsFalse(result);
            // one count error + one content error; the count error does not mask the others
            Assert.AreEqual(2, error.Errors.Count);
        }

        [TestMethod]
        public void StrictColumns_WhenSparseConfig_UsesLastIndexAsWidth()
        {
            // columns 1 and 4 defined, 2 and 3 omitted - expected width is 4
            RowValidator validator = new RowValidator(",");
            validator.StrictColumns = true;
            validator.AddColumnValidator(1, new ValidatorGroup(new List<IValidator>()));
            validator.AddColumnValidator(4, new ValidatorGroup(new List<IValidator>()));

            const string ROW = @"a,b,c,d"; // 4 columns - passes count check

            bool result = validator.IsValid(ROW);
            RowValidationError error = validator.GetError();

            Assert.IsTrue(result);
            Assert.AreEqual(0, error.Errors.Count);
        }

        [TestMethod]
        public void StrictColumns_WhenOff_SurplusColumnsPassSilently()
        {
            RowValidator validator = new RowValidator(",");
            // StrictColumns left at default (false)
            validator.AddColumnValidator(1, new ValidatorGroup(new List<IValidator>()));
            validator.AddColumnValidator(2, new ValidatorGroup(new List<IValidator>()));
            validator.AddColumnValidator(3, new ValidatorGroup(new List<IValidator>()));
            validator.AddColumnValidator(4, new ValidatorGroup(new List<IValidator>()));

            const string ROW = @"a,b,c,d,e,f"; // surplus columns

            bool result = validator.IsValid(ROW);
            RowValidationError error = validator.GetError();

            Assert.IsTrue(result);
            Assert.AreEqual(0, error.Errors.Count);
        }
    }
}
