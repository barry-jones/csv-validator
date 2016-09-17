using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormatValidator
{
    public class Parameters
    {
        private readonly string[] PARAMETERS = { "-config", "-validate" };

        private string _configuration;
        private string _fileToValidate;

        public Parameters()
        {
            _configuration = string.Empty;
            _fileToValidate = string.Empty;
        }

        public void Read(string[] parameters)
        {
            if(parameters != null && parameters.Length > 0)
            {
                for(int i = 0; i < parameters.Length; i++)
                {
                    string current = parameters[i];
                    if(current == "-config")
                    {
                        _configuration = ReadValue(i, parameters);
                    }
                    else if(current == "-validate")
                    {
                        _fileToValidate = ReadValue(i, parameters);
                    }
                }
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

            if (ValueIsAParameter(readValue))
            {
                readValue = string.Empty;
            }

            return readValue;
        }

        private bool ValueIsAParameter(string readValue)
        {
            return PARAMETERS.Contains(readValue);
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
