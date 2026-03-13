namespace MedGrupo_teste.Infraestructure.Repositories;

public interface IBaseRepositorio<TEntity> where TEntity : class
{
    Task AdicionarAsync(TEntity entidade, CancellationToken cancellationToken);
    Task<TEntity?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken);
    Task RemoverAsync(TEntity entidade, CancellationToken cancellationToken);
    Task SalvarAlteracoesAsync(CancellationToken cancellationToken);
}
