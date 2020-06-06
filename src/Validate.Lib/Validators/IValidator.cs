
namespace FormatValidator.Validators
{
    using System.Collections.Generic;

    public interface IValidator
    {
        bool IsValid(string toCheck);

        IList<ValidationError> GetErrors();

        void ClearErrors();
    }
}
