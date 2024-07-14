using FluentValidation;
using LibraryArchieve.WebAPI.V1.Requests;

namespace LibraryArchieve.WebAPI.Validators;
public class BookValidator : AbstractValidator<CreateBookRequest>
{
    public BookValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Author).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.CoverImageUrl).NotEmpty();
        RuleFor(x => x.ShelfLocation).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThanOrEqualTo(0);
        RuleFor(x => x.ISBN).NotEmpty();
        RuleFor(x => x.CategoryNames).NotEmpty();
    }
}
