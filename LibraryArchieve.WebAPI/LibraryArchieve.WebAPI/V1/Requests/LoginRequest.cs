namespace LibraryArchieve.WebAPI.V1.Requests;

public sealed record LoginRequest(
    string UserNameOrEmail,
    string Password);