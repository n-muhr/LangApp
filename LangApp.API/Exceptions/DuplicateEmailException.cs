namespace LangApp.API.Exceptions;

public class DuplicateEmailException : Exception
{
    public DuplicateEmailException() : base("An account with this email already exists.") { }
}
