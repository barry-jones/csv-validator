
namespace FormatValidator
{
    using System;
    using System.Collections.Generic;

    internal class ConsoleUserInterface : IUserInterface
    {
        private const int ProgressBarWidth = 20;
        private const int ProgressLineTotalWidth = 27; // [ + 20 bars + ] + space + 3 digits + %
        private const string ProgressClearLine = "\r                              \r";

        private bool _progressVisible;
        private int _lastPercentage;

        public void ShowStart()
        {
            Console.Error.WriteLine("Started validating document.");
            Console.Error.WriteLine();
        }

        public void ReportProgress(int percentage)
        {
            _lastPercentage = percentage;
            WriteProgressBar(percentage);
            _progressVisible = true;
        }

        public void ReportRowError(RowValidationError error)
        {
            bool overwriting = _progressVisible;
            _progressVisible = false;
            bool firstLine = true;

            foreach (ValidationError rowSpecificErrors in error.Errors)
            {
                if (firstLine && overwriting)
                    Console.Error.Write("\r");

                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.Write(string.Format("[Error] ", error.Row));
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Error.Write(string.Format("line {1} character {0} ", rowSpecificErrors.AtCharacter, error.Row));
                Console.ResetColor();
                Console.Error.Write(rowSpecificErrors.Message);

                if (firstLine && overwriting)
                    Console.Error.Write(new string(' ', ProgressLineTotalWidth));

                Console.Error.Write(Environment.NewLine);
                firstLine = false;
            }

            WriteProgressBar(_lastPercentage);
            _progressVisible = true;
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
                Console.Error.Write(ProgressClearLine);
                _progressVisible = false;
            }

            Console.Error.WriteLine();

            Console.ForegroundColor = colour;
            Console.Error.WriteLine(errors.Count > 0 ? "FAILED" : "PASSED");
            Console.ResetColor();
            Console.Error.WriteLine();

            Console.Error.WriteLine(
                string.Format(message,
                    validator.TotalRowsChecked,
                    errors.Count,
                    duration.TotalSeconds)
                );
        }

        private static void WriteProgressBar(int percentage)
        {
            int filled = percentage * ProgressBarWidth / 100;
            string bar = new string('█', filled) + new string('░', ProgressBarWidth - filled);
            Console.Error.Write($"\r[{bar}] {percentage,3}%");
        }
    }
}
