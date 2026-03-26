namespace THEONEEAD.Application.Forum.Dtos;

public record ForumRespostaDto(string Id, string AlunoId, string Mensagem, DateTime CriadoEm);

public record ForumEntradaDto(string Id, string AlunoId, string Mensagem, DateTime CriadoEm, IReadOnlyList<ForumRespostaDto> Respostas);

public record ForumThreadResponseDto(
    string Id,
    string Tipo,
    string ReferenciaId,
    IReadOnlyList<ForumEntradaDto> Entradas);

public record ForumPostRequestDto(string Mensagem, string? ParentEntradaId = null);
