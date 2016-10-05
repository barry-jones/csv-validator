using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FormatValidator.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FormatValidatorTests.Unit
{
    [TestClass]
    public class JsonReaderTests
    {
        [TestMethod]
        public void JsonReader_WhenEmptyString_ShouldReturnEmptyConfiguration()
        {
            JsonReader reader = new JsonReader();

            ValidatorConfiguration configuration = reader.Read(string.Empty);

            Assert.IsNotNull(configuration, "configuration reader did not create configuration.");
        }

        [TestMethod]
        public void JsonReader_WhenColumnNameIsProvided_ShouldBePopulated()
        {
            const string EXPECTED = "HEADER";
            const string INPUT = "{ \"columns\": { \"1\": { \"name\": \"HEADER\" } } }";

            JsonReader reader = new JsonReader();
            ValidatorConfiguration configuration = reader.Read(INPUT);

            Assert.AreEqual(EXPECTED, configuration.Columns[1].Name);
        }

        [TestMethod]
        public void JsonReader_WhenHasHeaderRowIsProvided_ShouldBePopulated()
        {
            const string INPUT = "{ \"hasHeaderRow\": true }";
            JsonReader reader = new JsonReader();

            ValidatorConfiguration configuration = reader.Read(INPUT);

            Assert.AreEqual(true, configuration.HasHeaderRow);
        }

        [TestMethod]
        public void JsonReader_WhenColumnSeperatorProvided_ShouldBePopulated()
        {
            const string INPUT = "{ \"columnSeperator\": \"|\" }";
            JsonReader reader = new JsonReader();

            ValidatorConfiguration configuration = reader.Read(INPUT);

            Assert.AreEqual("|", configuration.ColumnSeperator);
        }

        [TestMethod]
        public void JsonReader_WhenRowSeperatorProvided_ShouldBePopulated()
        {
            const string INPUT = "{ \"rowSeperator\": \"\\r\\n\" }";
            JsonReader reader = new JsonReader();

            ValidatorConfiguration configuration = reader.Read(INPUT);

            Assert.AreEqual("\r\n", configuration.RowSeperator);
        }
    }
}
