using AutoMapper;
using LibraryArchieve.WebAPI.Data;
using LibraryArchieve.WebAPI.Data.Entities;
using LibraryArchieve.WebAPI.Repositories;
using LibraryArchieve.WebAPI.V1.Requests;
using LibraryArchieve.WebAPI.V1.Responses;
using LibraryArchieve.WebAPI.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryArchieve.WebAPI.V1.Controllers;
[Route("api/book-shelves")]
[ApiController]
public class BookShelvesController (
    IUnitOfWork _unitOfWork,
    IMapper _mapper,
    ILogger<BookShelvesController> _logger)  : ControllerBase
{
    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateBookShelfResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> CreateBookShelf([FromBody] CreateBookShelfRequest request)
    {
        _logger.LogInformation("Creating a new book shelf with details: {@Request}", request);


        var validator = new BookShelfValidator();
        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Book shelf creation failed validation: {@ValidationResult}", validationResult.Errors);
            return BadRequest(validationResult.Errors);
        }

        var bookShelf = _mapper.Map<BookShelf>(request);
        bookShelf = await _unitOfWork.BookShelves.CreateBookShelfAsync(bookShelf);

        CreateBookShelfResponse response = _mapper.Map<CreateBookShelfResponse>(bookShelf);

        _logger.LogInformation("Book shelf created successfully with ID: {BookShelfId}", response.Id);

        return Created(new Uri($"/api/book-shelves/{response.Id}", UriKind.Relative), response);
    }
}
