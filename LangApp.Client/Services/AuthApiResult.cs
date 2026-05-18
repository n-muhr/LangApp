namespace LangApp.Client.Services;

public enum AuthErrorKind
{
    None,
    InvalidCredentials,
    DuplicateEmail,
    Validation,
    Network,
    Unknown
}

public class AuthApiResult<T>
{
    public T? Data { get; init; }
    public AuthErrorKind Error { get; init; }
    public string? Message { get; init; }

    public bool IsSuccess => Error == AuthErrorKind.None && Data is not null;

    public static AuthApiResult<T> Success(T data) => new() { Data = data, Error = AuthErrorKind.None };

    public static AuthApiResult<T> Fail(AuthErrorKind error, string? message = null) =>
        new() { Error = error, Message = message };
}
