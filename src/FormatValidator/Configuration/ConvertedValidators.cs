using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FormatValidator.Validators;

namespace FormatValidator.Configuration
{
    public class ConvertedValidators
    {
        private Dictionary<int, ValidatorGroup> _columns;

        public ConvertedValidators()
        {
            _columns = new Dictionary<int, ValidatorGroup>();
        }

        public Dictionary<int, ValidatorGroup> Columns
        {
            get
            {
                return _columns;
            }
        }
    }
}
