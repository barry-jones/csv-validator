using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace ValidateTests.Unit
{
    using FormatValidator;

    [TestClass]
    public class ColumnSplitterTests
    {
        [TestMethod]
        public void Split_WhenOnlyOneColmn_ReturnsSingleResult()
        {
            const string INPUT = "just a string";
            const string SEPERATOR = ",";

            string[] result = ColumnSplitter.Split(INPUT, SEPERATOR);

            Assert.AreEqual(result.Length, 1);
            Assert.AreEqual(result[0], INPUT);
        }

        [TestMethod]
        public void Split_WhenTwoColmns_ReturnsTwoResults()
        {
            const string INPUT = "just a string,seperated";
            const string SEPERATOR = ",";

            string[] result = ColumnSplitter.Split(INPUT, SEPERATOR);

            Assert.AreEqual(result.Length, 2);
            Assert.AreEqual(result[0], "just a string");
            Assert.AreEqual(result[1], "seperated");
        }

        [TestMethod]
        public void Split_WhenUsingMultiCharSeperators_IsCorrect()
        {
            const string INPUT = "just a string##seperated";
            const string SEPERATOR = "##";

            string[] result = ColumnSplitter.Split(INPUT, SEPERATOR);

            Assert.AreEqual(result.Length, 2);
            Assert.AreEqual(result[0], "just a string");
            Assert.AreEqual(result[1], "seperated");
        }

        [TestMethod]
        public void Split_WhenUsingQuotedStringAtStart_IgnoresSeperator()
        {
            const string INPUT = "\"just, a string\",seperated";
            const string SEPERATOR = ",";

            string[] result = ColumnSplitter.Split(INPUT, SEPERATOR);

            Assert.AreEqual(result.Length, 2);
            Assert.AreEqual("\"just, a string\"", result[0]);
            Assert.AreEqual("seperated", result[1]);
        }

        [TestMethod]
        public void Split_WhenUsingQuotedStringInMiddle_IgnoresSeperator()
        {
            const string INPUT = "seperated,\"just, a string\",seperated";
            const string SEPERATOR = ",";

            string[] result = ColumnSplitter.Split(INPUT, SEPERATOR);

            Assert.AreEqual(result.Length, 3);
            Assert.AreEqual("\"just, a string\"", result[1]);
            Assert.AreEqual("seperated", result[0]);
        }

        [TestMethod]
        public void Split_WhenUsingQuotedStringAtEnd_IgnoresSeperator()
        {
            const string INPUT = "seperated,string,\"just, a string\"";
            const string SEPERATOR = ",";

            string[] result = ColumnSplitter.Split(INPUT, SEPERATOR);

            Assert.AreEqual(result.Length, 3);
            Assert.AreEqual("\"just, a string\"", result[2]);
            Assert.AreEqual("seperated", result[0]);
        }
        
        [TestMethod]
        public void Split_WhenUsingQuotedStringMultipleTimes_IgnoresSeperator()
        {
            const string INPUT = "\"just, a string\",seperated,string,\"just, a string\",\"just, a string\"";
            const string SEPERATOR = ",";

            string[] result = ColumnSplitter.Split(INPUT, SEPERATOR);

            Assert.AreEqual(result.Length, 5);
            Assert.AreEqual("\"just, a string\"", result[0]);
            Assert.AreEqual("seperated", result[1]);
        }
    }
}
