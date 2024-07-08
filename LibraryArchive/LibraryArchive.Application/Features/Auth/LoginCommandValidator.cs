using FluentValidation;

namespace LibraryArchive.Application.Features.Auth;
public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.EmailOrUsername)
            .NotEmpty().WithMessage("Email or username is required")
            .EmailAddress().When(x => x.EmailOrUsername.Contains('@')).WithMessage("Invalid email address")
            .MinimumLength(3).WithMessage("Email or username must be at least 3 characters");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters");
    }
}
