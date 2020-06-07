
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
        public ValidatorConfiguration()
        {
            Columns = new Dictionary<int, ColumnValidatorConfiguration>();
        }

        public string ColumnSeperator { get; set; }

        public string RowSeperator { get; set; }

        public bool HasHeaderRow { get; set; }

        public Dictionary<int, ColumnValidatorConfiguration> Columns { get; set; }
    }
}
