using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FormatValidator.Configuration;
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
            ValidatorConfiguration configuration = Newtonsoft.Json.JsonConvert.DeserializeObject<ValidatorConfiguration>(json);
            ConvertedValidators converted = new Converter().Convert(configuration);

            Validator validator = new Validator();
            validator._rowValidator = new RowValidator(',');

            foreach(KeyValuePair<int, ValidatorGroup> column in converted.Columns)
            {
                validator._rowValidator.AddColumnValidator(column.Key, column.Value);
            }

            return validator;
        }

        public List<ValidatorGroup> GetColumnValidators()
        {
            return _rowValidator.GetColumnValidators();
        }
    }
}
