using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormatValidator.Validators
{
    public class RowValidator : ValidationEntry
    {
        private char _columnSeperator;
        private ValidatorGroup[] _columns;
        private int _rowCounter;

        public RowValidator(char columnSeperator)
        {
            _rowCounter = 0;
            _columns = new ValidatorGroup[0];
            _columnSeperator = columnSeperator;
        }
        
        public override bool IsValid(string toCheck)
        {
            bool isValid = true;
            string[] parts = toCheck.Split(_columnSeperator);

            MoveRowCounterToCurrentRow();

            for(int i = 0; i < parts.Length; i++)
            {
                if(i < _columns.Length)
                {
                    bool currentResult = _columns[i].IsValid(parts[i]);
                    Errors.AddRange(_columns[i].GetErrors());
                    isValid = isValid & currentResult;
                }
            }

            AddRowDetailsToErrors();

            return isValid;
        }

        public override void ClearErrors()
        {
            base.ClearErrors();
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

        private void AddRowDetailsToErrors()
        {
            foreach(ValidationError current in Errors)
            {
                current.Row = _rowCounter;
            }
        }

        private void MoveRowCounterToCurrentRow()
        {
            _rowCounter++;
        }
    }
}
