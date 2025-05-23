namespace TaskManager.Application.Exceptions;

public class NotFountException : Exception
{
    public NotFountException(string name, object key) : base($"Entity \"{name}\" ({key}) was not found.")
    {
    }
   
}
