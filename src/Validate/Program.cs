
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
            bool requested = !string.IsNullOrWhiteSpace(options.Output);
            bool structured = StructuredOutput.TryParse(options.Output, out OutputFormat format);

            // When a (valid) output format is active, route all human output to stderr so
            // stdout carries only the machine-readable document. Restored before we write it.
            System.IO.TextWriter originalOut = Console.Out;
            if (structured)
            {
                Console.SetOut(Console.Error);
            }

            ConsoleUserInterface ui = new ConsoleUserInterface();
            List<RowValidationError> errors = new List<RowValidationError>();

            // An --output value that was supplied but not recognised falls back to human
            // reporting; tell the user the value is not supported before anything else.
            if (requested && !structured)
            {
                Console.WriteLine($"Output format \"{options.Output}\" is not supported; using standard reporting.");
            }

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

            if (structured)
            {
                Console.SetOut(originalOut);
                StructuredOutput.Write(originalOut, format, errors);
            }

            return errors.Count > 0 ? 1 : 0;
        }
    }
}
