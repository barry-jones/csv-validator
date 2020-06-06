
namespace FormatValidator.Configuration
{
    interface IReader
    {
        ValidatorConfiguration Read(string content);
    }
}
