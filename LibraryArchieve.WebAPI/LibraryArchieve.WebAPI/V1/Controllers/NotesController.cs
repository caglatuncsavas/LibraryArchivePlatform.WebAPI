using AutoMapper;
using LibraryArchieve.WebAPI.Data.Entities;
using LibraryArchieve.WebAPI.Data.Enums;
using LibraryArchieve.WebAPI.Repositories;
using LibraryArchieve.WebAPI.V1.Requests;
using LibraryArchieve.WebAPI.V1.Responses;
using Microsoft.AspNetCore.Mvc;

namespace LibraryArchieve.WebAPI.V1.Controllers;
[Route("api/notes")]
[ApiController]
public class NotesController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<NotesController> _logger;

    public NotesController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<NotesController> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateNoteResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> CreateNote([FromBody] CreateNoteRequest request)
    {
        _logger.LogInformation("Creating a new note with details: {@Request}", request);

        var validator = new NoteValidator();
        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Note creation failed validation: {@ValidationResult}", validationResult.Errors);
            return BadRequest(validationResult.Errors);
        }

        var note = _mapper.Map<Note>(request);
        note = await _unitOfWork.Notes.CreateNoteAsync(note);
        await _unitOfWork.SaveChangesAsync();

        CreateNoteResponse response = _mapper.Map<CreateNoteResponse>(note);

        _logger.LogInformation("Note created successfully with ID: {NoteId}", response.Id);

        return Created(new Uri($"/api/notes/{response.Id}", UriKind.Relative), response);
    }

    [HttpGet("{bookId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<QueryNoteResponse>))]
    public async Task<IActionResult> QueryNotes([FromRoute] int bookId, [FromQuery] PrivacySetting privacySetting, [FromQuery] Guid userId)
    {
        _logger.LogInformation("Fetching notes for book ID: {BookId} with privacy setting: {PrivacySetting}", bookId, privacySetting);

        var notes = await _unitOfWork.Notes.GetNotesByBookIdAsync(bookId, privacySetting, userId);

        List<QueryNoteResponse> response = notes.Select(note => _mapper.Map<QueryNoteResponse>(note)).ToList();

        return Ok(response);
    }


    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UpdateNoteResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateNote([FromRoute] int id, [FromBody] UpdateNoteRequest request)
    {
        _logger.LogInformation("Updating note with ID: {Id} and details: {@Request}", id, request);

        Note? note = await _unitOfWork.Notes.GetNoteByIdAsync(id);

        if (note is null)
        {
            _logger.LogWarning("Note with ID: {Id} not found", id);
            return NotFound();
        }

        _mapper.Map(request, note);

        await _unitOfWork.Notes.UpdateNoteAsync(note);
        await _unitOfWork.SaveChangesAsync();

        UpdateNoteResponse response = _mapper.Map<UpdateNoteResponse>(note);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteNote([FromRoute] int id)
    {
        _logger.LogInformation("Deleting note with ID: {Id}", id);

        bool result = await _unitOfWork.Notes.DeleteNoteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        if (!result)
        {
            _logger.LogWarning("Note with ID: {Id} not found", id);
            return NotFound();
        }

        return NoContent();
    }
}


