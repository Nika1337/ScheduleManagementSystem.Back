
namespace Domain.Exceptions;


public class PasswordIncorrectException : Exception
{
    public PasswordIncorrectException(Guid id) : base($"Password for user with id '{id}' is incorrect")
    {

    }
}
