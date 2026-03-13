using MedGrupo_teste.Domain.Common;
using MedGrupo_teste.Domain.Enums;

namespace MedGrupo_teste.Domain.Entities;

public sealed class Contact
{
    private Contact()
    {
    }

    private Contact(Guid id, string name, DateOnly birthDate, Gender gender)
    {
        Id = id;
        SetData(name, birthDate, gender);
        IsActive = true;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public DateOnly BirthDate { get; private set; }
    public Gender Gender { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }

    public static Contact Create(string name, DateOnly birthDate, Gender gender)
    {
        return new Contact(Guid.NewGuid(), name, birthDate, gender);
    }

    public int GetAge(DateOnly? referenceDate = null)
    {
        var today = referenceDate ?? DateOnly.FromDateTime(DateTime.Today);
        var age = today.Year - BirthDate.Year;

        if (BirthDate > today.AddYears(-age))
        {
            age--;
        }

        return age;
    }

    public void Update(string name, DateOnly birthDate, Gender gender)
    {
        EnsureIsActive();
        SetData(name, birthDate, gender);
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        EnsureIsActive();
        IsActive = false;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    private void SetData(string name, DateOnly birthDate, Gender gender)
    {
        Validate(name, birthDate);
        Name = name.Trim();
        BirthDate = birthDate;
        Gender = gender;
    }

    private void EnsureIsActive()
    {
        if (!IsActive)
        {
            throw new DomainValidationException("O contato informado esta inativo.");
        }
    }

    private static void Validate(string name, DateOnly birthDate)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainValidationException("O nome do contato e obrigatorio.");
        }

        var today = DateOnly.FromDateTime(DateTime.Today);

        if (birthDate > today)
        {
            throw new DomainValidationException("A data de nascimento nao pode ser maior que a data atual.");
        }

        var age = CalculateAge(birthDate, today);

        if (age <= 0)
        {
            throw new DomainValidationException("A idade do contato deve ser maior que zero.");
        }

        if (age < 18)
        {
            throw new DomainValidationException("O contato deve ser maior de idade.");
        }
    }

    private static int CalculateAge(DateOnly birthDate, DateOnly referenceDate)
    {
        var age = referenceDate.Year - birthDate.Year;

        if (birthDate > referenceDate.AddYears(-age))
        {
            age--;
        }

        return age;
    }
}
