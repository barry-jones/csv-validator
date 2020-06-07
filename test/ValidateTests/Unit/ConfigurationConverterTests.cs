
namespace FormatValidatorTests.Unit
{
    using System.Linq;
    using FormatValidator;
    using FormatValidator.Validators;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        public void Converter_WhenColumnHasUniqueAttribute_ShouldCreateValidator()
        {
            _configuration.Columns.Add(1, new ColumnValidatorConfiguration() { Unique = true });

            ConvertedValidators validators = _converter.Convert();

            Assert.IsNotNull(validators.Columns[1][0] as UniqueColumnValidator);
        }

        [TestMethod]
        public void Converter_WhenColumnHasPatternAttribute_ShouldCreateValidator()
        {
            _configuration.Columns.Add(1, new ColumnValidatorConfiguration() { Pattern = @"\d\d\d\d" });

            ConvertedValidators validators = _converter.Convert();

            Assert.IsNotNull(validators.Columns[1][0] as TextFormatValidator);
        }

        [TestMethod]
        public void Converter_WhenColumnHasIsNumericAttribute_ShouldCreateValidator()
        {
            _configuration.Columns.Add(1, new ColumnValidatorConfiguration() { IsNumeric = true });

            ConvertedValidators validators = _converter.Convert();

            Assert.IsNotNull(validators.Columns[1][0] as NumberValidator);
        }

        [TestMethod]
        public void Converter_WhenColumnHasNullableAttribute_ShouldCreateValidator()
        {
            _configuration.Columns.Add(1, new ColumnValidatorConfiguration() { IsRequired = true });

            ConvertedValidators validators = _converter.Convert();

            Assert.IsNotNull(validators.Columns[1][0] as NotNullableValidator);
        }

        [TestMethod]
        public void Convertor_WhenColumnHasMaxLengthAttribute_ShouldCreateValidator()
        {
            _configuration.Columns.Add(1, new ColumnValidatorConfiguration() { MaxLength = 10 });

            ConvertedValidators validators = _converter.Convert();

            Assert.IsNotNull(validators.Columns[1][0] as StringLengthValidator);
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

        [TestMethod]
        public void Converter_WhenHasHeaderRowNotProvided_DefaultsToFalse()
        {
            ConvertedValidators converted = _converter.Convert();

            Assert.AreEqual(false, converted.HasHeaderRow);
        }

        [TestMethod]
        public void Converter_WhenHasHeaderRowProvided_ConvertsValue()
        {
            _configuration.HasHeaderRow = true;

            ConvertedValidators converted = _converter.Convert();

            Assert.AreEqual(true, converted.HasHeaderRow);
        }
    }
}
