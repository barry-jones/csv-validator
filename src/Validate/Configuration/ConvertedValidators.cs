using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FormatValidator.Validators;

namespace FormatValidator.Configuration
{
    public class ConvertedValidators
    {
        public ConvertedValidators()
        {
            Columns = new Dictionary<int, List<IValidator>>();
        }

        public Dictionary<int, List<IValidator>> Columns { get; private set; }

        public string RowSeperator { get; set; }

        public string ColumnSeperator { get; set; }

        public bool HasHeaderRow { get; set; }
    }
}
