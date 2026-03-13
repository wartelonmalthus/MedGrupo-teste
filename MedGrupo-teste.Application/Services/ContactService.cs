using MedGrupo_teste.Application.Abstractions;
using MedGrupo_teste.Application.Common;
using MedGrupo_teste.Application.DTOs;
using MedGrupo_teste.Domain.Entities;

namespace MedGrupo_teste.Application.Services;

public sealed class ContactService
{
    private readonly IContactRepository _contactRepository;

    public ContactService(IContactRepository contactRepository)
    {
        _contactRepository = contactRepository;
    }

    public async Task<ContactResponse> CreateAsync(ContactRequest request, CancellationToken cancellationToken)
    {
        var contact = Contact.Create(request.Name, request.BirthDate, request.Gender);
        await _contactRepository.AddAsync(contact, cancellationToken);
        await _contactRepository.SaveChangesAsync(cancellationToken);

        return ContactResponse.FromEntity(contact);
    }

    public async Task<IReadOnlyCollection<ContactResponse>> ListActiveAsync(CancellationToken cancellationToken)
    {
        var contacts = await _contactRepository.ListActiveAsync(cancellationToken);

        return contacts
            .Select(ContactResponse.FromEntity)
            .OrderBy(contact => contact.Name)
            .ToArray();
    }

    public async Task<ContactResponse> GetActiveByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var contact = await _contactRepository.GetActiveByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Contato ativo nao encontrado.");

        return ContactResponse.FromEntity(contact);
    }

    public async Task<ContactResponse> UpdateAsync(Guid id, ContactRequest request, CancellationToken cancellationToken)
    {
        var contact = await _contactRepository.GetActiveByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Contato ativo nao encontrado.");

        contact.Update(request.Name, request.BirthDate, request.Gender);
        await _contactRepository.SaveChangesAsync(cancellationToken);

        return ContactResponse.FromEntity(contact);
    }

    public async Task DeactivateAsync(Guid id, CancellationToken cancellationToken)
    {
        var contact = await _contactRepository.GetActiveByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Contato ativo nao encontrado.");

        contact.Deactivate();
        await _contactRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var contact = await _contactRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Contato nao encontrado.");

        await _contactRepository.RemoveAsync(contact, cancellationToken);
        await _contactRepository.SaveChangesAsync(cancellationToken);
    }
}
