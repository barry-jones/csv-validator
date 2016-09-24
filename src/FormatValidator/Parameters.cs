using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormatValidator
{
    public class Parameters
    {
        private const int EXPECTED_NUMBER_PARAMETERS = 3;
        private readonly string[] PARAMETERS = { "-with" };

        private string _configuration;
        private string _fileToValidate;

        public Parameters()
        {
            _configuration = string.Empty;
            _fileToValidate = string.Empty;
        }

        public void Read(string[] parameters)
        {
            // first value is expected to be the file to validate and the second
            // value is expected to the requirements, specified by the -with
            // argument
            if (hasParameters(parameters))
            {
                readValidationFile(parameters);
                readConfigurationFile(parameters);
            }
        }

        public bool IsValid()
        {
            bool isValid = false;

            bool configIsValid = !string.IsNullOrEmpty(_configuration) && System.IO.File.Exists(_configuration);
            bool fileToValidateIsValid = !string.IsNullOrEmpty(_fileToValidate) && System.IO.File.Exists(_fileToValidate);

            isValid = configIsValid && fileToValidateIsValid;

            return isValid;
        }

        private string ReadValue(int currentIndex, string[] parameters)
        {
            string readValue = string.Empty;
            int indexOfValue = (currentIndex + 1);

            if (indexOfValue < parameters.Length)
            {
                readValue = parameters[indexOfValue];
            }

            if (valueIsAParameter(readValue))
            {
                readValue = string.Empty;
            }

            return readValue;
        }

        private bool valueIsAParameter(string readValue)
        {
            return PARAMETERS.Contains(readValue);
        }

        private bool hasParameters(string[] parameters)
        {
            return parameters != null && parameters.Length == EXPECTED_NUMBER_PARAMETERS;
        }

        private void readValidationFile(string[] parameters)
        {
            _fileToValidate = parameters[0];
        }
        private void readConfigurationFile(string[] parameters)
        {
            if (string.Compare("-with", parameters[1], StringComparison.OrdinalIgnoreCase) == 0)
            {
                _configuration = parameters[2];
            }
        }

        public string Configuration
        {
            get
            {
                return _configuration;
            }
        }

        public string FileToValidate
        {
            get
            {
                return _fileToValidate;
            }
        }
    }
}
