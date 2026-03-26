namespace THEONEEAD.Domain.Estudantes;

/// <summary>Dados do cadastro acadêmico (tabela <c>estudantes</c> no MySQL).</summary>
public sealed record EstudanteDados(long Id, string EmailNormalizado, string Nome, string? CpfSomenteDigitos);
