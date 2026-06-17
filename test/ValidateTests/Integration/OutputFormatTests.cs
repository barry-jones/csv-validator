
namespace FormatValidatorTests.Integration
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using FormatValidator;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json.Linq;
    using Validate;

    [TestClass]
    public class OutputFormatTests
    {
        const string CONFIG = @"data\configuration\testfile-configuration.json";
        const string ERROR_FILE = @"data\headers-testfile-1.csv";
        const string CLEAN_FILE = @"data\singlerow.csv";

        // Runs Program.Run with stdout and stderr captured. Returns the exit code and the
        // text written to each stream.
        private static int RunCaptured(Options options, out string stdout, out string stderr)
        {
            TextWriter originalOut = Console.Out;
            TextWriter originalError = Console.Error;
            StringWriter outWriter = new StringWriter();
            StringWriter errorWriter = new StringWriter();

            try
            {
                Console.SetOut(outWriter);
                Console.SetError(errorWriter);
                int exitCode = Program.Run(options);
                stdout = outWriter.ToString();
                stderr = errorWriter.ToString();
                return exitCode;
            }
            finally
            {
                Console.SetOut(originalOut);
                Console.SetError(originalError);
            }
        }

        [TestMethod]
        public void Run_JsonWithErrors_StdoutIsJsonWithFailures()
        {
            Options options = new Options { With = CONFIG, File = ERROR_FILE, Output = "json" };

            int exitCode = RunCaptured(options, out string stdout, out string _);

            Assert.AreEqual(1, exitCode);
            JObject document = JObject.Parse(stdout);
            Assert.AreEqual(false, (bool)document["success"]);
            JArray failures = (JArray)document["failures"];
            Assert.IsTrue(failures.Count > 0);
            foreach (JToken failure in failures)
            {
                Assert.IsNotNull(failure["character"]);
                Assert.IsNotNull(failure["message"]);
                Assert.IsNotNull(failure["column"]);
            }
        }

        [TestMethod]
        public void Run_JsonOnCleanFile_StdoutIsSuccessTrueEmptyFailures()
        {
            Options options = new Options { With = CONFIG, File = CLEAN_FILE, Output = "json" };

            int exitCode = RunCaptured(options, out string stdout, out string _);

            Assert.AreEqual(0, exitCode);
            Assert.AreEqual("{\"success\":true,\"failures\":[]}", stdout.Trim());
            JObject document = JObject.Parse(stdout);
            Assert.AreEqual(true, (bool)document["success"]);
            Assert.AreEqual(0, ((JArray)document["failures"]).Count);
        }

        [TestMethod]
        public void Run_CsvWithErrors_StdoutHasHeaderThenOneLinePerFailure()
        {
            Options options = new Options { With = CONFIG, File = ERROR_FILE, Output = "csv" };

            int exitCode = RunCaptured(options, out string stdout, out string _);

            Assert.AreEqual(1, exitCode);
            string[] lines = stdout.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            Assert.AreEqual("character,column,message", lines[0]);
            Assert.IsTrue(lines.Length > 1);
        }

        [TestMethod]
        public void Run_CsvOnCleanFile_StdoutIsHeaderRowOnly()
        {
            Options options = new Options { With = CONFIG, File = CLEAN_FILE, Output = "csv" };

            int exitCode = RunCaptured(options, out string stdout, out string _);

            Assert.AreEqual(0, exitCode);
            Assert.AreEqual("character,column,message\r\n", stdout);
        }

        [TestMethod]
        public void Csv_FieldWithCommaAndQuote_IsRfc4180Quoted()
        {
            List<RowValidationError> errors = new List<RowValidationError>();
            RowValidationError row = new RowValidationError { Row = 2, Content = "x" };
            ValidationError error = new ValidationError(3, "does not match \"a,b\" pattern") { Column = 1 };
            row.Errors.Add(error);
            errors.Add(row);

            StringWriter writer = new StringWriter();
            StructuredOutput.Write(writer, OutputFormat.Csv, false, errors);
            string csv = writer.ToString();

            string[] lines = csv.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            Assert.AreEqual("character,column,message", lines[0]);
            // comma + internal quotes doubled, whole field wrapped in quotes
            Assert.AreEqual("3,1,\"does not match \"\"a,b\"\" pattern\"", lines[1]);
        }

        [TestMethod]
        public void Run_InvalidOutputValue_FallsBackToHumanReportingWithMessage()
        {
            Options options = new Options { With = CONFIG, File = ERROR_FILE, Output = "xml" };

            int exitCode = RunCaptured(options, out string stdout, out string stderr);

            Assert.AreEqual(1, exitCode);
            // No structured output is produced; the human report and the notice go to stderr.
            Assert.AreEqual(string.Empty, stdout);
            Assert.IsTrue(stderr.Contains("Started validating document."));
            Assert.IsTrue(stderr.Contains("xml"));
            Assert.IsTrue(stderr.Contains("not supported"));
        }

        [TestMethod]
        public void Run_NoOutputOption_HumanOutputOnStderr_ExitCodeUnchanged()
        {
            Options options = new Options { With = CONFIG, File = ERROR_FILE };

            int exitCode = RunCaptured(options, out string stdout, out string stderr);

            Assert.AreEqual(1, exitCode);
            Assert.AreEqual(string.Empty, stdout);
            Assert.IsTrue(stderr.Contains("Started validating document."));
            Assert.IsTrue(stderr.Contains("FAILED"));
        }

        [TestMethod]
        public void Run_OutputActive_HumanReportGoesToStderrNotStdout()
        {
            Options options = new Options { With = CONFIG, File = ERROR_FILE, Output = "json" };

            RunCaptured(options, out string stdout, out string stderr);

            // stdout is pure machine output; the human report is on stderr.
            Assert.IsTrue(stdout.TrimStart().StartsWith("{"));
            Assert.IsFalse(stdout.Contains("Started validating document."));
            Assert.IsTrue(stderr.Contains("Started validating document."));
            Assert.IsTrue(stderr.Contains("FAILED"));
        }

        [TestMethod]
        public void Help_ListsOutputOptionAsNotRequired()
        {
            string helpText = CommandLine.Text.HelpText.AutoBuild(
                CommandLine.Parser.Default.ParseArguments<Options>(new[] { "--help" }),
                h => h,
                e => e).ToString();

            Assert.IsTrue(helpText.Contains("--output"));
            Assert.IsTrue(helpText.Contains("stdout"));
            // Required options are marked "Required." in the help text; --output must not be.
            foreach (string line in helpText.Split('\n'))
            {
                if (line.Contains("--output"))
                    Assert.IsFalse(line.Contains("Required."), "--output should not be marked Required.");
            }
        }
    }
}
