using FluentValidation;
using LibraryArchieve.WebAPI.V1.Requests;

namespace LibraryArchieve.WebAPI.Validators;

public sealed class LoginValidator : AbstractValidator<LoginRequest>
{
    public LoginValidator()
    {
        RuleFor(x => x.UserNameOrEmail)
            .NotEmpty()
            .WithMessage("Username or email is required");

        RuleFor(p => p.UserNameOrEmail)
            .MinimumLength(3)
            .WithMessage("Geçerli bir kullanıcı adı veya mail adresi giriniz!");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required");

        RuleFor(p => p.Password)
            .Matches("[A-Z]")
            .WithMessage("Şifreniz en az bir büyük harf içermelidir!");
        
        RuleFor(p => p.Password)
            .Matches("[a-z]")
            .WithMessage("Şifreniz en az bir küçük harf içermelidir!");

        RuleFor(p => p.Password)
            .Matches("[0-9]")
            .WithMessage("Şifreniz en az bir rakam içermelidir!");

        RuleFor(p => p.Password)
            .Matches("[^a-zA-Z0-9]")
            .WithMessage("Şifreniz en az bir özel karakter içermelidir!");


    }
}
