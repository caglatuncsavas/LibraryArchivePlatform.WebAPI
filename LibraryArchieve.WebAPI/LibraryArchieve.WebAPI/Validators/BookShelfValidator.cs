using FluentValidation;
using LibraryArchieve.WebAPI.V1.Requests;

namespace LibraryArchieve.WebAPI.Validators;

public class BookShelfValidator : AbstractValidator<CreateBookShelfRequest>
{
    public BookShelfValidator()
    {
        RuleFor(x => x.BookId).NotEmpty();
        RuleFor(x => x.Location).NotEmpty();
        RuleFor(x => x.Section).NotEmpty();
        RuleFor(x => x.Shelf).NotEmpty();
        RuleFor(x => x.Notes).NotEmpty();
    }
}
