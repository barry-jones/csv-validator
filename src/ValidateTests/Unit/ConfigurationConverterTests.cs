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
    public class ConfigurationConverterTests
    {
        private ValidatorConfiguration _configuration;
        private ConfigurationConvertor _converter;

        [TestInitialize]
        public void Setup()
        {
            _configuration = new ValidatorConfiguration();
            _converter = new ConfigurationConvertor(_configuration);
        }

        [TestMethod]
        public void Converter_WhenEmptyConfiguration_ReturnsEmptyList()
        {
            ConvertedValidators validators = _converter.Convert();

            Assert.AreEqual(0, validators.Columns.Count);
        }

        [TestMethod]
        public void Converter_WhenHasColumnConfiguration_HasColumns()
        {
            _configuration.Columns.Add(1, new ColumnValidatorConfiguration() { Unique = true });
            _configuration.Columns.Add(3, new ColumnValidatorConfiguration() { MaxLength = 10 });
            _configuration.Columns.Add(9, new ColumnValidatorConfiguration() { Unique = true, MaxLength = 10 });

            ConvertedValidators validators = _converter.Convert();

            Assert.AreEqual(3, validators.Columns.Count);
        }

        [TestMethod]
        public void Converter_WhenColumnHasMultipleValidators_ShouldHaveCorrectCount()
        {
            const int EXPECTED_COUNT_ONE = 1;
            const int EXPECTED_COUNT_TWO = 1;
            const int EXPECTED_COUNT_THREE = 2;

            _configuration.Columns.Add(1, new ColumnValidatorConfiguration() { Unique = true });
            _configuration.Columns.Add(3, new ColumnValidatorConfiguration() { MaxLength = 10 });
            _configuration.Columns.Add(9, new ColumnValidatorConfiguration() { Unique = true, MaxLength = 10 });

            ConvertedValidators validators = _converter.Convert();

            Assert.AreEqual(EXPECTED_COUNT_ONE, validators.Columns[1].Count());
            Assert.AreEqual(EXPECTED_COUNT_TWO, validators.Columns[3].Count());
            Assert.AreEqual(EXPECTED_COUNT_THREE, validators.Columns[9].Count());
        }

        [TestMethod]
        public void Converter_WhenColumnHasPatternAttribute_ShouldCreateValidator()
        {
            _configuration.Columns.Add(1, new ColumnValidatorConfiguration() { Pattern = @"\d\d\d\d" });

            ConvertedValidators validators = _converter.Convert();

            Assert.IsNotNull(validators.Columns[1].Find(p => p.GetType() == typeof(TextFormatValidator)));
        }

        [TestMethod]
        public void Converter_WhenColumnHasIsNumericAttribute_ShouldCreateValidator()
        {
            _configuration.Columns.Add(1, new ColumnValidatorConfiguration() { IsNumeric = true });

            ConvertedValidators validators = _converter.Convert();

            Assert.IsNotNull(validators.Columns[1].Find(p => p.GetType() == typeof(NumberValidator)));
        }

        [TestMethod]
        public void Converter_WhenColumnHasNullableAttribute_ShouldCreateValidator()
        {
            _configuration.Columns.Add(1, new ColumnValidatorConfiguration() { IsRequired = true });

            ConvertedValidators validators = _converter.Convert();

            Assert.IsNotNull(validators.Columns[1].Find(p => p.GetType() == typeof(NotNullableValidator)));
        }

        [TestMethod]
        public void Converter_WhenRowSeperatorWithNewLine_UnescapesTheString()
        {
            const string EXPECTED_SEPERATOR = @"##@#
";
            
            _configuration.RowSeperator = @"##@#\r\n";

            ConvertedValidators validators = _converter.Convert();

            Assert.AreEqual(EXPECTED_SEPERATOR, validators.RowSeperator);
        }

        [TestMethod]
        public void Converter_WhenColumnSeperatorProvided_Converts()
        {
            const string EXPECTED_SEPERATOR = "|";
            const string INPUT = "|";

            _configuration.ColumnSeperator = INPUT;

            ConvertedValidators converted = _converter.Convert();

            Assert.AreEqual(EXPECTED_SEPERATOR, converted.ColumnSeperator);
        }
    }
}
