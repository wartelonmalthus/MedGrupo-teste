using MedGrupo_teste.Domain.Entities;
using MedGrupo_teste.Domain.Enums;

namespace MedGrupo_teste.Application.DTOs;

public sealed record RespostaContato(
    Guid Id,
    string Nome,
    DateOnly DataNascimento,
    Sexo Sexo,
    int Idade,
    bool EstaAtivo)
{
    public static RespostaContato DeEntidade(Contato contato)
    {
        return new RespostaContato(
            contato.Id,
            contato.Nome,
            contato.DataNascimento,
            contato.Sexo,
            contato.ObterIdade(),
            contato.EstaAtivo);
    }
}
