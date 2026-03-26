using MediatR;
using THEONEEAD.Application.Admin.Dtos;

namespace THEONEEAD.Application.Admin.Commands;

public record CriarDependenciaCommand(string Tipo, Guid OrigemId, Guid DestinoId) : IRequest<RecursoCriadoResponseDto>;
