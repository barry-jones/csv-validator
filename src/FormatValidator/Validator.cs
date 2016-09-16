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

        public Validator()
        {
            _rowValidator = new RowValidator(',');
        }

        public static Validator FromJson(string json)
        {
            ValidatorConfiguration configuration = new JsonReader().Read(json);
            Converter converter = new Converter(configuration);
            ConvertedValidators converted = converter.Convert();

            Validator validator = new Validator();
            validator.TransferConvertedColumns(converted);

            return validator;
        }
        
        public IEnumerable<RowValidationError> Validate(ISourceReader reader)
        {
            foreach(string line in reader.ReadLines())
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

        private void TransferConvertedColumns(ConvertedValidators converted)
        {
            _rowValidator = new RowValidator(',');

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
