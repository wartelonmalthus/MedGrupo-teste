using MedGrupo_teste.Application.DTOs;
using MedGrupo_teste.Domain.Entities;
using MedGrupo_teste.Infraestructure.Repositories;

namespace MedGrupo_teste.Application.Services;

public sealed class ServicoContato
{
    private readonly IRepositorioContato _repositorioContato;

    public ServicoContato(IRepositorioContato repositorioContato)
    {
        _repositorioContato = repositorioContato;
    }

    public async Task<RespostaContato> CriarAsync(RequisicaoContato requisicao, CancellationToken cancellationToken)
    {
        var contato = Contato.Criar(requisicao.Nome, requisicao.DataNascimento, requisicao.Sexo);
        await _repositorioContato.AdicionarAsync(contato, cancellationToken);
        await _repositorioContato.SalvarAlteracoesAsync(cancellationToken);

        return RespostaContato.DeEntidade(contato);
    }

    public async Task<ResultadoPaginado<RespostaContato>> ListarAsync(FiltroContato filtro, CancellationToken cancellationToken)
    {
        var resultado = await _repositorioContato.ListarAsync(filtro, cancellationToken);

        return new ResultadoPaginado<RespostaContato>(
            resultado.Itens.Select(RespostaContato.DeEntidade).ToArray(),
            resultado.Pagina,
            resultado.TamanhoPagina,
            resultado.TotalItens,
            resultado.TotalPaginas);
    }

    public async Task<RespostaContato> ObterAtivoPorIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var contato = await _repositorioContato.ObterAtivoPorIdAsync(id, cancellationToken)
            ?? throw new Exception("Contato ativo nao encontrado.");

        return RespostaContato.DeEntidade(contato);
    }

    public async Task<RespostaContato> AtualizarAsync(Guid id, RequisicaoAtualizacaoContato requisicao, CancellationToken cancellationToken)
    {
        var contato = await _repositorioContato.ObterAtivoPorIdAsync(id, cancellationToken)
            ?? throw new Exception("Contato ativo nao encontrado.");

        contato.Atualizar(requisicao.Nome, requisicao.DataNascimento, requisicao.Sexo);
        await _repositorioContato.SalvarAlteracoesAsync(cancellationToken);

        return RespostaContato.DeEntidade(contato);
    }

    public async Task DesativarAsync(Guid id, CancellationToken cancellationToken)
    {
        var contato = await _repositorioContato.ObterAtivoPorIdAsync(id, cancellationToken)
            ?? throw new Exception("Contato ativo nao encontrado.");

        contato.Desativar();
        await _repositorioContato.SalvarAlteracoesAsync(cancellationToken);
    }

    public async Task ExcluirAsync(Guid id, CancellationToken cancellationToken)
    {
        var contato = await _repositorioContato.ObterPorIdAsync(id, cancellationToken)
            ?? throw new Exception("Contato nao encontrado.");

        await _repositorioContato.RemoverAsync(contato, cancellationToken);
        await _repositorioContato.SalvarAlteracoesAsync(cancellationToken);
    }
}
