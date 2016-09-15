using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormatValidator.Configuration
{
    interface IReader
    {
        ValidatorConfiguration Read(string content);
    }
}
