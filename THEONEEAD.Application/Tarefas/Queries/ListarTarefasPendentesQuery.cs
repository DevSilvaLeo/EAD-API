using MediatR;
using THEONEEAD.Application.Tarefas.Dtos;

namespace THEONEEAD.Application.Tarefas.Queries;

public record ListarTarefasPendentesQuery : IRequest<IReadOnlyList<TarefaPendenteFrontendDto>>;
