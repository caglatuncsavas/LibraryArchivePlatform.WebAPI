namespace LibraryArchieve.WebAPI.V1.Requests;

public sealed record RegisterRequest(
    string FirstName,
    string LastName,
    string Email,
    string UserName,
    string Password);
