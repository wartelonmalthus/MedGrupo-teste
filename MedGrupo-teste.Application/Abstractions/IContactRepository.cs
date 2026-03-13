using MedGrupo_teste.Domain.Entities;

namespace MedGrupo_teste.Application.Abstractions;

public interface IContactRepository
{
    Task AddAsync(Contact contact, CancellationToken cancellationToken);
    Task<Contact?> GetActiveByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Contact?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<Contact>> ListActiveAsync(CancellationToken cancellationToken);
    Task RemoveAsync(Contact contact, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
