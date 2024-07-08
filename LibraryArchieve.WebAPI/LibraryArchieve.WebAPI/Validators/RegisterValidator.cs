using FluentValidation;
using LibraryArchieve.WebAPI.Data.Entities;
using LibraryArchieve.WebAPI.V1.Requests;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LibraryArchieve.WebAPI.Validators;

public class RegisterValidator : AbstractValidator<RegisterRequest>
{
    public RegisterValidator(UserManager<AppUser> userManager)
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .MustAsync(async (userName, cancellationToken) => !await userManager.Users.AnyAsync(p => p.UserName == userName, cancellationToken))
            .WithMessage("User name already exists");
        
        RuleFor(p => p.FirstName).MinimumLength(3);
        
        RuleFor(p => p.LastName).MinimumLength(3);

        RuleFor(p => p.Email)
            .EmailAddress()
            .MustAsync(async (email, cancellationToken) => !await userManager.Users.AnyAsync(p => p.Email == email, cancellationToken))
            .WithMessage("Email address already exists");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required");

        RuleFor(p => p.Password)
            .Matches("[A-Z]")
            .WithMessage("Your password must contain at least one uppercase letter!");

        RuleFor(p => p.Password)
            .Matches("[a-z]")
            .WithMessage("Your password must contain at least one lowercase letter!");

        RuleFor(p => p.Password)
            .Matches("[0-9]")
            .WithMessage("Your password must contain at least one number!");

        RuleFor(p => p.Password)
            .Matches("[^a-zA-Z0-9]")
            .WithMessage("Your password must contain at least one special character!");
    }
}
