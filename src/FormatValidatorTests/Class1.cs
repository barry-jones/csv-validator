using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FormatValidator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FormatValidatorTests
{
    [TestClass]
    public class Class1
    {
        [TestMethod]
        public void Nothing()
        {
            string fromFile = string.Empty;

            Validator validator = new Validator();
            validator.LoadRequirements(fromFile);

            bool hasRequirements = validator.HasRequirments;

            Assert.IsTrue(hasRequirements);
        }
    }
}
