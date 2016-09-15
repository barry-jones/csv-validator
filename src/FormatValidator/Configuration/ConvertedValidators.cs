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
        private string _columnSeperator;
        private string _rowSeperator;

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

        public string RowSeperator
        {
            get
            {
                return _rowSeperator;
            }
            set
            {
                _rowSeperator = value;
            }
        }

        public string ColumnSeperator
        {
            get
            {
                return _columnSeperator;
            }
            set
            {
                _columnSeperator = value;
            }
        }
    }
}
