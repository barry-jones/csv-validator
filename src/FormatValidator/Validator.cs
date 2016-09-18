using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FormatValidator.Configuration;
using FormatValidator.Input;
using FormatValidator.Validators;

namespace FormatValidator
{
    public class Validator
    {
        private RowValidator _rowValidator;
        private string _rowSeperator;

        public Validator()
        {
            _rowValidator = new RowValidator();
            _rowSeperator = "\r\n";
        }

        public static Validator FromJson(string json)
        {
            ValidatorConfiguration configuration = new JsonReader().Read(json);
            ConfigurationConvertor converter = new ConfigurationConvertor(configuration);
            ConvertedValidators converted = converter.Convert();

            Validator validator = new Validator();
            validator.SetColumnSeperator(converted.ColumnSeperator);
            validator.SetRowSeperator(converted.RowSeperator);
            validator.TransferConvertedColumns(converted);

            return validator;
        }

        public IEnumerable<RowValidationError> Validate(ISourceReader reader)
        {
            foreach(string line in reader.ReadLines(_rowSeperator))
            {
                if (!_rowValidator.IsValid(line))
                {
                    RowValidationError error = _rowValidator.GetError();
                    _rowValidator.ClearErrors();

                    yield return error;
                }
            }
        }

        public List<ValidatorGroup> GetColumnValidators()
        {
            return _rowValidator.GetColumnValidators();
        }

        public void SetColumnSeperator(string seperator)
        {
            if (string.IsNullOrEmpty(seperator))
            {
                _rowValidator.ColumnSeperator = ",";
            }
            else
            {
                _rowValidator.ColumnSeperator = seperator;
            }
        }

        public void SetRowSeperator(string rowSeperator)
        {
            if(!string.IsNullOrEmpty(rowSeperator))
            {
                _rowSeperator = rowSeperator;
            }                
        }

        private void TransferConvertedColumns(ConvertedValidators converted)
        {
            foreach (KeyValuePair<int, List<IValidator>> column in converted.Columns)
            {
                foreach (IValidator columnValidator in column.Value)
                {
                    _rowValidator.AddColumnValidator(column.Key, columnValidator);
                }
            }
        }
    }
}
