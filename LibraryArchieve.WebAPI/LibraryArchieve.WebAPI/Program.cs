using LibraryArchieve.WebAPI.Data;
using LibraryArchieve.WebAPI.Data.Entities;
using LibraryArchieve.WebAPI.Repositories;
using LibraryArchieve.WebAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.TryAddScoped<JwtProvider>();

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddScoped<IBookRepository, BookRepository>();

builder.Services.AddScoped<IBookShelvesRepository, BookShelvesRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidateAudience = true, 
        ValidateIssuerSigningKey = true, 
        ValidateLifetime = true, 
        ValidIssuer = "Cagla Tunc Savas",
        ValidAudience = "Library Archieve Platform",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("my secret key my secret key my secret key 1234...my secret key my secret key my secret key 1234..."))
    };
});


builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer("Data Source=CAGLA\\SQLEXPRESS;Initial Catalog=LibraryArchieveWithWebApi;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
});

builder.Services.AddIdentity<AppUser, AppRole>(options =>
{
    options.Password.RequiredLength = 6;
    options.SignIn.RequireConfirmedEmail = true;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 2;
    options.Lockout.AllowedForNewUsers = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup =>
{
    var jwtSecuritySheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put **_ONLY_** yourt JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    setup.AddSecurityDefinition(jwtSecuritySheme.Reference.Id, jwtSecuritySheme);

    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecuritySheme, Array.Empty<string>() }
                });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

#region CreateRoles

using (var scoped = app.Services.CreateScope())
{
    var roleManager = scoped.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
    if (!roleManager.Roles.Any())
    {
        roleManager.CreateAsync(new AppRole()
        {
            Id = Guid.NewGuid(),
            Name = "Admin",
            NormalizedName = "ADMIN"
        }).Wait();
    }
}

#endregion

#region CreateFirstUser

using (var scoped = app.Services.CreateScope())
{
    var userManager = scoped.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    if (!userManager.Users.Any())//if there is no user in the database, create a user
    {
        userManager.CreateAsync(new()
        {
            Email = "caglasavas@gmail.com",
            UserName = "CSavas",
            FirstName = "Çaðla",
            LastName = "Savaþ",
            EmailConfirmed = true,
        }, "Password12*").Wait();
    }
}

#endregion

#region CreateUserRoles


using (var scoped = app.Services.CreateScope())
{
    var context = scoped.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var userManager = scoped.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    var roleManager = scoped.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

    AppUser? user = userManager.Users.FirstOrDefault(p => p.Email == "caglasavas@gmail.com");
    if (user is not null)
    {
        AppRole? role = roleManager.Roles.FirstOrDefault(p => p.Name == "Admin");
        if (role is not null)
        {
            bool userRoleExist = context.AppUserRoles.Any(p => p.RoleId == role.Id && p.UserId == user.Id);
            
            if (!userRoleExist)
            {
                AppUserRole appUserRole = new()
                {
                    RoleId = role.Id,
                    UserId = user.Id
                };
                context.AppUserRoles.Add(appUserRole);
                context.SaveChanges();
            }
        }
    }
}
#endregion


app.Run();
