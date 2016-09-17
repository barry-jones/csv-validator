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

            if(parameters.IsValid())
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
            else
            {
                Console.WriteLine(string.Format("The parameters '{0}' and '{1}' provided either are empty or do not point to files.", parameters.Configuration, parameters.FileToValidate));
            }
        }
    }
}
