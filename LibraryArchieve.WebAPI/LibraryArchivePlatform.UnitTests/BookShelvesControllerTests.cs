using AutoMapper;
using FluentAssertions;
using LibraryArchieve.WebAPI.Data.Entities;
using LibraryArchieve.WebAPI.Repositories;
using LibraryArchieve.WebAPI.V1.Controllers;
using LibraryArchieve.WebAPI.V1.Requests;
using LibraryArchieve.WebAPI.V1.Responses;
using LibraryArchieve.WebAPI.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace LibraryArchivePlatform.UnitTests;
public class BookShelvesControllerTests
{
    private readonly BookShelvesController _sut;
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly ILogger<BookShelvesController> _logger = Substitute.For<ILogger<BookShelvesController>>();

    public BookShelvesControllerTests()
    {
        _sut = new BookShelvesController(_unitOfWork, _mapper, _logger);
    }

    [Fact]
    public async Task CreateBookShelf_ShouldReturnCreated_WhenBookShelfIsValid()
    {
        // Arrange
        var createBookShelfRequest = new CreateBookShelfRequest
        {
            BookId = 1,
            Location = "Library",
            Section = "Section A",
            Shelf = "Shelf 1",
            Notes = "Some notes"
        };

        var bookShelf = new BookShelf
        {
            Id = 1,
            BookId = createBookShelfRequest.BookId,
            Location = createBookShelfRequest.Location,
            Section = createBookShelfRequest.Section,
            Shelf = createBookShelfRequest.Shelf,
            Notes = createBookShelfRequest.Notes
        };

        var createBookShelfResponse = new CreateBookShelfResponse
        {
            Id = bookShelf.Id,
            BookId = bookShelf.BookId,
            Location = bookShelf.Location,
            Section = bookShelf.Section,
            Shelf = bookShelf.Shelf,
            Notes = bookShelf.Notes
        };

        _mapper.Map<BookShelf>(createBookShelfRequest).Returns(bookShelf);
        _unitOfWork.BookShelves.CreateBookShelfAsync(Arg.Any<BookShelf>()).Returns(bookShelf);
        _mapper.Map<CreateBookShelfResponse>(bookShelf).Returns(createBookShelfResponse);

        // Act
        var result = await _sut.CreateBookShelf(createBookShelfRequest);

        // Assert
        result.Should().BeOfType<CreatedResult>();
        var createdResult = result as CreatedResult;
        createdResult.Value.Should().Be(createBookShelfResponse);
    }

    [Fact]
    public async Task CreateBookShelf_ShouldReturnBadRequest_WhenValidationFails()
    {
        // Arrange
        var createBookShelfRequest = new CreateBookShelfRequest
        {
            // Hatalı veriler
            Shelf = string.Empty,
            Location = string.Empty
        };

        var validator = new BookShelfValidator();
        var validationResult = await validator.ValidateAsync(createBookShelfRequest);

        _mapper.Map<BookShelf>(createBookShelfRequest).Returns((BookShelf)null);

        // Act
        var result = await _sut.CreateBookShelf(createBookShelfRequest);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.Value.Should().BeEquivalentTo(validationResult.Errors);
    }
}

