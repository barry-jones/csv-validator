
namespace FormatValidator
{
    /// <summary>
    /// Provides defails of an error found during validation.
    /// </summary>
    public class ValidationError
    {
        private int _atCharacter;
        private string _message;

        internal ValidationError(int at, string message)
        {
            _atCharacter = at;
            _message = message;
        }

        public string Message
        {
            get { return _message; }
        }

        public int AtCharacter
        {
            get { return _atCharacter; }
            internal set { _atCharacter = value; }
        }
    }
}
