
namespace FormatValidator
{
    using System.Collections.Generic;
    using System.IO;
    using Newtonsoft.Json;

    internal enum OutputFormat
    {
        None,
        Json,
        Csv
    }

    // The machine-readable contract for --output json. The property names are the wire format.
    internal sealed class ValidationOutput
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("failures")]
        public List<ValidationFailure> Failures { get; set; }
    }

    internal sealed class ValidationFailure
    {
        [JsonProperty("character")]
        public int Character { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("column")]
        public int Column { get; set; }
    }

    internal static class StructuredOutput
    {
        // Parses the --output value. Returns false for null/empty (none requested) and for
        // unsupported values; format is set to None in both cases.
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

        public static void Write(TextWriter writer, OutputFormat format, bool success, IEnumerable<RowValidationError> errors)
        {
            if (format == OutputFormat.Json)
                WriteJson(writer, success, errors);
            else if (format == OutputFormat.Csv)
                WriteCsv(writer, errors);
        }

        private static void WriteJson(TextWriter writer, bool success, IEnumerable<RowValidationError> errors)
        {
            ValidationOutput output = new ValidationOutput
            {
                Success = success,
                Failures = Flatten(errors)
            };

            writer.Write(JsonConvert.SerializeObject(output));
            writer.Write('\n');
        }

        private static void WriteCsv(TextWriter writer, IEnumerable<RowValidationError> errors)
        {
            writer.Write("character,column,message\r\n");

            foreach (ValidationFailure failure in Flatten(errors))
            {
                writer.Write(failure.Character);
                writer.Write(',');
                writer.Write(failure.Column);
                writer.Write(',');
                writer.Write(EscapeCsvField(failure.Message ?? string.Empty));
                writer.Write("\r\n");
            }
        }

        private static List<ValidationFailure> Flatten(IEnumerable<RowValidationError> errors)
        {
            List<ValidationFailure> failures = new List<ValidationFailure>();

            foreach (RowValidationError row in errors)
            {
                foreach (ValidationError error in row.Errors)
                {
                    failures.Add(new ValidationFailure
                    {
                        Character = error.AtCharacter,
                        Message = error.Message,
                        Column = error.Column
                    });
                }
            }

            return failures;
        }

        // RFC-4180: wrap in double quotes if the field contains a comma, double-quote or newline,
        // doubling any internal double-quotes.
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
