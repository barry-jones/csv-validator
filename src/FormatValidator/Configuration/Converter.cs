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

            if(config.Columns != null && config.Columns.Count > 0)
            {
                foreach(KeyValuePair<int, ColumnValidatorConfiguration> columnConfig in config.Columns)
                {
                    ValidatorGroup group = new ValidatorGroup();

                    if (columnConfig.Value.Unique) group.Add(new UniqueColumnValidator());
                    if (columnConfig.Value.MaxLength > 0) group.Add(new StringLengthValidator(columnConfig.Value.MaxLength));

                    converted.Columns.Add(columnConfig.Key, group);
                }
            }

            return converted;
        }
    }
}
