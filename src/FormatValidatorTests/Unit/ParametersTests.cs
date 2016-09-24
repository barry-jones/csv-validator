using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FormatValidator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FormatValidatorTests.Unit
{
    [TestClass]
    public class ParametersTests
    {
        const string VALID_CONFIGFILE = @"data\configuration\configuration.json";
        const string VALID_INPUTFILE = @"data\simplefile.csv";

        private Parameters _parameters;

        [TestInitialize]
        public void Setup()
        {
            _parameters = new Parameters();
        }

        [TestMethod]
        public void Parameters_WhenValidInput_ReadsConfigFile()
        {
            const string EXPECTED = "testing";
            string[] input = new string[] { "file.csv", "-with", "testing" };

            _parameters.Read(input);

            Assert.AreEqual(EXPECTED, _parameters.Configuration);
        }

        [TestMethod]
        public void Parameters_WhenValidInput_ReadsValidationFile()
        {
            const string EXPECTED = "testing.csv";
            string[] input = new string[] { "testing.csv", "-with", "config.json" };

            _parameters.Read(input);

            Assert.AreEqual(EXPECTED, _parameters.FileToValidate);
        }

        [TestMethod]
        public void Parameters_WhenCorrectlyConfigured_IsValid()
        {
            const bool EXPECTED_RESULT = true;
            string[] input = new string[] { VALID_INPUTFILE, "-with", VALID_CONFIGFILE };

            _parameters.Read(input);

            bool isValid = _parameters.IsValid();

            Assert.AreEqual(EXPECTED_RESULT, isValid);
        }

        [TestMethod]
        public void Parameters_WhenNoParameters_NoValuesAreSet()
        {
            _parameters.Read(new string[] { });

            Assert.AreEqual(string.Empty, _parameters.Configuration);
            Assert.AreEqual(string.Empty, _parameters.FileToValidate);
        }

        [TestMethod]
        public void Parameters_WhenParameterProvidedWithNoValue_NoValueIsSet()
        {
            ParameterWithNoValue(new string[] { "-with" });
            ParameterWithNoValue(new string[] { "file.csv" });
        }

        [TestMethod]
        public void Parameters_WhenNoValuesProvide_IsNotValid()
        {
            const bool EXPECTED = false;

            bool result = _parameters.IsValid();

            Assert.AreEqual(EXPECTED, result);
        }

        [TestMethod]
        public void Parameters_WhenOnlyValidationFileProvided_IsNotValid()
        {
            const bool EXPECTED = false;

            _parameters.Read(new string[] { VALID_INPUTFILE });

            bool result = _parameters.IsValid();

            Assert.AreEqual(EXPECTED, result);
        }

        [TestMethod]
        public void Parameters_WhenOnlyConfigurationFile_IsNotValid()
        {
            const bool EXPECTED = false;

            _parameters.Read(new string[] { "-with", VALID_CONFIGFILE });

            bool result = _parameters.IsValid();

            Assert.AreEqual(EXPECTED, result);
        }

        [TestMethod]
        public void Parameters_WhenConfigFileDoesntExist_IsNotValid()
        {
            const bool EXPECTED = false;

            _parameters.Read(new string[] { "-with", @"nonexistent-configuration.json" });

            bool result = _parameters.IsValid();

            Assert.AreEqual(EXPECTED, result);
        }

        private void ParameterWithNoValue(string[] parameters)
        {
            const string EXPECTED = "";

            _parameters.Read(parameters);

            Assert.AreEqual(EXPECTED, _parameters.Configuration);
        }
    }
}
