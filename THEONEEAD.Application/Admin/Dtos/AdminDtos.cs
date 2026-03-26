namespace THEONEEAD.Application.Admin.Dtos;

public record CriarCursoEadRequestDto(long CursoLegadoId, string Nome, bool Sequencial);

public record CriarDisciplinaRequestDto(Guid CursoEADId, string Nome, int Ordem);

public record CriarConteudoRequestDto(
    Guid DisciplinaEADId,
    string Titulo,
    string Tipo,
    int Ordem,
    string? UrlVideo,
    string? UrlSlides,
    string? ConteudoTexto,
    int? DuracaoSegundos);

public record CriarDependenciaRequestDto(string Tipo, Guid OrigemId, Guid DestinoId);

public record RecursoCriadoResponseDto(Guid Id);
