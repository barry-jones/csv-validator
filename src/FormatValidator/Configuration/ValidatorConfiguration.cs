using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormatValidator.Configuration
{
    public class ValidatorConfiguration
    {
        private string _columnSeperator;
        private string _rowSeperator;
        private Dictionary<int, ColumnValidatorConfiguration> _columns;

        public ValidatorConfiguration()
        {
            _columns = new Dictionary<int, ColumnValidatorConfiguration>();
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

        public Dictionary<int, ColumnValidatorConfiguration> Columns
        {
            get
            {
                return _columns;
            }
            set
            {
                _columns = value;
            }
        }
    }
}
