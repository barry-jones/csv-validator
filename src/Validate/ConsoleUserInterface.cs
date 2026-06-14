
namespace FormatValidator
{
    using System;
    using System.Collections.Generic;

    internal class ConsoleUserInterface : IUserInterface
    {
        private bool _progressVisible;

        public void ShowStart()
        {
            Console.WriteLine("Started validating document.");
            Console.WriteLine();
        }

        public void ReportProgress(int percentage)
        {
            const int barWidth = 20;
            int filled = percentage * barWidth / 100;
            string bar = new string('█', filled) + new string('░', barWidth - filled);
            Console.Write($"\r[{bar}] {percentage,3}%");
            _progressVisible = true;
        }

        public void ReportRowError(RowValidationError error)
        {
            if (_progressVisible)
            {
                Console.WriteLine();
                _progressVisible = false;
            }

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
                colour = ConsoleColor.Green;
                message = "{0} rows checked and no errors found in {1}s.";
            }

            if (_progressVisible)
            {
                Console.WriteLine();
                _progressVisible = false;
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
