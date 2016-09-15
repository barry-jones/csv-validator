using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FormatValidator.Validators;

namespace FormatValidator.Configuration
{
    public class Converter
    {
        public ConvertedValidators Convert(ValidatorConfiguration config)
        {
            ConvertedValidators converted = new ConvertedValidators();

            converted.RowSeperator = UnescapeString(config.RowSeperator);
            converted.ColumnSeperator = UnescapeString(config.ColumnSeperator);

            if(config.Columns != null && config.Columns.Count > 0)
            {
                foreach(KeyValuePair<int, ColumnValidatorConfiguration> columnConfig in config.Columns)
                {
                    List<IValidator> group = new List<IValidator>();

                    if (columnConfig.Value.Unique) group.Add(new UniqueColumnValidator());
                    if (columnConfig.Value.MaxLength > 0) group.Add(new StringLengthValidator(columnConfig.Value.MaxLength));
                    if (!string.IsNullOrWhiteSpace(columnConfig.Value.Pattern)) group.Add(new TextFormatValidator(columnConfig.Value.Pattern));
                    if (columnConfig.Value.IsNumeric) group.Add(new NumberValidator());
                    if (columnConfig.Value.IsRequired) group.Add(new NotNullableValidator());

                    converted.Columns.Add(columnConfig.Key, group);
                }
            }

            return converted;
        }

        private string UnescapeString(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            return System.Text.RegularExpressions.Regex.Unescape(input);
        }
    }
}
