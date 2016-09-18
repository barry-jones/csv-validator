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
        public void Parameters_ReadsConfigurationFile_ContentIsValid()
        {
            const string EXPECTED = "testing";
            string[] input = new string[] { "-config", "testing" };

            _parameters.Read(input);

            Assert.AreEqual(EXPECTED, _parameters.Configuration);
        }

        [TestMethod]
        public void Parameters_ReadsInputFile_ContentIsValid()
        {
            const string EXPECTED = "testing";
            string[] input = new string[] { "-validate", "testing" };

            _parameters.Read(input);

            Assert.AreEqual(EXPECTED, _parameters.FileToValidate);
        }

        [TestMethod]
        public void Parameters_WhenNoParameters_NoValuesAreSet()
        {
            _parameters.Read(new string[] { });

            Assert.AreEqual(string.Empty, _parameters.Configuration);
            Assert.AreEqual(string.Empty, _parameters.FileToValidate);
        }

        [TestMethod]
        public void Parametesr_WhenParameterProvidedWithNoValue_NoValueIsSet()
        {
            ParameterWithNoValue(new string[] { "-config" });
            ParameterWithNoValue(new string[] { "-validate" });
        }

        [TestMethod]
        public void Parameters_WhenMultipleArgumentsButNoValues_NoValueSet()
        {
            string[] ARGUMENTS = { "-config", "-validate" };

            _parameters.Read(ARGUMENTS);

            Assert.AreEqual(string.Empty, _parameters.Configuration);
            Assert.AreEqual(string.Empty, _parameters.FileToValidate);
        }

        [TestMethod]
        public void Parameters_IsValidWithEmptyValues_ReturnsFalse()
        {
            const bool EXPECTED = false;

            bool result = _parameters.IsValid();

            Assert.AreEqual(EXPECTED, result);
        }

        [TestMethod]
        public void Parameters_IsValidWithOnlyValidateValue_ReturnsFalse()
        {
            const bool EXPECTED = false;

            _parameters.Read(new string[] { "-validate", VALID_INPUTFILE });

            bool result = _parameters.IsValid();

            Assert.AreEqual(EXPECTED, result);
        }

        [TestMethod]
        public void Parameters_IsValidWithOnlyConfigValue_ReturnsFalse()
        {
            const bool EXPECTED = false;

            _parameters.Read(new string[] { "-config", VALID_CONFIGFILE });

            bool result = _parameters.IsValid();

            Assert.AreEqual(EXPECTED, result);
        }

        [TestMethod]
        public void Parameters_IsValidWithInvalidFile_ReturnsFalse()
        {
            const bool EXPECTED = false;

            _parameters.Read(new string[] { "-config", @"nonexistent-configuration.json" });

            bool result = _parameters.IsValid();

            Assert.AreEqual(EXPECTED, result);
        }

        [TestMethod]
        public void Parameters_IsValidWhenBothValuesProvidedAndCorrect_IsValid()
        {
            const bool EXPECTED = true;

            _parameters.Read(new string[] {
                "-config", VALID_CONFIGFILE,
                "-validate", VALID_INPUTFILE
            });

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
