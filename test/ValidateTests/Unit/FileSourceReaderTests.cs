
namespace FormatValidatorTests.Unit
{
    using System.Collections.Generic;
    using FormatValidator;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class FileSourceReaderTests
    {
        [TestMethod]
        public void FileSourceReader_Create()
        {
            const string FILE = @"data\simplefile.csv";
            const string ROW_SEPERATOR = "\r\n";

            FileSourceReader reader = new FileSourceReader(FILE);
            foreach(string line in reader.ReadLines(ROW_SEPERATOR))
            {
            }
        }

        // GWT-1: Given a file with multiple rows,
        //        when OnProgress is set and ReadLines completes,
        //        then the final callback value is 100.
        [TestMethod]
        public void FileSourceReader_WhenOnProgressSet_FinalCallbackValueIs100()
        {
            const string FILE = @"data\simplefile.csv";
            List<int> progressValues = new List<int>();

            FileSourceReader reader = new FileSourceReader(FILE);
            reader.OnProgress = p => progressValues.Add(p);

            foreach (string line in reader.ReadLines("\r\n")) { }

            Assert.IsTrue(progressValues.Count > 0);
            Assert.AreEqual(100, progressValues[progressValues.Count - 1]);
        }

        // GWT-2: Given a file with multiple rows,
        //        when OnProgress is set and ReadLines is enumerated,
        //        then every callback value is between 0 and 100 inclusive.
        [TestMethod]
        public void FileSourceReader_WhenOnProgressSet_AllCallbackValuesAreBetween0And100()
        {
            const string FILE = @"data\simplefile.csv";
            List<int> progressValues = new List<int>();

            FileSourceReader reader = new FileSourceReader(FILE);
            reader.OnProgress = p => progressValues.Add(p);

            foreach (string line in reader.ReadLines("\r\n")) { }

            foreach (int value in progressValues)
                Assert.IsTrue(value >= 0 && value <= 100, $"Progress value {value} is out of the 0-100 range");
        }

        // GWT-3: Given a file with multiple rows,
        //        when OnProgress is set and ReadLines is enumerated,
        //        then callback values are non-decreasing.
        [TestMethod]
        public void FileSourceReader_WhenOnProgressSet_CallbackValuesAreNonDecreasing()
        {
            const string FILE = @"data\simplefile.csv";
            List<int> progressValues = new List<int>();

            FileSourceReader reader = new FileSourceReader(FILE);
            reader.OnProgress = p => progressValues.Add(p);

            foreach (string line in reader.ReadLines("\r\n")) { }

            for (int i = 1; i < progressValues.Count; i++)
                Assert.IsTrue(progressValues[i] >= progressValues[i - 1],
                    $"Progress decreased from {progressValues[i - 1]} to {progressValues[i]} at index {i}");
        }

        // GWT-4: Given a file with multiple rows,
        //        when OnProgress is set and only two rows have been yielded,
        //        then at least one callback has already fired (progress fires during reading, not just at the end).
        [TestMethod]
        public void FileSourceReader_WhenOnProgressSet_CallbackFiresDuringReading()
        {
            const string FILE = @"data\simplefile.csv";
            List<int> progressValues = new List<int>();

            FileSourceReader reader = new FileSourceReader(FILE);
            reader.OnProgress = p => progressValues.Add(p);

            using (IEnumerator<string> enumerator = reader.ReadLines("\r\n").GetEnumerator())
            {
                enumerator.MoveNext(); // row 1 yielded; iterator pauses at yield return
                enumerator.MoveNext(); // iterator resumes, fires progress, then yields row 2
            }

            Assert.IsTrue(progressValues.Count > 0, "Expected at least one progress callback before all rows were consumed");
        }

        // GWT-5: Given a file with multiple rows and OnProgress set,
        //        when ReadLines is enumerated,
        //        then the rows returned are identical to those returned without OnProgress.
        [TestMethod]
        public void FileSourceReader_WhenOnProgressSet_RowsReturnedAreIdenticalToWithout()
        {
            const string FILE = @"data\simplefile.csv";
            const string ROW_SEPARATOR = "\r\n";

            FileSourceReader withoutProgress = new FileSourceReader(FILE);
            List<string> rowsWithout = new List<string>(withoutProgress.ReadLines(ROW_SEPARATOR));

            FileSourceReader withProgress = new FileSourceReader(FILE);
            withProgress.OnProgress = _ => { };
            List<string> rowsWith = new List<string>(withProgress.ReadLines(ROW_SEPARATOR));

            CollectionAssert.AreEqual(rowsWithout, rowsWith);
        }

        // GWT-6: Given OnProgress is not set,
        //        when ReadLines is enumerated,
        //        then all rows are returned without exception.
        [TestMethod]
        public void FileSourceReader_WhenOnProgressNotSet_AllRowsReturnedWithoutException()
        {
            const string FILE = @"data\simplefile.csv";

            FileSourceReader reader = new FileSourceReader(FILE);
            List<string> rows = new List<string>(reader.ReadLines("\r\n"));

            Assert.IsTrue(rows.Count > 0);
        }

        // GWT-13: Given a file whose last row has no row separator,
        //         when OnProgress is set and ReadLines completes,
        //         then the final callback value is still 100.
        [TestMethod]
        public void FileSourceReader_WhenLastRowHasNoSeparator_ProgressCompletesAt100()
        {
            const string FILE = @"data\simplefile.csv"; // last row has no trailing \r\n
            List<int> progressValues = new List<int>();

            FileSourceReader reader = new FileSourceReader(FILE);
            reader.OnProgress = p => progressValues.Add(p);

            foreach (string line in reader.ReadLines("\r\n")) { }

            Assert.IsTrue(progressValues.Count > 0);
            Assert.AreEqual(100, progressValues[progressValues.Count - 1]);
        }

        // GWT-14: Given a file with a single row,
        //         when OnProgress is set and ReadLines completes,
        //         then the callback fires at least once.
        [TestMethod]
        public void FileSourceReader_WhenFileHasSingleRow_ProgressCallbackFiresAtLeastOnce()
        {
            const string FILE = @"data\singlerow.csv";
            List<int> progressValues = new List<int>();

            FileSourceReader reader = new FileSourceReader(FILE);
            reader.OnProgress = p => progressValues.Add(p);

            foreach (string line in reader.ReadLines("\r\n")) { }

            Assert.IsTrue(progressValues.Count > 0);
        }
    }
}
