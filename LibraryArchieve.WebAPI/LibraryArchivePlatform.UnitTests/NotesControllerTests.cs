using AutoMapper;
using FluentAssertions;
using LibraryArchieve.WebAPI.Data.Entities;
using LibraryArchieve.WebAPI.Data.Enums;
using LibraryArchieve.WebAPI.Repositories;
using LibraryArchieve.WebAPI.V1.Controllers;
using LibraryArchieve.WebAPI.V1.Requests;
using LibraryArchieve.WebAPI.V1.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace LibraryArchivePlatform.UnitTests;
public class NotesControllerTests
{
    private readonly NotesController _sut;
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly ILogger<NotesController> _logger = Substitute.For<ILogger<NotesController>>();

    public NotesControllerTests()
    {
        _sut = new NotesController(_unitOfWork, _mapper, _logger);
    }

    [Fact]
    public async Task CreateNote_ShouldReturnCreated_WhenNoteIsValid()
    {
        // Arrange
        var createNoteRequest = new CreateNoteRequest
        {
            BookId = 1,
            Content = "Sample note content"
        };

        var note = new Note
        {
            Id = 1,
            BookId = createNoteRequest.BookId,
            Content = createNoteRequest.Content
        };

        var createNoteResponse = new CreateNoteResponse
        {
            Id = note.Id,
            BookId = note.BookId,
            Content = note.Content
        };

        _mapper.Map<Note>(createNoteRequest).Returns(note);
        _unitOfWork.Notes.CreateNoteAsync(Arg.Any<Note>()).Returns(note);
        _mapper.Map<CreateNoteResponse>(note).Returns(createNoteResponse);

        // Act
        var result = await _sut.CreateNote(createNoteRequest);

        // Assert
        result.Should().BeOfType<CreatedResult>();
        var createdResult = result as CreatedResult;
        createdResult.Value.Should().Be(createNoteResponse);
    }

    [Fact]
    public async Task QueryNotes_ShouldReturnOk_WhenNotesExist()
    {
        // Arrange
        var bookId = 1;
        var userId = Guid.NewGuid();
        var privacySetting = PrivacySetting.Public;

        var notes = new List<Note>
            {
                new Note { Id = 1, BookId = bookId, Content = "Note 1" },
                new Note { Id = 2, BookId = bookId, Content = "Note 2" }
            };

        var queryNoteResponses = notes.Select(note => new QueryNoteResponse
        {
            Id = note.Id,
            BookId = note.BookId,
            Content = note.Content
        }).ToList();

        _unitOfWork.Notes.GetNotesByBookIdAsync(bookId, privacySetting, userId).Returns(notes);
        _mapper.Map<QueryNoteResponse>(Arg.Any<Note>()).Returns(args =>
        {
            var note = args.Arg<Note>();
            return new QueryNoteResponse
            {
                Id = note.Id,
                BookId = note.BookId,
                Content = note.Content
            };
        });

        // Act
        var result = await _sut.QueryNotes(bookId, privacySetting, userId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(queryNoteResponses);
    }

    [Fact]
    public async Task UpdateNote_ShouldReturnOk_WhenNoteIsValid()
    {
        // Arrange
        var noteId = 1;
        var updateNoteRequest = new UpdateNoteRequest
        {
            Content = "Updated content",
        };

        var note = new Note
        {
            Id = noteId,
            Content = "Old content",
        };

        var updateNoteResponse = new UpdateNoteResponse
        {
            Id = note.Id,
            Content = updateNoteRequest.Content
        };

        _unitOfWork.Notes.GetNoteByIdAsync(noteId).Returns(note);
        _mapper.Map(updateNoteRequest, note).Returns(note);
        _unitOfWork.Notes.UpdateNoteAsync(note).Returns(note);
        _mapper.Map<UpdateNoteResponse>(note).Returns(updateNoteResponse);

        // Act
        var result = await _sut.UpdateNote(noteId, updateNoteRequest);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Value.Should().Be(updateNoteResponse);
    }

    [Fact]
    public async Task DeleteNote_ShouldReturnNoContent_WhenNoteIsDeleted()
    {
        // Arrange
        var noteId = 1;
        _unitOfWork.Notes.DeleteNoteAsync(noteId).Returns(true);

        // Act
        var result = await _sut.DeleteNote(noteId);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task DeleteNote_ShouldReturnNotFound_WhenNoteDoesNotExist()
    {
        // Arrange
        var noteId = 1;
        _unitOfWork.Notes.DeleteNoteAsync(noteId).Returns(false);

        // Act
        var result = await _sut.DeleteNote(noteId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}

