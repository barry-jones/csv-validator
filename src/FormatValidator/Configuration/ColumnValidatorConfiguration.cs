using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormatValidator.Configuration
{
    public class ColumnValidatorConfiguration
    {
        public bool Unique { get; set; }

        public int MaxLength { get; set; }

        public string Pattern { get; set; }

        public bool IsNumeric { get; set; }

        public bool IsRequired { get; set; }
    }
}
