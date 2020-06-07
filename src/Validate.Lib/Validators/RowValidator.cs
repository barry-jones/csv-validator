
namespace FormatValidator.Validators
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A collection of column validators that are orchestrated over a single
    /// row of the file being validated.
    /// </summary>
    /// <seealso cref="Validator"/>
    internal class RowValidator
    {
        private ValidatorGroup[] _columns;
        private RowValidationError _errorInformation;
        private string _columnSeperator;

        public RowValidator()
        {
            _errorInformation = new RowValidationError();
            _columns = new ValidatorGroup[0];
        }

        public RowValidator(string columnSeperator) : this()
        {
            _columnSeperator = columnSeperator;
        }
        
        /// <summary>
        /// Runs all of the column validators against the string provided in <paramref name="toCheck"/>.
        /// </summary>
        /// <param name="toCheck">The row of data to validate</param>
        /// <returns>True if the row has passed validation else false.</returns>
        public bool IsValid(string toCheck)
        {
            bool isValid = true;
            string[] seperators = new string[] { _columnSeperator };
            string[] parts = toCheck.Split(seperators, StringSplitOptions.None);
            int[] columnIndexes = CalculateColumnStartIndexes(parts);

            for (int currentColumn = 0; currentColumn < parts.Length; currentColumn++)
            {
                if (currentColumn < _columns.Length)
                {
                    bool result = _columns[currentColumn].IsValid(parts[currentColumn]);

                    IList<ValidationError> newErrors = _columns[currentColumn].GetErrors();
                    _errorInformation.Errors.AddRange(newErrors);

                    // set validation character error location for all errors
                    // on this column to the first character in the column
                    for (int i = 0; i < newErrors.Count; i++) newErrors[i].AtCharacter = columnIndexes[currentColumn];

                    isValid = isValid & result;
                }
            }

            AddRowDetailsToErrors(toCheck);

            return isValid;
        }

        public RowValidationError GetError()
        {
            return _errorInformation;
        }

        public void ClearErrors()
        {
            _errorInformation = new RowValidationError();
            foreach(ValidatorGroup current in _columns)
            {
                current.ClearErrors();
            }
        }

        public void AddColumnValidator(int toColumn, IValidator validator)
        {
            CheckAndResizeColumnList(toColumn);

            _columns[toColumn - 1].Add(validator);
        }

        public List<ValidatorGroup> GetColumnValidators()
        {
            return new List<ValidatorGroup>(_columns);
        }

        private void CheckAndResizeColumnList(int allowColumnAt)
        {
            if (_columns.Length < allowColumnAt)
            {
                ValidatorGroup[] resizedColumns = new ValidatorGroup[allowColumnAt];
                for (int i = 0; i < _columns.Length; i++)
                {
                    resizedColumns[i] = _columns[i];
                }

                _columns = resizedColumns;

                MakeSureColumnsAreNotNull();
            }
        }

        private void MakeSureColumnsAreNotNull()
        {
            for (int i = 0; i < _columns.Length; i++)
            {
                if (_columns[i] == null)
                {
                    _columns[i] = new ValidatorGroup();
                }
            }
        }

        private void AddRowDetailsToErrors(string content)
        {
            _errorInformation.Content = content;
        }

        private int[] CalculateColumnStartIndexes(string[] parts)
        {
            int[] columnIndexes = new int[parts.Length];

            // find the indexes of each of the columns being provided
            int columnCounter = 1; // the first column always starts at zero
            int position = 0;
            if (columnIndexes.Length > 0)
            {
                columnIndexes[0] = 1;
            }
            foreach (string part in parts)
            {
                if (columnCounter < parts.Length)
                {
                    position += part.Length + 1; // add 1 for the seperator
                    columnIndexes[columnCounter++] = position + 1; // because we are not working zero based
                }
            }

            return columnIndexes;
        }

        public string ColumnSeperator
        {
            get { return _columnSeperator; }
            set { _columnSeperator = value; }
        }
    }
}
