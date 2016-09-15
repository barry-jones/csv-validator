using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormatValidator
{
    public class ValidationError
    {
        private int _at;
        private string _message;
        private int _row;
        private string _rowContent;

        public ValidationError(int at, string message)
        {
            _at = at;
            _message = message;
        }

        public int At
        {
            get
            {
                return _at;
            }
            set
            {
                _at = value;
            }
        }

        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
            }
        }

        public int Row
        {
            get
            {
                return _row;
            }
            set
            {
                _row = value;
            }
        }

        public string RowContent
        {
            get
            {
                return _rowContent;
            }
            set
            {
                _rowContent = value;
            }
        }
    }
}
