using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormatValidator
{
    public class Validator
    {
        private bool _hasRequirements;

        public Validator()
        {
            _hasRequirements = false;
        }

        public void LoadRequirements(string fromFile)
        {
        }

        public bool HasRequirments
        {
            get
            {
                return _hasRequirements;
            }
        }
    }
}
