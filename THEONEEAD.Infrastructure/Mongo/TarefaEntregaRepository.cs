using Microsoft.Extensions.Options;
using MongoDB.Driver;
using THEONEEAD.Domain.Repositories;
using THEONEEAD.Domain.Tarefas;

namespace THEONEEAD.Infrastructure.Mongo;

public class TarefaEntregaRepository : ITarefaEntregaRepository
{
    private readonly IMongoCollection<TarefaEntrega> _col;

    public TarefaEntregaRepository(IOptions<MongoDbOptions> options)
    {
        var client = new MongoClient(options.Value.ConnectionString);
        var db = client.GetDatabase(options.Value.DatabaseName);
        _col = db.GetCollection<TarefaEntrega>("tarefa_entregas");
    }

    public async Task<TarefaEntrega?> ObterAsync(long alunoLegadoId, Guid conteudoId, CancellationToken cancellationToken = default)
    {
        var id = TarefaEntrega.MontarId(alunoLegadoId, conteudoId);
        return await _col.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public Task SalvarAsync(TarefaEntrega entrega, CancellationToken cancellationToken = default) =>
        _col.ReplaceOneAsync(
            x => x.Id == entrega.Id,
            entrega,
            new ReplaceOptions { IsUpsert = true },
            cancellationToken);
}
