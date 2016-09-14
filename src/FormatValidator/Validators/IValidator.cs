using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormatValidator.Validators
{
    public interface IValidator
    {
        bool IsValid(string toCheck);

        IList<ValidationError> GetErrors();
    }
}
