namespace THEONEEAD.Application.Tarefas.Dtos;

/// <summary>Contrato GET /api/tarefas/pendentes (Angular).</summary>
public record TarefaPendenteFrontendDto(string Id, string Titulo, string Descricao, string DataLimite, bool Concluida);

public record EntregarTarefaRequestDto(string? Texto, string? UrlArquivo);
