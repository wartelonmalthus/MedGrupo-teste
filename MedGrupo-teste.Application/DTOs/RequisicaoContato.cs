using MedGrupo_teste.Domain.Enums;

namespace MedGrupo_teste.Application.DTOs;

public sealed record RequisicaoContato(string Nome, DateOnly DataNascimento, Sexo Sexo);
