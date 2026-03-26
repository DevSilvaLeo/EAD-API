using MediatR;
using THEONEEAD.Application.Admin.Dtos;

namespace THEONEEAD.Application.Admin.Commands;

public record CriarDisciplinaEADCommand(Guid CursoEADId, string Nome, int Ordem) : IRequest<RecursoCriadoResponseDto>;
