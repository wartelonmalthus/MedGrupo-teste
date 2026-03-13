using MedGrupo_teste.Domain.Enums;

namespace MedGrupo_teste.Application.DTOs;

public sealed record RequisicaoAtualizacaoContato(string? Nome, DateOnly? DataNascimento, Sexo? Sexo);
