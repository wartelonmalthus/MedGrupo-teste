using MedGrupo_teste.Domain.Enums;

namespace MedGrupo_teste.Application.DTOs;

public sealed record ContactRequest(string Name, DateOnly BirthDate, Gender Gender);
