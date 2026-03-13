using MedGrupo_teste.Domain.Entities;

namespace MedGrupo_teste.Infraestructure.Repositories;

public interface IRepositorioContato : IBaseRepositorio<Contato>
{
    Task<Contato?> ObterAtivoPorIdAsync(Guid id, CancellationToken cancellationToken);
    Task<ResultadoPaginado<Contato>> ListarAsync(FiltroContato filtro, CancellationToken cancellationToken);
}
