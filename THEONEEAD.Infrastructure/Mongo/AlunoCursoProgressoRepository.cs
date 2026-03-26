using Microsoft.Extensions.Options;
using MongoDB.Driver;
using THEONEEAD.Domain.Progresso;
using THEONEEAD.Domain.Repositories;

namespace THEONEEAD.Infrastructure.Mongo;

public class AlunoCursoProgressoRepository : IAlunoCursoProgressoRepository
{
    private readonly IMongoCollection<AlunoCursoProgresso> _col;

    public AlunoCursoProgressoRepository(IOptions<MongoDbOptions> options)
    {
        var client = new MongoClient(options.Value.ConnectionString);
        var db = client.GetDatabase(options.Value.DatabaseName);
        _col = db.GetCollection<AlunoCursoProgresso>("progresso_aluno_curso");
    }

    public async Task<AlunoCursoProgresso?> ObterAsync(long alunoLegadoId, Guid cursoEADId, CancellationToken cancellationToken = default)
    {
        var id = AlunoCursoProgresso.MontarId(alunoLegadoId, cursoEADId);
        return await _col.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public Task SubstituirAsync(AlunoCursoProgresso documento, CancellationToken cancellationToken = default) =>
        _col.ReplaceOneAsync(
            x => x.Id == documento.Id,
            documento,
            new ReplaceOptions { IsUpsert = true },
            cancellationToken);
}
