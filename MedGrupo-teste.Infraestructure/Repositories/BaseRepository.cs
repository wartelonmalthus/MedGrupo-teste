using MedGrupo_teste.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MedGrupo_teste.Infraestructure.Repositories;

public abstract class BaseRepository<TEntity> : IBaseRepositorio<TEntity> where TEntity : class
{
    protected BaseRepository(ContextoAplicacaoDb context)
    {
        Context = context;
        _dbset = context.Set<TEntity>();
    }

    protected ContextoAplicacaoDb Context { get; }
    protected DbSet<TEntity> _dbset { get; }

    public virtual async Task AdicionarAsync(TEntity entidade, CancellationToken cancellationToken)
    {
        await _dbset.AddAsync(entidade, cancellationToken);
    }

    public virtual Task<TEntity?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return _dbset.FindAsync([id], cancellationToken).AsTask();
    }

    public virtual Task RemoverAsync(TEntity entidade, CancellationToken cancellationToken)
    {
        _dbset.Remove(entidade);
        return Task.CompletedTask;
    }

    public virtual async Task SalvarAlteracoesAsync(CancellationToken cancellationToken)
    {
        await Context.SaveChangesAsync(cancellationToken);
    }
}
