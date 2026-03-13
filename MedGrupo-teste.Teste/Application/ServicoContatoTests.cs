using MedGrupo_teste.Application.DTOs;
using MedGrupo_teste.Application.Services;
using MedGrupo_teste.Domain.Entities;
using MedGrupo_teste.Domain.Enums;
using MedGrupo_teste.Infraestructure.Repositories;
using Moq;
using Xunit;

namespace MedGrupo_teste.Teste.Application;

public sealed class ServicoContatoTests
{
    [Fact]
    public async Task ObterAtivoPorIdAsync_DeveLancarExcecao_QuandoContatoNaoEncontrado()
    {
        var repositorioMock = new Mock<IRepositorioContato>();
        repositorioMock
            .Setup(repositorio => repositorio.ObterAtivoPorIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Contato?)null);

        var servico = new ServicoContato(repositorioMock.Object);

        var acao = async () => await servico.ObterAtivoPorIdAsync(Guid.NewGuid(), CancellationToken.None);

        await Assert.ThrowsAsync<Exception>(acao);
    }

    [Fact]
    public async Task ListarAsync_DeveRetornarSomenteContatosAtivos_PorPadrao()
    {
        var ativo = Contato.Criar("Beatriz", DateOnly.FromDateTime(DateTime.Today.AddYears(-25)), Sexo.Feminino);
        var resultado = new ResultadoPaginado<Contato>([ativo], 1, 10, 1, 1);

        var repositorioMock = new Mock<IRepositorioContato>();
        repositorioMock
            .Setup(repositorio => repositorio.ListarAsync(It.IsAny<FiltroContato>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(resultado);

        var servico = new ServicoContato(repositorioMock.Object);

        var contatos = await servico.ListarAsync(new FiltroContato(), CancellationToken.None);

        Assert.Single(contatos.Itens);
        Assert.Equal(ativo.Id, contatos.Itens.Single().Id);
        repositorioMock.Verify(
            repositorio => repositorio.ListarAsync(
                It.Is<FiltroContato>(filtro => filtro.SomenteAtivos),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task ListarAsync_DeveAplicarFiltrosDinamicosEPaginacao()
    {
        var maria = Contato.Criar("Maria Silva", DateOnly.FromDateTime(DateTime.Today.AddYears(-30)), Sexo.Feminino);
        var resultado = new ResultadoPaginado<Contato>([maria], 1, 1, 2, 2);
        var filtro = new FiltroContato
        {
            Nome = "Mari",
            Sexo = Sexo.Feminino,
            SomenteAtivos = false,
            Pagina = 1,
            TamanhoPagina = 1
        };

        var repositorioMock = new Mock<IRepositorioContato>();
        repositorioMock
            .Setup(repositorio => repositorio.ListarAsync(
                It.Is<FiltroContato>(x =>
                    x.Nome == filtro.Nome &&
                    x.Sexo == filtro.Sexo &&
                    x.SomenteAtivos == filtro.SomenteAtivos &&
                    x.Pagina == filtro.Pagina &&
                    x.TamanhoPagina == filtro.TamanhoPagina),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(resultado);

        var servico = new ServicoContato(repositorioMock.Object);

        var retorno = await servico.ListarAsync(filtro, CancellationToken.None);

        Assert.Equal(2, retorno.TotalItens);
        Assert.Equal(2, retorno.TotalPaginas);
        Assert.Single(retorno.Itens);
        Assert.Equal("Maria Silva", retorno.Itens.Single().Nome);
    }

    [Fact]
    public async Task AtualizarAsync_DeveAlterarSomenteCampoInformado()
    {
        var contato = Contato.Criar("Carlos Souza", DateOnly.FromDateTime(DateTime.Today.AddYears(-20)), Sexo.Masculino);
        var repositorioMock = new Mock<IRepositorioContato>();
        repositorioMock
            .Setup(repositorio => repositorio.ObterAtivoPorIdAsync(contato.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(contato);

        var servico = new ServicoContato(repositorioMock.Object);
        var requisicao = new RequisicaoAtualizacaoContato("Carlos Silva", null, null);

        var atualizado = await servico.AtualizarAsync(contato.Id, requisicao, CancellationToken.None);

        Assert.Equal("Carlos Silva", atualizado.Nome);
        Assert.Equal(Sexo.Masculino, atualizado.Sexo);
        Assert.Equal(contato.DataNascimento, atualizado.DataNascimento);
        repositorioMock.Verify(repositorio => repositorio.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
