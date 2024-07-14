using LibraryArchieve.WebAPI.Data.Entities;
using LibraryArchieve.WebAPI.Repositories;
using LibraryArchieve.WebAPI.Services;
using LibraryArchieve.WebAPI.V1.Requests;
using LibraryArchieve.WebAPI.V1.Responses;
using LibraryArchieve.WebAPI.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace LibraryArchieve.WebAPI.V1.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> signInManager;
    private readonly JwtProvider jwtProvider;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IUnitOfWork unitOfWork,
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        JwtProvider jwtProvider,
        ILogger<AuthController> logger)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        signInManager = signInManager;
        jwtProvider = jwtProvider;
        _logger = logger;
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Attempting to log in with details: {@Request}", request);

        LoginValidator validator = new LoginValidator();
        ValidationResult validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Login failed validation: {@ValidationResult}", validationResult.Errors);
            return BadRequest(validationResult.Errors);
        }

        AppUser? appUser = await _unitOfWork.Auth.FindByUsernameOrEmailAsync(request.UserNameOrEmail);

        if (appUser is null)
        {
            appUser = await _unitOfWork.Auth.FindByUsernameOrEmailAsync(request.UserNameOrEmail);
            
            if (appUser is null)
            {
                _logger.LogWarning("User not found: {UserNameOrEmail}", request.UserNameOrEmail);
                return BadRequest(new ProblemDetails
                {
                    Title = "User Not Found!",
                    Detail = "The username or email you entered is not found!."
                });
            }
        }

       var result = await signInManager.CheckPasswordSignInAsync(appUser, request.Password, false);

        if (!result.Succeeded)
        {
            _logger.LogWarning("Invalid password for user: {UserNameOrEmail}", request.UserNameOrEmail);
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid Password!",
                Detail = "The password you entered is incorrect!."
            });
        }

        var roles = await _unitOfWork.Auth.GetUserRolesAsync(appUser);
        var roleList = roles.ToList();

        LoginResponse response = await jwtProvider.CreateToken(appUser, roleList);

        return Ok(response);
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Register(RegisterRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Attempting to register with details: {@Request}", request);

        RegisterValidator validator = new RegisterValidator(_userManager);
        ValidationResult validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Registration failed validation: {@ValidationResult}", validationResult.Errors);
            return BadRequest(validationResult.Errors);
        }

        AppUser appUser = new()
        {
            UserName = request.UserName,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            EmailConfirmed = true
        };

        IdentityResult result = await _unitOfWork.Auth.CreateUserAsync(appUser, request.Password);
        await _unitOfWork.SaveChangesAsync();

        if (!result.Succeeded)
        {
            _logger.LogError("User creation failed: {@Result}", result);
            return BadRequest(new ProblemDetails
            {
                Title = "User Creation Failed!",
            });
        }

        return Ok();
    }   
}
