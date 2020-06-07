
namespace FormatValidator.Configuration
{
    internal interface IReader
    {
        ValidatorConfiguration Read(string content);
    }
}
