namespace THEONEEAD.Application.PortalAluno;

public record FinanceiroItemDto(string Id, string CursoId, string NomeCurso, string Status);

public record AvisoDto(string Id, string Titulo, string Mensagem, string Data, bool Lido);

public record TarefaPendenteListaDto(string Id, string Titulo, string Descricao, string DataLimite, bool Concluida);

public record EventoCalendarioDto(string Id, string Title, string Start, string? End, string? BackgroundColor);

public record DadosAlunoSolicitacaoDto(string Nome, string Email, string DataSolicitacao);

public record TipoSolicitacaoDto(string Id, string Descricao);

public record SolicitacaoAcademicaRequestDto(string TipoSolicitacaoId, string Descricao);

public record SolicitacaoAcademicaResponseDto(string Id, string Mensagem);
