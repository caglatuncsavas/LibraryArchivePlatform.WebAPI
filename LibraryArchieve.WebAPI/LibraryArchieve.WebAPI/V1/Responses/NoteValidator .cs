using FluentValidation;
using LibraryArchieve.WebAPI.V1.Requests;

namespace LibraryArchieve.WebAPI.V1.Responses;

public class NoteValidator : AbstractValidator<CreateNoteRequest>
{
    public NoteValidator()
    {
        RuleFor(note => note.Content).NotEmpty().WithMessage("Content is required.");
        RuleFor(note => note.BookId).NotEmpty().WithMessage("BookId is required.");
    }
}
