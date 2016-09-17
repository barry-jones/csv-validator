using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormatValidator.Validators
{
    public class RowValidator
    {
        private char _columnSeperator;
        private ValidatorGroup[] _columns;
        private int _rowCounter;
        private RowValidationError _errorInformation;

        public RowValidator(char columnSeperator)
        {
            _rowCounter = 0;
            _errorInformation = new RowValidationError();
            _columns = new ValidatorGroup[0];
            _columnSeperator = columnSeperator;
        }
        
        public bool IsValid(string toCheck)
        {
            bool isValid = true;
            string[] parts = toCheck.Split(_columnSeperator);

            MoveRowCounterToCurrentRow();

            for(int i = 0; i < parts.Length; i++)
            {
                if(i < _columns.Length)
                {
                    bool currentResult = _columns[i].IsValid(parts[i]);

                    IList<ValidationError> newErrors = _columns[i].GetErrors();
                    _errorInformation.Errors.AddRange(newErrors);

                    isValid = isValid & currentResult;
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
            _errorInformation.Row = _rowCounter;
            _errorInformation.Content = content;
        }

        private void MoveRowCounterToCurrentRow()
        {
            _rowCounter++;
        }
    }
}
