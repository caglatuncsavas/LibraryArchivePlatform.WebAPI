using FluentValidation.Results;
using LibraryArchieve.WebAPI.Data;
using LibraryArchieve.WebAPI.Data.Entities;
using LibraryArchieve.WebAPI.Services;
using LibraryArchieve.WebAPI.V1.Requests;
using LibraryArchieve.WebAPI.V1.Responses;
using LibraryArchieve.WebAPI.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace LibraryArchieve.WebAPI.V1.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController(
    ApplicationDbContext context,
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager,
    JwtProvider jwtProvider) : ControllerBase
{
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        LoginValidator validator = new LoginValidator();
        ValidationResult validationresult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationresult.IsValid)
        {
            return BadRequest(validationresult.Errors);
        }

        AppUser? appUser = await userManager.FindByNameAsync(request.UserNameOrEmail);

        if (appUser is null)
        {
            appUser = await userManager.FindByEmailAsync(request.UserNameOrEmail);
            
            if (appUser is null)
            {
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
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid Password!",
                Detail = "The password you entered is incorrect!."
            });
        }

        var roles =
            context.AppUserRoles
            .Where(p => p.UserId == appUser.Id)
            .Include(p => p.Role)
            .Select(s => s.Role!.Name)
            .ToList();

        LoginResponse response = await jwtProvider.CreateToken(appUser, roles);

        return Ok(response);

    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Register(RegisterRequest request, CancellationToken cancellationToken)
    {
        RegisterValidator validator = new RegisterValidator(userManager);
        ValidationResult validationresult = await validator.ValidateAsync(request);

        if (!validationresult.IsValid)
        {
            return BadRequest(validationresult.Errors);
        }

        AppUser appUser = new()
        {
            UserName = request.UserName,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            EmailConfirmed = true
        };

        IdentityResult result = await userManager.CreateAsync(appUser, request.Password);

        if (!result.Succeeded)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "User Creation Failed!",
            });
        }

        return Ok();
    }   
}
