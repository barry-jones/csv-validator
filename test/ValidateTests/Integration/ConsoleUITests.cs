
namespace FormatValidatorTests.Integration
{
    using FormatValidator;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Validate;

    [TestClass]
    public class ConsoleUITests
    {
        const string CONFIG = @"data\configuration\testfile-configuration.json";

        [TestMethod]
        public void Run_WhenFileHasValidationErrors_ReturnsNonZero()
        {
            Options options = new Options
            {
                With = CONFIG,
                File = @"data\headers-testfile-1.csv"
            };

            int exitCode = Program.Run(options);

            Assert.AreEqual(1, exitCode);
        }

        [TestMethod]
        public void Run_WhenFileIsClean_ReturnsZero()
        {
            Options options = new Options
            {
                With = CONFIG,
                File = @"data\singlerow.csv"
            };

            int exitCode = Program.Run(options);

            Assert.AreEqual(0, exitCode);
        }

        [TestMethod]
        public void Main_WhenArgumentsAreMissing_ReturnsNonZero()
        {
            int exitCode = Program.Main(new string[0]);

            Assert.AreNotEqual(0, exitCode);
        }
    }
}
