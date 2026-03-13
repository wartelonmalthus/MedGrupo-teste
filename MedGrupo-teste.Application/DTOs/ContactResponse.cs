using MedGrupo_teste.Domain.Entities;
using MedGrupo_teste.Domain.Enums;

namespace MedGrupo_teste.Application.DTOs;

public sealed record ContactResponse(
    Guid Id,
    string Name,
    DateOnly BirthDate,
    Gender Gender,
    int Age,
    bool IsActive)
{
    public static ContactResponse FromEntity(Contact contact)
    {
        return new ContactResponse(
            contact.Id,
            contact.Name,
            contact.BirthDate,
            contact.Gender,
            contact.GetAge(),
            contact.IsActive);
    }
}
