using THEONEEAD.Domain.Entities;
using THEONEEAD.Domain.Enums;
using THEONEEAD.Domain.Progresso;

namespace THEONEEAD.Application.Cursos;

public static class ProgressoCursoHelper
{
    public static AlunoCursoProgresso GarantirDocumento(long alunoLegadoId, CursoEAD curso)
    {
        var id = AlunoCursoProgresso.MontarId(alunoLegadoId, curso.Id);
        var doc = new AlunoCursoProgresso
        {
            Id = id,
            AlunoId = alunoLegadoId,
            CursoId = curso.Id,
            Disciplinas = curso.Disciplinas.OrderBy(d => d.Ordem).Select(d => new ProgressoDisciplinaAluno
            {
                DisciplinaId = d.Id,
                Percentual = 0,
                Conteudos = d.Conteudos.OrderBy(c => c.Ordem).Select(c => new ProgressoConteudoAluno
                {
                    ConteudoId = c.Id,
                    Concluido = false,
                    DataConclusao = null
                }).ToList()
            }).ToList()
        };
        return doc;
    }

    public static void SincronizarEstrutura(AlunoCursoProgresso doc, CursoEAD curso)
    {
        foreach (var disc in curso.Disciplinas.OrderBy(d => d.Ordem))
        {
            var pd = doc.Disciplinas.FirstOrDefault(x => x.DisciplinaId == disc.Id);
            if (pd is null)
            {
                pd = new ProgressoDisciplinaAluno { DisciplinaId = disc.Id, Percentual = 0, Conteudos = new List<ProgressoConteudoAluno>() };
                doc.Disciplinas.Add(pd);
            }

            foreach (var cont in disc.Conteudos.OrderBy(c => c.Ordem))
            {
                if (pd.Conteudos.All(c => c.ConteudoId != cont.Id))
                    pd.Conteudos.Add(new ProgressoConteudoAluno { ConteudoId = cont.Id, Concluido = false });
            }

            pd.Conteudos.RemoveAll(c => disc.Conteudos.All(x => x.Id != c.ConteudoId));
            RecalcularPercentualDisciplina(pd, disc);
        }

        doc.Disciplinas.RemoveAll(d => curso.Disciplinas.All(x => x.Id != d.DisciplinaId));
    }

    public static void RecalcularPercentualDisciplina(ProgressoDisciplinaAluno pd, DisciplinaEAD disc)
    {
        var total = disc.Conteudos.Count;
        if (total == 0)
        {
            pd.Percentual = 100;
            return;
        }

        var concluidos = pd.Conteudos.Count(c => c.Concluido && disc.Conteudos.Any(x => x.Id == c.ConteudoId));
        pd.Percentual = Math.Round(100m * concluidos / total, 2);
    }

    public static decimal PercentualCurso(CursoEAD curso, AlunoCursoProgresso doc)
    {
        var disciplinas = curso.Disciplinas.ToList();
        if (disciplinas.Count == 0)
            return 0;

        var soma = disciplinas.Sum(d =>
        {
            var pd = doc.Disciplinas.FirstOrDefault(x => x.DisciplinaId == d.Id);
            return pd?.Percentual ?? 0;
        });

        return Math.Round(soma / disciplinas.Count, 2);
    }

    public static HashSet<Guid> ConteudosConcluidos(AlunoCursoProgresso doc) =>
        doc.Disciplinas.SelectMany(d => d.Conteudos).Where(c => c.Concluido).Select(c => c.ConteudoId).ToHashSet();

    public static HashSet<Guid> DisciplinasConcluidas(CursoEAD curso, AlunoCursoProgresso doc)
    {
        var set = new HashSet<Guid>();
        foreach (var d in curso.Disciplinas)
        {
            var pd = doc.Disciplinas.FirstOrDefault(x => x.DisciplinaId == d.Id);
            if (pd is null)
                continue;
            var total = d.Conteudos.Count;
            if (total == 0 && pd.Percentual >= 100)
                set.Add(d.Id);
            else if (total > 0 && pd.Conteudos.Count(c => c.Concluido) >= total)
                set.Add(d.Id);
        }

        return set;
    }

    public static string TipoConteudoApi(TipoConteudo t) => t switch
    {
        TipoConteudo.Video => "video",
        TipoConteudo.Slide => "slide",
        TipoConteudo.Texto => "texto",
        TipoConteudo.Tarefa => "tarefa",
        _ => "texto"
    };
}
