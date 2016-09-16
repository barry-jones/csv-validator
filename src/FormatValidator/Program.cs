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
            const int PREVIEW_LENGTH = 40;
            string configurationData = System.IO.File.ReadAllText(@"d:\development\personal\formatvalidator\src\formatvalidatortests\data\configuration\simplefile-configuration.json");
            string filename = @"d:\development\personal\formatvalidator\src\formatvalidatortests\data\simplefile.csv";

            FileSourceReader source = new FileSourceReader(filename, "\r\n");
            Validator validator = Validator.FromJson(configurationData);

            List<RowValidationError> errors = validator.Validate(source);

            foreach(RowValidationError current in errors)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(string.Format("[{0}] ", current.Row));
                Console.ResetColor();
                Console.WriteLine(current.Content.Trim().Substring(0, current.Content.Trim().Length > PREVIEW_LENGTH ? PREVIEW_LENGTH : current.Content.Trim().Length));

                foreach (ValidationError rowSpecificErrors in current.Errors)
                {
                    Console.WriteLine(
                        string.Format("\t{1}: {2}", current.Row, rowSpecificErrors.At, rowSpecificErrors.Message)
                        );
                }
            }

            WriteSummary(errors);

            Console.ReadLine();
        }

        private static void WriteSummary(List<RowValidationError> errors)
        {
            Console.WriteLine();

            if(errors.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("FAILED");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("PASSED");
            }

            Console.ResetColor();
        }
    }
}
