using MedGrupo_teste.Domain.Entities;
using MedGrupo_teste.Domain.Enums;
using Xunit;

namespace MedGrupo_teste.Teste.Domain;

public sealed class ContatoTests
{
    [Fact]
    public void Criar_DeveCalcularIdade_QuandoContatoForValido()
    {
        var dataNascimento = DateOnly.FromDateTime(DateTime.Today.AddYears(-30));

        var contato = Contato.Criar("Maria Silva", dataNascimento, Sexo.Feminino);

        Assert.Equal("Maria Silva", contato.Nome);
        Assert.True(contato.EstaAtivo);
        Assert.True(contato.ObterIdade() >= 30);
    }

    [Fact]
    public void Criar_DeveLancarExcecao_QuandoDataNascimentoForFutura()
    {
        var dataNascimentoFutura = DateOnly.FromDateTime(DateTime.Today.AddDays(1));

        var acao = () => Contato.Criar("Joao Silva", dataNascimentoFutura, Sexo.Masculino);

        var excecao = Assert.Throws<Exception>(acao);
        Assert.Equal("A data de nascimento nao pode ser maior que a data atual.", excecao.Message);
    }

    [Fact]
    public void Criar_DeveLancarExcecao_QuandoContatoForMenorDeIdade()
    {
        var dataNascimentoMenorIdade = DateOnly.FromDateTime(DateTime.Today.AddYears(-17));

        var acao = () => Contato.Criar("Ana Silva", dataNascimentoMenorIdade, Sexo.Feminino);

        var excecao = Assert.Throws<Exception>(acao);
        Assert.Equal("O contato deve ser maior de idade.", excecao.Message);
    }

    [Fact]
    public void Desativar_DeveMarcarContatoComoInativo()
    {
        var contato = Contato.Criar("Carlos Souza", DateOnly.FromDateTime(DateTime.Today.AddYears(-20)), Sexo.Masculino);

        contato.Desativar();

        Assert.False(contato.EstaAtivo);
    }
}
