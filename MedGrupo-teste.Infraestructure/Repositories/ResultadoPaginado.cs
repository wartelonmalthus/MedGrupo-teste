namespace MedGrupo_teste.Infraestructure.Repositories;

public sealed record ResultadoPaginado<T>(
    IReadOnlyCollection<T> Itens,
    int Pagina,
    int TamanhoPagina,
    int TotalItens,
    int TotalPaginas);
