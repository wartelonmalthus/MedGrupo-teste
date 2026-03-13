using MedGrupo_teste.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedGrupo_teste.Infraestructure.Persistence;

public sealed class ContextoAplicacaoDb : DbContext
{
    public ContextoAplicacaoDb(DbContextOptions<ContextoAplicacaoDb> options) : base(options)
    {
    }

    public DbSet<Contato> Contatos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContextoAplicacaoDb).Assembly);
    }
}
