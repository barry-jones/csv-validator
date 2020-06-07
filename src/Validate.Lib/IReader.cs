
namespace FormatValidator
{
    internal interface IReader
    {
        ValidatorConfiguration Read(string content);
    }
}
