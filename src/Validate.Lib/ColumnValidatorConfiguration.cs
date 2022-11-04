
namespace FormatValidator
{
    public class ColumnValidatorConfiguration
    {
        public string Name { get; set; }

        public bool Unique { get; set; }

        public int MaxLength { get; set; }

        public string Pattern { get; set; }

        public bool IsNumeric { get; set; }

        public bool IsRequired { get; set; }        

        public bool Trim { get; set; }
    }
}
