using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FormatValidator.Configuration;
using FormatValidator.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FormatValidatorTests.Unit
{
    [TestClass]
    public class ConverterTests
    {
        [TestMethod]
        public void Converter_WhenEmptyConfiguration_ReturnsEmptyList()
        {
            ValidatorConfiguration config = new ValidatorConfiguration();
            Converter converter = new Converter();

            ConvertedValidators validators = converter.Convert(config);

            Assert.AreEqual(0, validators.Columns.Count);
        }

        [TestMethod]
        public void Converter_WhenHasColumnConfiguration_HasColumns()
        {
            ValidatorConfiguration config = new ValidatorConfiguration();
            config.Columns.Add(1, new ColumnValidatorConfiguration() { Unique = true });
            config.Columns.Add(3, new ColumnValidatorConfiguration() { MaxLength = 10 });
            config.Columns.Add(9, new ColumnValidatorConfiguration() { Unique = true, MaxLength = 10 });

            Converter converter = new Converter();

            ConvertedValidators validators = converter.Convert(config);

            Assert.AreEqual(3, validators.Columns.Count);
        }

        [TestMethod]
        public void Converter_WhenColumnHasMultipleValidators_ShouldHaveCorrectCount()
        {
            const int EXPECTED_COUNT_ONE = 1;
            const int EXPECTED_COUNT_TWO = 1;
            const int EXPECTED_COUNT_THREE = 2;

            ValidatorConfiguration config = new ValidatorConfiguration();
            config.Columns.Add(1, new ColumnValidatorConfiguration() { Unique = true });
            config.Columns.Add(3, new ColumnValidatorConfiguration() { MaxLength = 10 });
            config.Columns.Add(9, new ColumnValidatorConfiguration() { Unique = true, MaxLength = 10 });

            Converter converter = new Converter();

            ConvertedValidators validators = converter.Convert(config);

            Assert.AreEqual(EXPECTED_COUNT_ONE, validators.Columns[1].Count());
            Assert.AreEqual(EXPECTED_COUNT_TWO, validators.Columns[3].Count());
            Assert.AreEqual(EXPECTED_COUNT_THREE, validators.Columns[9].Count());
        }
    }
}
