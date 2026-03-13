using MedGrupo_teste.Domain.Common;
using MedGrupo_teste.Domain.Entities;
using MedGrupo_teste.Domain.Enums;
using Xunit;

namespace MedGrupo_teste.Teste.Domain;

public sealed class ContactTests
{
    [Fact]
    public void Create_ShouldCalculateAge_WhenContactIsValid()
    {
        var birthDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-30));

        var contact = Contact.Create("Maria Silva", birthDate, Gender.Female);

        Assert.Equal("Maria Silva", contact.Name);
        Assert.True(contact.IsActive);
        Assert.True(contact.GetAge() >= 30);
    }

    [Fact]
    public void Create_ShouldThrow_WhenBirthDateIsInTheFuture()
    {
        var futureBirthDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1));

        var action = () => Contact.Create("Joao Silva", futureBirthDate, Gender.Male);

        var exception = Assert.Throws<DomainValidationException>(action);
        Assert.Equal("A data de nascimento nao pode ser maior que a data atual.", exception.Message);
    }

    [Fact]
    public void Create_ShouldThrow_WhenContactIsUnderage()
    {
        var underageBirthDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-17));

        var action = () => Contact.Create("Ana Silva", underageBirthDate, Gender.Female);

        var exception = Assert.Throws<DomainValidationException>(action);
        Assert.Equal("O contato deve ser maior de idade.", exception.Message);
    }

    [Fact]
    public void Deactivate_ShouldSetContactAsInactive()
    {
        var contact = Contact.Create("Carlos Souza", DateOnly.FromDateTime(DateTime.Today.AddYears(-20)), Gender.Male);

        contact.Deactivate();

        Assert.False(contact.IsActive);
    }
}
