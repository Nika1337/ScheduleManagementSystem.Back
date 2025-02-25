
namespace Domain.Exceptions;

public class PasswordStructureValidationException : Exception
{
    public PasswordStructureValidationException(string message) : base(message)
    {

    }
}
