using MedGrupo_teste.Infraestructure.Persistence;
using MedGrupo_teste.Infraestructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MedGrupo_teste.Infraestructure;

public static class DependencyInjection
{
    public static IServiceCollection AdicionarInfraestrutura(this IServiceCollection services, IConfiguration configuration)
    {
        var stringConexao = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("A connection string 'DefaultConnection' nao foi configurada.");

        services.AddDbContext<ContextoAplicacaoDb>(options => options.UseSqlServer(stringConexao));
        services.AddScoped<IRepositorioContato, RepositorioContato>();

        return services;
    }
}
