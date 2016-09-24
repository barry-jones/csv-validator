using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormatValidator
{
    public class ConsoleUserInterface : IUserInterface
    {
        public void ReportRowError(RowValidationError error)
        {
            const int PREVIEW_LENGTH = 40;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(string.Format("Error on line {0}: ", error.Row));
            Console.ResetColor();
            Console.WriteLine(error.Content.Trim().Substring(0, error.Content.Trim().Length > PREVIEW_LENGTH ? PREVIEW_LENGTH : error.Content.Trim().Length));

            foreach (ValidationError rowSpecificErrors in error.Errors)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write(string.Format("\tat character {0} ", rowSpecificErrors.AtCharacter));
                Console.ResetColor();
                Console.Write(rowSpecificErrors.Message);
                Console.Write(Environment.NewLine);
            }
        }

        public void ShowSummary(List<RowValidationError> errors)
        {
            Console.WriteLine();

            if (errors.Count > 0)
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
