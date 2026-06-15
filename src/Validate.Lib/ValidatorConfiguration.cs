
namespace FormatValidator
{
    using System.Collections.Generic;

    /// <summary>
    /// A direct representation of the Data read from the configuration file
    /// </summary>
    /// <seealso cref="ConfigurationConvertor"/>
    /// <seealso cref="ConvertedValidators"/>
    public class ValidatorConfiguration
    {
        /// <summary>
        /// Initialises a new instance of ValidatorConfiguration
        /// </summary>
        public ValidatorConfiguration()
        {
            Columns = new Dictionary<int, ColumnValidatorConfiguration>();
        }

        public string ColumnSeperator { get; set; }

        public string RowSeperator { get; set; }

        public bool HasHeaderRow { get; set; }

        /// <summary>
        /// When true, every row (including the header) must have exactly the
        /// number of columns defined by the column schema; otherwise a row-level
        /// column-count error is reported. Defaults to false (off).
        /// </summary>
        public bool StrictColumns { get; set; }

        public Dictionary<int, ColumnValidatorConfiguration> Columns { get; set; }
    }
}
