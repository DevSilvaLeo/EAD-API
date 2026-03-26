using MediatR;
using THEONEEAD.Application.Admin.Dtos;

namespace THEONEEAD.Application.Admin.Commands;

public record CriarCursoEADCommand(long CursoLegadoId, string Nome, bool Sequencial) : IRequest<RecursoCriadoResponseDto>;
