using MediatR;
using THEONEEAD.Application.Admin.Dtos;

namespace THEONEEAD.Application.Admin.Commands;

public record CriarConteudoCommand(
    Guid DisciplinaEADId,
    string Titulo,
    string Tipo,
    int Ordem,
    string? UrlVideo,
    string? UrlSlides,
    string? ConteudoTexto,
    int? DuracaoSegundos) : IRequest<RecursoCriadoResponseDto>;
