using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ValidateTests")]
namespace FormatValidator
{
    using System.Collections.Generic;
    using Validators;

    public class Validator
    {
        private RowValidator _rowValidator;
        private string _rowSeperator;
        private int _totalRowsChecked;
        private bool _hasHeaderRow;

        internal Validator()
        {
            _rowValidator = new RowValidator();
            _rowSeperator = "\r\n";
        }

        public static Validator FromJson(string json)
        {
            ValidatorConfiguration configuration = new JsonReader().Read(json);

            return FromConfiguration(configuration);
        }

        public static Validator FromConfiguration(ValidatorConfiguration configuration)
        {
            ConfigurationConvertor converter = new ConfigurationConvertor(configuration);
            ConvertedValidators converted = converter.Convert();

            Validator validator = new Validator();
            validator.SetColumnSeperator(converted.ColumnSeperator);
            validator.SetRowSeperator(converted.RowSeperator);
            validator.TransferConvertedColumns(converted);
            validator._hasHeaderRow = converted.HasHeaderRow;

            return validator;
        }

        public IEnumerable<RowValidationError> Validate(ISourceReader reader)
        {
            foreach(string line in reader.ReadLines(_rowSeperator))
            {
                _totalRowsChecked++;

                if (IsHeaderRow())
                {
                }
                else if (!_rowValidator.IsValid(line))
                {
                    RowValidationError error = _rowValidator.GetError();
                    error.Row = _totalRowsChecked;
                    _rowValidator.ClearErrors();

                    yield return error;
                }
            }
        }

        public void SetColumnSeperator(string seperator)
        {
            if (string.IsNullOrEmpty(seperator))
            {
                _rowValidator.ColumnSeperator = ",";
            }
            else
            {
                _rowValidator.ColumnSeperator = seperator;
            }
        }

        public void SetRowSeperator(string rowSeperator)
        {
            if(!string.IsNullOrEmpty(rowSeperator))
            {
                _rowSeperator = rowSeperator;
            }                
        }

        internal List<ValidatorGroup> GetColumnValidators()
        {
            return _rowValidator.GetColumnValidators();
        }

        private void TransferConvertedColumns(ConvertedValidators converted)
        {
            foreach (KeyValuePair<int, List<IValidator>> column in converted.Columns)
            {
                foreach (IValidator columnValidator in column.Value)
                {
                    _rowValidator.AddColumnValidator(column.Key, columnValidator);
                }
            }
        }

        private bool IsHeaderRow() => _hasHeaderRow && _totalRowsChecked == 1;

        public int TotalRowsChecked
        {
            get {  return _totalRowsChecked; }
        }
    }
}
