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
        
        private void ParameterWithNoValue(string[] parameters)
        {
            const string EXPECTED = "";

            _parameters.Read(parameters);

            Assert.AreEqual(EXPECTED, _parameters.Configuration);
        }
    }
}
