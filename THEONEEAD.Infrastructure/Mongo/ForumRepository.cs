using Microsoft.Extensions.Options;
using MongoDB.Driver;
using THEONEEAD.Domain.Enums;
using THEONEEAD.Domain.Forum;
using THEONEEAD.Domain.Repositories;

namespace THEONEEAD.Infrastructure.Mongo;

public class ForumRepository : IForumRepository
{
    private readonly IMongoCollection<ForumThread> _col;

    public ForumRepository(IOptions<MongoDbOptions> options)
    {
        var client = new MongoClient(options.Value.ConnectionString);
        var db = client.GetDatabase(options.Value.DatabaseName);
        _col = db.GetCollection<ForumThread>("forum_threads");
    }

    public async Task<ForumThread?> ObterPorReferenciaAsync(ForumTipoReferencia tipo, string referenciaId, CancellationToken cancellationToken = default)
    {
        var id = ForumThread.MontarId(tipo, referenciaId);
        return await _col.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public Task SalvarAsync(ForumThread thread, CancellationToken cancellationToken = default) =>
        _col.ReplaceOneAsync(
            x => x.Id == thread.Id,
            thread,
            new ReplaceOptions { IsUpsert = true },
            cancellationToken);
}
