using MedGrupo_teste.Domain.Enums;

namespace MedGrupo_teste.Infraestructure.Repositories;

public sealed class FiltroContato
{
    public string? Nome { get; init; }
    public Sexo? Sexo { get; init; }
    public bool SomenteAtivos { get; init; } = true;
    public int Pagina { get; init; } = 1;
    public int TamanhoPagina { get; init; } = 10;
}
