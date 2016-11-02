
namespace FormatValidator
{
    using System.Collections.Generic;

    public class RowValidationError
    {
        private int _row;
        private string _content;
        private List<ValidationError> _errors;

        public RowValidationError()
        {
            _errors = new List<ValidationError>();
        }

        public List<ValidationError> Errors
        {
            get { return _errors; }
        }

        public int Row
        {
            get { return _row; }
            set { _row = value; }
        }

        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }
    }
}
