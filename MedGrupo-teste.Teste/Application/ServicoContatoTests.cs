using MedGrupo_teste.Application.DTOs;
using MedGrupo_teste.Application.Services;
using MedGrupo_teste.Domain.Entities;
using MedGrupo_teste.Domain.Enums;
using MedGrupo_teste.Infraestructure.Repositories;
using Xunit;

namespace MedGrupo_teste.Teste.Application;

public sealed class ServicoContatoTests
{
    [Fact]
    public async Task ObterAtivoPorIdAsync_DeveLancarExcecao_QuandoContatoNaoEncontrado()
    {
        var servico = new ServicoContato(new RepositorioContatoEmMemoria());

        var acao = async () => await servico.ObterAtivoPorIdAsync(Guid.NewGuid(), CancellationToken.None);

        await Assert.ThrowsAsync<Exception>(acao);
    }

    [Fact]
    public async Task ListarAsync_DeveRetornarSomenteContatosAtivos_PorPadrao()
    {
        var repositorio = new RepositorioContatoEmMemoria();
        var ativo = Contato.Criar("Beatriz", DateOnly.FromDateTime(DateTime.Today.AddYears(-25)), Sexo.Feminino);
        var inativo = Contato.Criar("Andre", DateOnly.FromDateTime(DateTime.Today.AddYears(-40)), Sexo.Masculino);
        inativo.Desativar();

        await repositorio.AdicionarAsync(ativo, CancellationToken.None);
        await repositorio.AdicionarAsync(inativo, CancellationToken.None);

        var servico = new ServicoContato(repositorio);

        var contatos = await servico.ListarAsync(new FiltroContato(), CancellationToken.None);

        Assert.Single(contatos.Itens);
        Assert.Equal(ativo.Id, contatos.Itens.Single().Id);
    }

    [Fact]
    public async Task ListarAsync_DeveAplicarFiltrosDinamicosEPaginacao()
    {
        var repositorio = new RepositorioContatoEmMemoria();
        var maria = Contato.Criar("Maria Silva", DateOnly.FromDateTime(DateTime.Today.AddYears(-30)), Sexo.Feminino);
        var marcos = Contato.Criar("Marcos Lima", DateOnly.FromDateTime(DateTime.Today.AddYears(-32)), Sexo.Masculino);
        var marilia = Contato.Criar("Marilia Souza", DateOnly.FromDateTime(DateTime.Today.AddYears(-28)), Sexo.Feminino);
        marilia.Desativar();

        await repositorio.AdicionarAsync(maria, CancellationToken.None);
        await repositorio.AdicionarAsync(marcos, CancellationToken.None);
        await repositorio.AdicionarAsync(marilia, CancellationToken.None);

        var servico = new ServicoContato(repositorio);
        var filtro = new FiltroContato
        {
            Nome = "Mari",
            Sexo = Sexo.Feminino,
            SomenteAtivos = false,
            Pagina = 1,
            TamanhoPagina = 1
        };

        var resultado = await servico.ListarAsync(filtro, CancellationToken.None);

        Assert.Equal(2, resultado.TotalItens);
        Assert.Equal(2, resultado.TotalPaginas);
        Assert.Single(resultado.Itens);
        Assert.Equal("Maria Silva", resultado.Itens.Single().Nome);
    }

    private sealed class RepositorioContatoEmMemoria : IRepositorioContato
    {
        private readonly List<Contato> _contatos = [];

        public Task AdicionarAsync(Contato contato, CancellationToken cancellationToken)
        {
            _contatos.Add(contato);
            return Task.CompletedTask;
        }

        public Task<Contato?> ObterAtivoPorIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return Task.FromResult(_contatos.FirstOrDefault(contato => contato.Id == id && contato.EstaAtivo));
        }

        public Task<Contato?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return Task.FromResult(_contatos.FirstOrDefault(contato => contato.Id == id));
        }

        public Task<ResultadoPaginado<Contato>> ListarAsync(FiltroContato filtro, CancellationToken cancellationToken)
        {
            var query = _contatos.AsEnumerable();

            if (filtro.SomenteAtivos)
            {
                query = query.Where(contato => contato.EstaAtivo);
            }

            if (filtro.Sexo.HasValue)
            {
                query = query.Where(contato => contato.Sexo == filtro.Sexo.Value);
            }

            if (!string.IsNullOrWhiteSpace(filtro.Nome))
            {
                query = query.Where(contato => contato.Nome.Contains(filtro.Nome, StringComparison.OrdinalIgnoreCase));
            }

            var totalItens = query.Count();
            var pagina = filtro.Pagina < 1 ? 1 : filtro.Pagina;
            var tamanhoPagina = filtro.TamanhoPagina < 1 ? 10 : filtro.TamanhoPagina;

            var itens = query
                .OrderBy(contato => contato.Nome)
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToArray();

            var totalPaginas = totalItens == 0 ? 0 : (int)Math.Ceiling(totalItens / (double)tamanhoPagina);

            return Task.FromResult(new ResultadoPaginado<Contato>(itens, pagina, tamanhoPagina, totalItens, totalPaginas));
        }

        public Task RemoverAsync(Contato contato, CancellationToken cancellationToken)
        {
            _contatos.Remove(contato);
            return Task.CompletedTask;
        }

        public Task SalvarAlteracoesAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
