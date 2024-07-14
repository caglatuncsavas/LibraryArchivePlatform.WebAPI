using AutoMapper;
using FluentValidation.Results;
using LibraryArchieve.WebAPI.Data.Entities;
using LibraryArchieve.WebAPI.Repositories;
using LibraryArchieve.WebAPI.V1.Requests;
using LibraryArchieve.WebAPI.V1.Responses;
using LibraryArchieve.WebAPI.Validators;
using Microsoft.AspNetCore.Mvc;

namespace LibraryArchieve.WebAPI.V1.Controllers;
[Route("api/books")]
[ApiController]
public class BooksController(
    IUnitOfWork _unitOfWork,
    IMapper _mapper,
    ILogger<BooksController> _logger) : ControllerBase
{
    [HttpGet("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<QueryBooksResponse>))]
    public async Task<IActionResult> QueryBooks([FromQuery] QueryBooksRequest request)
    {
        _logger.LogInformation("Querying books with parameters: {@Request}", request);

        var books = await _unitOfWork.Books.QueryBooksAsync(
                 request.Title,
                 request.Author,
                 request.ISBN,
                 request.IsActive,
                 request.CategoryId,
                 request.ShelfLocation
             );

        List<QueryBooksResponse> response = books
            .Select(x => _mapper.Map<QueryBooksResponse>(x))
            .ToList();

        return Ok(response);
    }

    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateBookResponse))]
    public async Task<IActionResult> CreateBook([FromBody] CreateBookRequest request)
    {
        _logger.LogInformation("Creating book with details: {@Request}", request);

        BookValidator validator = new BookValidator();
        ValidationResult validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Book creation failed validation: {@ValidationResult}", validationResult.Errors);
            return BadRequest(validationResult.Errors);
        }

        Book book = _mapper.Map<Book>(request);

        book = await _unitOfWork.Books.CreateBookAsync(book);

        CreateBookResponse response = _mapper.Map<CreateBookResponse>(book);

        return Created(new Uri($"/api/books/{response.Id}", UriKind.Relative), response);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UpdateBookResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUser([FromRoute] int id, [FromBody] UpdateBookRequest request)
    {
        _logger.LogInformation("Updating book with ID: {Id} and details: {@Request}", id, request);

        Book? book = await _unitOfWork.Books.GetBookByIdAsync(id);

        if (book is null)
        {
            _logger.LogWarning("Book with ID: {Id} not found", id);
            return NotFound();
        }

        _mapper.Map(request, book);

        await _unitOfWork.Books.UpdateBookAsync(book);

        UpdateBookResponse response = _mapper.Map<UpdateBookResponse>(book);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteBook([FromRoute] int id)
    {
        _logger.LogInformation("Deleting book with ID: {Id}", id);

        Book? book = await _unitOfWork.Books.GetBookByIdAsync(id);
        if (book is null)
        {
            _logger.LogWarning("Book with ID: {Id} not found", id);
            return NotFound();
        }

        await _unitOfWork.Books.DeleteBookAsync(book);

        return NoContent();
    }
}
