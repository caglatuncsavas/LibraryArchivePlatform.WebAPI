using AutoMapper;
using FluentAssertions;
using LibraryArchieve.WebAPI.Data.Entities;
using LibraryArchieve.WebAPI.Repositories;
using LibraryArchieve.WebAPI.V1.Controllers;
using LibraryArchieve.WebAPI.V1.Requests;
using LibraryArchieve.WebAPI.V1.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace LibraryArchivePlatform.UnitTests;
public class BooksControllerTests
{
    private readonly BooksController _sut; // System Under Test
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly ILogger<BooksController> _logger = Substitute.For<ILogger<BooksController>>();

    public BooksControllerTests()
    {
        _sut = new BooksController(_unitOfWork, _mapper, _logger);
    }

    [Fact]
    public async Task QueryBooks_ShouldReturnOk_WhenBooksExist()
    {
        // Arrange
        var request = new QueryBooksRequest { Title = "Test" };
        var books = new List<Book>
            {
                new Book { Id = 1, Title = "Test Book", Author = "Author" }
            };
        _unitOfWork.Books.QueryBooksAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool?>(), Arg.Any<int?>(), Arg.Any<string>())
            .Returns(books);
        _mapper.Map<QueryBooksResponse>(Arg.Any<Book>()).Returns(new QueryBooksResponse { Id = 1, Title = "Test Book" });

        // Act
        var result = await _sut.QueryBooks(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Value.Should().BeOfType<List<QueryBooksResponse>>();
        var response = okResult.Value as List<QueryBooksResponse>;
        response.Should().HaveCount(1);
    }

    [Fact]
    public async Task CreateBook_ShouldReturnCreated_WhenBookIsValid()
    {
        // Arrange
        var createBookRequest = new CreateBookRequest
        {
            Title = "Sample Book Title",
            Author = "Sample Author",
            Description = "Sample Description",
            CoverImageUrl = "https://example.com/sample-cover.jpg",
            ShelfLocation = "A1",
            Quantity = 10,
            ISBN = "123-456-789",
            CategoryNames = new List<string> { "Fiction", "Adventure" }
        };

        var book = new Book
        {
            Id = 1,
            Title = createBookRequest.Title,
            Author = createBookRequest.Author,
            Description = createBookRequest.Description,
            CoverImageUrl = createBookRequest.CoverImageUrl,
            ShelfLocation = createBookRequest.ShelfLocation,
            Quantity = createBookRequest.Quantity,
            ISBN = createBookRequest.ISBN
        };

        var createBookResponse = new CreateBookResponse
        {
            Id = book.Id,
            Title = book.Title
        };

        _mapper.Map<Book>(createBookRequest).Returns(book);
        _unitOfWork.Books.CreateBookAsync(Arg.Any<Book>()).Returns(book);
        _mapper.Map<CreateBookResponse>(book).Returns(createBookResponse);

        // Act
        var result = await _sut.CreateBook(createBookRequest);

        // Assert
        result.Should().BeOfType<CreatedResult>();
        var createdResult = result as CreatedResult;
        createdResult.Value.Should().Be(createBookResponse);
    }

    [Fact]
    public async Task UpdateUser_ShouldReturnOk_WhenBookExists()
    {
        // Arrange
        var id = 1;
        var request = new UpdateBookRequest { Title = "Updated Book", Author = "Updated Author" };
        var book = new Book { Id = id, Title = "Original Title", Author = "Original Author" };
        var updateBookResponse = new UpdateBookResponse { Id = id, Title = "Updated Book" };

        _unitOfWork.Books.GetBookByIdAsync(id).Returns(book);
        _mapper.Map(request, book).Returns(book);
        _unitOfWork.Books.UpdateBookAsync(book).Returns(book);
        _mapper.Map<UpdateBookResponse>(book).Returns(updateBookResponse);

        // Act
        var result = await _sut.UpdateUser(id, request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Value.Should().Be(updateBookResponse);
    }

    [Fact]
    public async Task DeleteBook_ShouldReturnNoContent_WhenBookExists()
    {
        // Arrange
        var id = 1;
        var book = new Book { Id = id, Title = "Book to Delete" };

        _unitOfWork.Books.GetBookByIdAsync(id).Returns(book);

        // Act
        var result = await _sut.DeleteBook(id);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        await _unitOfWork.Books.Received(1).DeleteBookAsync(book);
    }
}

