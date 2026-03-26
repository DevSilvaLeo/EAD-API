using THEONEEAD.Domain.Tarefas;

namespace THEONEEAD.Domain.Repositories;

public interface ITarefaEntregaRepository
{
    Task<TarefaEntrega?> ObterAsync(long alunoLegadoId, Guid conteudoId, CancellationToken cancellationToken = default);
    Task SalvarAsync(TarefaEntrega entrega, CancellationToken cancellationToken = default);
}
