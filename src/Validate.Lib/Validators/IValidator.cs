
namespace FormatValidator.Validators
{
    using System.Collections.Generic;

    internal interface IValidator
    {
        bool IsValid(string toCheck);

        IList<ValidationError> GetErrors();

        void ClearErrors();
    }
}
