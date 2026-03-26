using Microsoft.Extensions.Options;
using MongoDB.Driver;
using THEONEEAD.Application.Common.Interfaces;
using THEONEEAD.Application.PortalAluno;

namespace THEONEEAD.Infrastructure.Mongo;

public class PortalAlunoMongoRepository : IPortalAlunoLeituraRepository
{
    private readonly IMongoDatabase _db;

    public PortalAlunoMongoRepository(IOptions<MongoDbOptions> options)
    {
        var client = new MongoClient(options.Value.ConnectionString);
        _db = client.GetDatabase(options.Value.DatabaseName);
    }

    public async Task<IReadOnlyList<FinanceiroItemDto>> ListarFinanceiroAsync(long alunoLegadoId, CancellationToken cancellationToken = default)
    {
        await GarantirSeedAsync(alunoLegadoId, cancellationToken);
        var col = _db.GetCollection<FinanceiroItemDoc>("financeiro_itens");
        var list = await col.Find(x => x.AlunoLegadoId == alunoLegadoId).ToListAsync(cancellationToken);
        return list.Select(x => new FinanceiroItemDto(x.Id, x.CursoId, x.NomeCurso, x.Status)).ToList();
    }

    public async Task<IReadOnlyList<AvisoDto>> ListarAvisosAsync(long alunoLegadoId, CancellationToken cancellationToken = default)
    {
        await GarantirSeedAsync(alunoLegadoId, cancellationToken);
        var col = _db.GetCollection<AvisoDoc>("avisos");
        var list = await col.Find(x => x.AlunoLegadoId == alunoLegadoId).SortByDescending(x => x.Data).ToListAsync(cancellationToken);
        return list.Select(x => new AvisoDto(x.Id, x.Titulo, x.Mensagem, x.Data, x.Lido)).ToList();
    }

    public async Task<IReadOnlyList<EventoCalendarioDto>> ListarEventosCalendarioAsync(long alunoLegadoId, CancellationToken cancellationToken = default)
    {
        await GarantirSeedAsync(alunoLegadoId, cancellationToken);
        var col = _db.GetCollection<EventoCalendarioDoc>("calendario_eventos");
        var list = await col.Find(x => x.AlunoLegadoId == alunoLegadoId).ToListAsync(cancellationToken);
        return list.Select(x => new EventoCalendarioDto(x.Id, x.Title, x.Start, x.End, x.BackgroundColor)).ToList();
    }

    public Task<DadosAlunoSolicitacaoDto?> ObterDadosAlunoAsync(long alunoLegadoId, string email, string nome, CancellationToken cancellationToken = default) =>
        Task.FromResult<DadosAlunoSolicitacaoDto?>(new DadosAlunoSolicitacaoDto(
            string.IsNullOrWhiteSpace(nome) ? "Aluno" : nome,
            string.IsNullOrWhiteSpace(email) ? "aluno@ead.com" : email,
            DateTime.UtcNow.ToString("yyyy-MM-dd")));

    public async Task<IReadOnlyList<TipoSolicitacaoDto>> ListarTiposSolicitacaoAsync(CancellationToken cancellationToken = default)
    {
        await GarantirTiposAsync(cancellationToken);
        var col = _db.GetCollection<TipoSolicitacaoDoc>("tipos_solicitacao");
        var list = await col.Find(_ => true).ToListAsync(cancellationToken);
        return list.Select(x => new TipoSolicitacaoDto(x.Id, x.Descricao)).ToList();
    }

    public async Task<SolicitacaoAcademicaResponseDto> RegistrarSolicitacaoAsync(long alunoLegadoId, SolicitacaoAcademicaRequestDto request, CancellationToken cancellationToken = default)
    {
        var col = _db.GetCollection<SolicitacaoDoc>("solicitacoes_academicas");
        var id = Guid.NewGuid().ToString("N");
        await col.InsertOneAsync(
            new SolicitacaoDoc
            {
                Id = id,
                AlunoLegadoId = alunoLegadoId,
                TipoSolicitacaoId = request.TipoSolicitacaoId,
                Descricao = request.Descricao,
                CriadoEm = DateTime.UtcNow
            },
            cancellationToken: cancellationToken);
        return new SolicitacaoAcademicaResponseDto(
            id,
            "Solicitação registrada com sucesso. Você receberá um retorno em até 5 dias úteis.");
    }

    private async Task GarantirTiposAsync(CancellationToken ct)
    {
        var col = _db.GetCollection<TipoSolicitacaoDoc>("tipos_solicitacao");
        if (await col.EstimatedDocumentCountAsync(cancellationToken: ct) > 0)
            return;
        await col.InsertManyAsync(
            new[]
            {
                new TipoSolicitacaoDoc { Id = "1", Descricao = "Declaração de matrícula" },
                new TipoSolicitacaoDoc { Id = "2", Descricao = "Histórico escolar" },
                new TipoSolicitacaoDoc { Id = "3", Descricao = "Atestado de conclusão" },
                new TipoSolicitacaoDoc { Id = "4", Descricao = "Outros" }
            },
            cancellationToken: ct);
    }

    private async Task GarantirSeedAsync(long alunoLegadoId, CancellationToken ct)
    {
        var fin = _db.GetCollection<FinanceiroItemDoc>("financeiro_itens");
        if (await fin.CountDocumentsAsync(x => x.AlunoLegadoId == alunoLegadoId, cancellationToken: ct) > 0)
            return;

        // Demo apenas para aluno 1 (seed SQL)
        if (alunoLegadoId != 1)
            return;

        await fin.InsertManyAsync(
            new[]
            {
                new FinanceiroItemDoc { Id = "1", AlunoLegadoId = 1, CursoId = "demo", NomeCurso = "Introdução ao EAD", Status = "pago" },
                new FinanceiroItemDoc { Id = "2", AlunoLegadoId = 1, CursoId = "demo2", NomeCurso = "Gestão de Projetos", Status = "pendente" },
                new FinanceiroItemDoc { Id = "3", AlunoLegadoId = 1, CursoId = "demo3", NomeCurso = "Comunicação Empresarial", Status = "atrasado" }
            },
            cancellationToken: ct);

        var avisos = _db.GetCollection<AvisoDoc>("avisos");
        var hoje = DateTime.UtcNow.ToString("yyyy-MM-dd");
        await avisos.InsertManyAsync(
            new[]
            {
                new AvisoDoc { Id = "1", AlunoLegadoId = 1, Titulo = "Novo material disponível", Mensagem = "Novo conteúdo disponível no portal.", Data = hoje, Lido = false },
                new AvisoDoc { Id = "2", AlunoLegadoId = 1, Titulo = "Prazo de entrega", Mensagem = "Lembrete: verifique suas tarefas pendentes.", Data = hoje, Lido = true }
            },
            cancellationToken: ct);

        var agora = DateTime.UtcNow;
        var cal = _db.GetCollection<EventoCalendarioDoc>("calendario_eventos");
        await cal.InsertManyAsync(
            new[]
            {
                new EventoCalendarioDoc
                {
                    Id = "1",
                    AlunoLegadoId = 1,
                    Title = "Aula ao vivo - Gestão de Projetos",
                    Start = new DateTime(agora.Year, agora.Month, agora.Day, 14, 0, 0, DateTimeKind.Utc).ToString("O"),
                    End = new DateTime(agora.Year, agora.Month, agora.Day, 16, 0, 0, DateTimeKind.Utc).ToString("O"),
                    BackgroundColor = "#1565c0"
                },
                new EventoCalendarioDoc
                {
                    Id = "2",
                    AlunoLegadoId = 1,
                    Title = "Prazo: entrega de atividade",
                    Start = agora.AddDays(5).ToString("yyyy-MM-dd"),
                    End = null,
                    BackgroundColor = "#c62828"
                }
            },
            cancellationToken: ct);
    }

    private sealed class FinanceiroItemDoc
    {
        public string Id { get; set; } = null!;
        public long AlunoLegadoId { get; set; }
        public string CursoId { get; set; } = null!;
        public string NomeCurso { get; set; } = null!;
        public string Status { get; set; } = null!;
    }

    private sealed class AvisoDoc
    {
        public string Id { get; set; } = null!;
        public long AlunoLegadoId { get; set; }
        public string Titulo { get; set; } = null!;
        public string Mensagem { get; set; } = null!;
        public string Data { get; set; } = null!;
        public bool Lido { get; set; }
    }

    private sealed class EventoCalendarioDoc
    {
        public string Id { get; set; } = null!;
        public long AlunoLegadoId { get; set; }
        public string Title { get; set; } = null!;
        public string Start { get; set; } = null!;
        public string? End { get; set; }
        public string? BackgroundColor { get; set; }
    }

    private sealed class TipoSolicitacaoDoc
    {
        public string Id { get; set; } = null!;
        public string Descricao { get; set; } = null!;
    }

    private sealed class SolicitacaoDoc
    {
        public string Id { get; set; } = null!;
        public long AlunoLegadoId { get; set; }
        public string TipoSolicitacaoId { get; set; } = null!;
        public string Descricao { get; set; } = null!;
        public DateTime CriadoEm { get; set; }
    }
}
