using MedGrupo_teste.Domain.Entities;
using MedGrupo_teste.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MedGrupo_teste.Infraestructure.Repositories;

public sealed class RepositorioContato : BaseRepository<Contato>, IRepositorioContato
{
    public RepositorioContato(ContextoAplicacaoDb contexto) : base(contexto)
    {
    }

    public async Task<Contato?> ObterAtivoPorIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await Context.Contatos
            .FirstOrDefaultAsync(contato => contato.Id == id && contato.EstaAtivo, cancellationToken);
    }

    public async Task<ResultadoPaginado<Contato>> ListarAsync(FiltroContato filtro, CancellationToken cancellationToken)
    {
        var pagina = filtro.Pagina < 1 ? 1 : filtro.Pagina;
        var tamanhoPagina = filtro.TamanhoPagina < 1 ? 10 : filtro.TamanhoPagina;
        tamanhoPagina = tamanhoPagina > 100 ? 100 : tamanhoPagina;

        var query = Context.Contatos.AsQueryable();

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
            var nome = filtro.Nome.Trim();
            query = query.Where(contato => EF.Functions.Like(contato.Nome, $"%{nome}%"));
        }

        var totalItens = await query.CountAsync(cancellationToken);

        var itens = await query
            .OrderBy(contato => contato.Nome)
            .Skip((pagina - 1) * tamanhoPagina)
            .Take(tamanhoPagina)
            .ToListAsync(cancellationToken);

        var totalPaginas = totalItens == 0 ? 0 : (int)Math.Ceiling(totalItens / (double)tamanhoPagina);

        return new ResultadoPaginado<Contato>(itens, pagina, tamanhoPagina, totalItens, totalPaginas);
    }
}
