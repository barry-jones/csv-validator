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
    }
}
