using MedGrupo_teste.Application.Abstractions;
using MedGrupo_teste.Application.Common;
using MedGrupo_teste.Application.Services;
using MedGrupo_teste.Domain.Entities;
using MedGrupo_teste.Domain.Enums;
using Xunit;

namespace MedGrupo_teste.Teste.Application;

public sealed class ContactServiceTests
{
    [Fact]
    public async Task GetActiveByIdAsync_ShouldThrow_WhenContactIsNotFound()
    {
        var service = new ContactService(new InMemoryContactRepository());

        var action = async () => await service.GetActiveByIdAsync(Guid.NewGuid(), CancellationToken.None);

        await Assert.ThrowsAsync<NotFoundException>(action);
    }

    [Fact]
    public async Task ListActiveAsync_ShouldReturnOnlyActiveContacts()
    {
        var repository = new InMemoryContactRepository();
        var active = Contact.Create("Beatriz", DateOnly.FromDateTime(DateTime.Today.AddYears(-25)), Gender.Female);
        var inactive = Contact.Create("André", DateOnly.FromDateTime(DateTime.Today.AddYears(-40)), Gender.Male);
        inactive.Deactivate();

        await repository.AddAsync(active, CancellationToken.None);
        await repository.AddAsync(inactive, CancellationToken.None);

        var service = new ContactService(repository);

        var contacts = await service.ListActiveAsync(CancellationToken.None);

        Assert.Single(contacts);
        Assert.Equal(active.Id, contacts.Single().Id);
    }

    private sealed class InMemoryContactRepository : IContactRepository
    {
        private readonly List<Contact> _contacts = [];

        public Task AddAsync(Contact contact, CancellationToken cancellationToken)
        {
            _contacts.Add(contact);
            return Task.CompletedTask;
        }

        public Task<Contact?> GetActiveByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return Task.FromResult(_contacts.FirstOrDefault(contact => contact.Id == id && contact.IsActive));
        }

        public Task<Contact?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return Task.FromResult(_contacts.FirstOrDefault(contact => contact.Id == id));
        }

        public Task<IReadOnlyCollection<Contact>> ListActiveAsync(CancellationToken cancellationToken)
        {
            IReadOnlyCollection<Contact> contacts = _contacts.Where(contact => contact.IsActive).ToArray();
            return Task.FromResult(contacts);
        }

        public Task RemoveAsync(Contact contact, CancellationToken cancellationToken)
        {
            _contacts.Remove(contact);
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
