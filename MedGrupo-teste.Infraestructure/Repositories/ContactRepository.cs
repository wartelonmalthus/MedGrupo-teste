using MedGrupo_teste.Application.Abstractions;
using MedGrupo_teste.Domain.Entities;
using MedGrupo_teste.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MedGrupo_teste.Infraestructure.Repositories;

public sealed class ContactRepository : IContactRepository
{
    private readonly AppDbContext _context;

    public ContactRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Contact contact, CancellationToken cancellationToken)
    {
        await _context.Contacts.AddAsync(contact, cancellationToken);
    }

    public async Task<Contact?> GetActiveByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Contacts
            .FirstOrDefaultAsync(contact => contact.Id == id && contact.IsActive, cancellationToken);
    }

    public async Task<Contact?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Contacts
            .FirstOrDefaultAsync(contact => contact.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyCollection<Contact>> ListActiveAsync(CancellationToken cancellationToken)
    {
        return await _context.Contacts
            .Where(contact => contact.IsActive)
            .OrderBy(contact => contact.Name)
            .ToListAsync(cancellationToken);
    }

    public Task RemoveAsync(Contact contact, CancellationToken cancellationToken)
    {
        _context.Contacts.Remove(contact);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
