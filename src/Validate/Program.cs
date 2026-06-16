
namespace FormatValidator
{
    using System;
    using System.Collections.Generic;
    using CommandLine;
    using Validate;

    /// <summary>
    /// Main application entry point
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main application entry point
        /// </summary>
        /// <param name="args">Application aruments</param>
        /// <returns>The process exit code: 0 on success, non-zero on validation errors or invalid arguments.</returns>
        public static int Main(string[] args)
        {
            return Parser.Default.ParseArguments<Options>(args)
                .MapResult(options => Run(options), errors => 1);
        }

        internal static int Run(Options options)
        {
            ConsoleUserInterface ui = new ConsoleUserInterface();
            List<RowValidationError> errors = new List<RowValidationError>();

            ui.ShowStart();

            DateTime start = DateTime.Now;
            Validator validator = Validator.FromJson(System.IO.File.ReadAllText(options.With));
            FileSourceReader source = new FileSourceReader(options.File);
            source.OnProgress = percentage => ui.ReportProgress(percentage);

            foreach (RowValidationError current in validator.Validate(source))
            {
                errors.Add(current);
                ui.ReportRowError(current);
            }

            DateTime end = DateTime.Now;

            ui.ShowSummary(validator, errors, end.Subtract(start));

            return errors.Count > 0 ? 1 : 0;
        }
    }
}
