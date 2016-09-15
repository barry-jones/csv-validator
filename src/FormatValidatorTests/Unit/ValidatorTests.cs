using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FormatValidator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FormatValidatorTests.Unit
{
    [TestClass]
    public class ValidatorTests
    {
        [TestMethod]
        public void Validator_Create()
        {
            Validator validator = new Validator();
        }
    }
}
