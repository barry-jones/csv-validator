using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FormatValidator.Validators;

namespace FormatValidator.Configuration
{
    public class ConvertedValidators
    {
        private Dictionary<int, List<IValidator>> _columns;

        public ConvertedValidators()
        {
            _columns = new Dictionary<int, List<IValidator>>();
        }

        public Dictionary<int, List<IValidator>> Columns
        {
            get
            {
                return _columns;
            }
        }
    }
}
