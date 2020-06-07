
namespace FormatValidator
{
    using System;
    using System.Collections.Generic;

    internal class ConsoleUserInterface : IUserInterface
    {
        public void ShowStart()
        {
            Console.WriteLine("Started validating document.");
            Console.WriteLine();
        }

        public void ReportRowError(RowValidationError error)
        {
            foreach (ValidationError rowSpecificErrors in error.Errors)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(string.Format("[Error] ", error.Row));
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write(string.Format("line {1} character {0} ", rowSpecificErrors.AtCharacter, error.Row));
                Console.ResetColor();
                Console.Write(rowSpecificErrors.Message);
                Console.Write(Environment.NewLine);
            }
        }

        public void ShowSummary(Validator validator, List<RowValidationError> errors, TimeSpan duration)
        {
            ConsoleColor colour;
            string message = string.Empty;

            if(errors.Count > 0)
            {
                colour = ConsoleColor.Red;
                message = "{0} rows checked and {1} errors found in {2}s.";
            }
            else
            {
                colour = ConsoleColor.Red;
                message = "{0} rows checked and no errors found in {1}s.";
            }

            Console.WriteLine();

            Console.ForegroundColor = colour;
            Console.WriteLine(errors.Count > 0 ? "FAILED" : "PASSED");
            Console.ResetColor();
            Console.WriteLine();

            Console.WriteLine(
                string.Format(message, 
                    validator.TotalRowsChecked, 
                    errors.Count, 
                    duration.TotalSeconds)
                );
        }
    }
}
