
namespace FormatValidator
{
    using System.Collections.Generic;

    public class ColumnValidatorConfiguration
    {
        public string Name { get; set; }

        public bool Unique { get; set; }

        public int MaxLength { get; set; }

        /// <summary>
        /// The minimum number of characters required for this column. When greater than zero, any cell value shorter than this length is a validation error.
        /// </summary>
        public int MinLength { get; set; }

        public string Pattern { get; set; }

        public bool IsNumeric { get; set; }

        public bool IsRequired { get; set; }

        /// <summary>
        /// The set of permitted values for this column. When non-empty, any cell value not in this list is a validation error.
        /// </summary>
        public List<string> AllowedValues { get; set; }
    }
}
