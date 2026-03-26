namespace THEONEEAD.Infrastructure.Persistence.Seminario.Entities;

/// <summary>Tabela <c>estudantes</c> no MySQL (cadastro acadêmico).</summary>
public class Estudante
{
    public long Id { get; set; }
    public string? Cpf { get; set; }
    public string? Email { get; set; }
    public string? Nome { get; set; }
}
