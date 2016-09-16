using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FormatValidator.Input;

namespace FormatValidator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Parameters parameters = new Parameters();
            ConsoleUserInterface ui = new ConsoleUserInterface();
            List<RowValidationError> errors = new List<RowValidationError>();

            parameters.Read(args);

            if(IsParametersValid(parameters))
            {
                Validator validator = Validator.FromJson(System.IO.File.ReadAllText(parameters.Configuration));
                FileSourceReader source = new FileSourceReader(parameters.FileToValidate, "\r\n");

                foreach (RowValidationError current in validator.Validate(source))
                {
                    errors.Add(current);
                    ui.ReportRowError(current);
                }

                ui.ShowSummary(errors);

                Console.ReadLine();
            }
        }

        private static bool IsParametersValid(Parameters parameters)
        {
            bool isValid = true;

            if (!string.IsNullOrEmpty(parameters.Configuration))
            {
                if (!System.IO.File.Exists(parameters.Configuration))
                {
                    Console.WriteLine(string.Format("The -config file '{0}' was not provided or the file does not exist.", parameters.Configuration));
                    isValid = false;
                }
            }
            if (!string.IsNullOrEmpty(parameters.FileToValidate))
            {
                if (!System.IO.File.Exists(parameters.FileToValidate))
                {
                    Console.WriteLine(string.Format("The -validate file '{0}' was not provided or the file does not exist.", parameters.FileToValidate));
                    isValid = false;
                }
            }

            return isValid;
        }
    }
}
