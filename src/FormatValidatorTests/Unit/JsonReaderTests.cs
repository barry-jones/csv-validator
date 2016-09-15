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
    }
}
