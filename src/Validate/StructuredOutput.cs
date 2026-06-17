
namespace FormatValidator
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using Newtonsoft.Json;

    // Produces machine-readable output (json/csv) for the --output option.
    internal enum OutputFormat
    {
        None,
        Json,
        Csv
    }

    internal static class StructuredOutput
    {
        // Parses the --output value. Returns false for null/empty (no structured output requested)
        // and for unsupported values; format is set to None in both cases.
        public static bool TryParse(string value, out OutputFormat format)
        {
            format = OutputFormat.None;

            if (string.IsNullOrWhiteSpace(value))
                return false;

            switch (value.Trim().ToLowerInvariant())
            {
                case "json":
                    format = OutputFormat.Json;
                    return true;
                case "csv":
                    format = OutputFormat.Csv;
                    return true;
                default:
                    return false;
            }
        }

        public static void Write(TextWriter writer, OutputFormat format, List<RowValidationError> errors)
        {
            if (format == OutputFormat.Json)
                writer.Write(BuildJson(errors));
            else if (format == OutputFormat.Csv)
                writer.Write(BuildCsv(errors));
        }

        private static string BuildJson(List<RowValidationError> errors)
        {
            StringBuilder failures = new StringBuilder();
            bool first = true;

            foreach (RowValidationError row in errors)
            {
                foreach (ValidationError error in row.Errors)
                {
                    if (!first)
                        failures.Append(",");
                    first = false;

                    failures.Append("{\"character\":");
                    failures.Append(error.AtCharacter);
                    failures.Append(",\"message\":");
                    failures.Append(JsonConvert.ToString(error.Message ?? string.Empty));
                    failures.Append(",\"column\":");
                    failures.Append(error.Column);
                    failures.Append("}");
                }
            }

            bool success = failures.Length == 0;
            StringBuilder document = new StringBuilder();
            document.Append("{\"success\":");
            document.Append(success ? "true" : "false");
            document.Append(",\"failures\":[");
            document.Append(failures);
            document.Append("]}");
            return document.ToString();
        }

        private static string BuildCsv(List<RowValidationError> errors)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("character,column,message");
            builder.Append("\r\n");

            foreach (RowValidationError row in errors)
            {
                foreach (ValidationError error in row.Errors)
                {
                    builder.Append(error.AtCharacter);
                    builder.Append(",");
                    builder.Append(error.Column);
                    builder.Append(",");
                    builder.Append(EscapeCsvField(error.Message ?? string.Empty));
                    builder.Append("\r\n");
                }
            }

            return builder.ToString();
        }

        // RFC-4180: wrap in double quotes if the field contains a comma, double-quote or newline,
        // and double any internal double-quotes.
        private static string EscapeCsvField(string field)
        {
            bool mustQuote = field.IndexOf(',') >= 0
                || field.IndexOf('"') >= 0
                || field.IndexOf('\n') >= 0
                || field.IndexOf('\r') >= 0;

            if (!mustQuote)
                return field;

            return "\"" + field.Replace("\"", "\"\"") + "\"";
        }
    }
}
