using MedGrupo_teste.Domain.Enums;

namespace MedGrupo_teste.Domain.Entities;

public class Contato
{
    private Contato()
    {
    }

    public Contato(Guid id, string nome, DateOnly dataNascimento, Sexo sexo)
    {
        Id = id;
        DefinirDados(nome, dataNascimento, sexo);
        EstaAtivo = true;
        CriadoEmUtc = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public DateOnly DataNascimento { get; private set; }
    public Sexo Sexo { get; private set; }
    public bool EstaAtivo { get; private set; }
    public DateTime CriadoEmUtc { get; private set; }
    public DateTime? AtualizadoEmUtc { get; private set; }

    public static Contato Criar(string nome, DateOnly dataNascimento, Sexo sexo)
    {
        return new Contato(Guid.NewGuid(), nome, dataNascimento, sexo);
    }

    public int ObterIdade(DateOnly? dataReferencia = null)
    {
        var hoje = dataReferencia ?? DateOnly.FromDateTime(DateTime.Today);
        var idade = hoje.Year - DataNascimento.Year;

        if (DataNascimento > hoje.AddYears(-idade))
        {
            idade--;
        }

        return idade;
    }

    public void Atualizar(string nome, DateOnly dataNascimento, Sexo sexo)
    {
        GarantirQueEstaAtivo();
        DefinirDados(nome, dataNascimento, sexo);
        AtualizadoEmUtc = DateTime.UtcNow;
    }

    public void Desativar()
    {
        GarantirQueEstaAtivo();
        EstaAtivo = false;
        AtualizadoEmUtc = DateTime.UtcNow;
    }

    private void DefinirDados(string nome, DateOnly dataNascimento, Sexo sexo)
    {
        Validar(nome, dataNascimento);
        Nome = nome.Trim();
        DataNascimento = dataNascimento;
        Sexo = sexo;
    }

    private void GarantirQueEstaAtivo()
    {
        if (!EstaAtivo)
        {
            throw new Exception("O contato informado esta inativo.");
        }
    }

    private static void Validar(string nome, DateOnly dataNascimento)
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            throw new Exception("O nome do contato e obrigatorio.");
        }

        var hoje = DateOnly.FromDateTime(DateTime.Today);

        if (dataNascimento > hoje)
        {
            throw new Exception("A data de nascimento nao pode ser maior que a data atual.");
        }

        var idade = CalcularIdade(dataNascimento, hoje);

        if (idade <= 0)
        {
            throw new Exception("A idade do contato deve ser maior que zero.");
        }

        if (idade < 18)
        {
            throw new Exception("O contato deve ser maior de idade.");
        }
    }

    private static int CalcularIdade(DateOnly dataNascimento, DateOnly dataReferencia)
    {
        var idade = dataReferencia.Year - dataNascimento.Year;

        if (dataNascimento > dataReferencia.AddYears(-idade))
        {
            idade--;
        }

        return idade;
    }
}
