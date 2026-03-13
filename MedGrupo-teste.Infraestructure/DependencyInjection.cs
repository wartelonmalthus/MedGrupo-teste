using MedGrupo_teste.Application.Abstractions;
using MedGrupo_teste.Application.Services;
using MedGrupo_teste.Infraestructure.Persistence;
using MedGrupo_teste.Infraestructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MedGrupo_teste.Infraestructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("A connection string 'DefaultConnection' nao foi configurada.");

        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
        services.AddScoped<IContactRepository, ContactRepository>();
        services.AddScoped<ContactService>();

        return services;
    }
}
