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
            string configurationData = System.IO.File.ReadAllText(@"d:\development\personal\formatvalidator\src\formatvalidatortests\data\configuration\simplefile-configuration.json");
            string filename = @"d:\development\personal\formatvalidator\src\formatvalidatortests\data\simplefile.csv";

            ConsoleUserInterface ui = new ConsoleUserInterface();
            FileSourceReader source = new FileSourceReader(filename, "\r\n");
            Validator validator = Validator.FromJson(configurationData);

            List<RowValidationError> errors = new List<RowValidationError>();
                
            foreach(RowValidationError current in validator.Validate(source))
            {
                errors.Add(current);
                ui.ReportRowError(current);
            }

            ui.ShowSummary(errors);

            Console.ReadLine();
        }
    }
}
