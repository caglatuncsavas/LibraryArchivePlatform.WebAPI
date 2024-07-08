using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;


namespace LibraryArchive.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        

        services.AddMediatR(conf =>
        {
            conf.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly);
        });

        services.AddFluentValidationAutoValidation().AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);


        return services;
    }
}
