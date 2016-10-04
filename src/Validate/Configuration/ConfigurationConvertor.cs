using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FormatValidator.Validators;

namespace FormatValidator.Configuration
{
    public class ConfigurationConvertor
    {
        private ConvertedValidators _converted;
        private ValidatorConfiguration _fromConfig;

        public ConfigurationConvertor(ValidatorConfiguration fromConfig)
        {
            _fromConfig = fromConfig;
            _converted = new ConvertedValidators();
        }

        public ConvertedValidators Convert()
        {
            _converted = new ConvertedValidators();
            
            ConvertProperties();
            ConvertColumns();

            return _converted;
        }

        private void ConvertProperties()
        {
            _converted.RowSeperator = UnescapeString(_fromConfig.RowSeperator);
            _converted.ColumnSeperator = UnescapeString(_fromConfig.ColumnSeperator);
        }

        private void ConvertColumns()
        {
            if (ConfigHasColumns())
            {
                foreach (KeyValuePair<int, ColumnValidatorConfiguration> columnConfig in _fromConfig.Columns)
                {
                    List<IValidator> group = new List<IValidator>();

                    if (columnConfig.Value.Unique) group.Add(new UniqueColumnValidator());
                    if (columnConfig.Value.MaxLength > 0) group.Add(new StringLengthValidator(columnConfig.Value.MaxLength));
                    if (!string.IsNullOrWhiteSpace(columnConfig.Value.Pattern)) group.Add(new TextFormatValidator(columnConfig.Value.Pattern));
                    if (columnConfig.Value.IsNumeric) group.Add(new NumberValidator());
                    if (columnConfig.Value.IsRequired) group.Add(new NotNullableValidator());

                    _converted.Columns.Add(columnConfig.Key, group);
                }
            }
        }

        private string UnescapeString(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            return System.Text.RegularExpressions.Regex.Unescape(input);
        }

        private bool ConfigHasColumns()
        {
            return _fromConfig.Columns != null && _fromConfig.Columns.Count > 0;
        }
    }
}
