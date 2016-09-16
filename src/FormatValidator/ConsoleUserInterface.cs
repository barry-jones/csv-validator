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
            Console.Write(string.Format("[{0}] ", error.Row));
            Console.ResetColor();
            Console.WriteLine(error.Content.Trim().Substring(0, error.Content.Trim().Length > PREVIEW_LENGTH ? PREVIEW_LENGTH : error.Content.Trim().Length));

            foreach (ValidationError rowSpecificErrors in error.Errors)
            {
                Console.WriteLine(
                    string.Format("\t{1}: {2}", error.Row, rowSpecificErrors.At, rowSpecificErrors.Message)
                    );
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
