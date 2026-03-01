namespace Domain.Exceptions;

public class UserNotFoundException : NotFoundException
{
    public UserNotFoundException(string userEmail) : base($"User with Email {userEmail} was not found.")
    {
    }
}