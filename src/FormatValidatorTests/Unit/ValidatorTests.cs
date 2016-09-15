using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FormatValidator;
using FormatValidator.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FormatValidatorTests.Unit
{
    [TestClass]
    public class ValidatorTests
    {
        [TestMethod]
        public void Validator_Create()
        {
            Validator validator = new Validator();
        }

        [TestMethod]
        public void Validator_WhenCreatedFromJson_ReturnsAValidator()
        {
            string JSON = System.IO.File.ReadAllText(@"data\configuration\configuration.json");

            // act
            Validator created = Validator.FromJson(JSON);

            // assert
            List<ValidatorGroup> columns = created.GetColumnValidators();

            Assert.IsNotNull(created);
            Assert.AreEqual(3, columns.Count);
            Assert.AreEqual(2, columns[0].Count());
            Assert.AreEqual(1, columns[1].Count());
            Assert.AreEqual(1, columns[2].Count());
        }
    }
}
