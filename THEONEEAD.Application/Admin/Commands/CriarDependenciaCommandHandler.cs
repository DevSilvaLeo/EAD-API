using MediatR;
using THEONEEAD.Application.Admin.Dtos;
using THEONEEAD.Domain.Entities;
using THEONEEAD.Domain.Enums;
using THEONEEAD.Domain.Repositories;

namespace THEONEEAD.Application.Admin.Commands;

public class CriarDependenciaCommandHandler : IRequestHandler<CriarDependenciaCommand, RecursoCriadoResponseDto>
{
    private readonly IDependenciaRepository _dependencias;
    private readonly IConteudoRepository _conteudos;
    private readonly IDisciplinaEADRepository _disciplinas;
    private readonly IUnitOfWorkSeminario _uow;

    public CriarDependenciaCommandHandler(
        IDependenciaRepository dependencias,
        IConteudoRepository conteudos,
        IDisciplinaEADRepository disciplinas,
        IUnitOfWorkSeminario uow)
    {
        _dependencias = dependencias;
        _conteudos = conteudos;
        _disciplinas = disciplinas;
        _uow = uow;
    }

    public async Task<RecursoCriadoResponseDto> Handle(CriarDependenciaCommand request, CancellationToken cancellationToken)
    {
        var tipo = ParseTipo(request.Tipo);

        if (await _dependencias.ExisteAsync(request.OrigemId, request.DestinoId, tipo, cancellationToken))
            throw new InvalidOperationException("Dependência já cadastrada.");

        Dependencia dep = tipo switch
        {
            TipoDependencia.Conteudo => await CriarDependenciaConteudoAsync(request, cancellationToken),
            TipoDependencia.Disciplina => await CriarDependenciaDisciplinaAsync(request, cancellationToken),
            _ => throw new ArgumentOutOfRangeException()
        };

        _dependencias.Adicionar(dep);
        await _uow.SaveChangesAsync(cancellationToken);
        return new RecursoCriadoResponseDto(dep.Id);
    }

    private async Task<Dependencia> CriarDependenciaConteudoAsync(CriarDependenciaCommand request, CancellationToken cancellationToken)
    {
        var origem = await _conteudos.ObterPorIdAsync(request.OrigemId, cancellationToken)
            ?? throw new InvalidOperationException("Conteúdo origem não encontrado.");
        var destino = await _conteudos.ObterPorIdAsync(request.DestinoId, cancellationToken)
            ?? throw new InvalidOperationException("Conteúdo destino não encontrado.");
        return Dependencia.EntreConteudos(origem.Id, destino.Id);
    }

    private async Task<Dependencia> CriarDependenciaDisciplinaAsync(CriarDependenciaCommand request, CancellationToken cancellationToken)
    {
        var origem = await _disciplinas.ObterPorIdAsync(request.OrigemId, cancellationToken)
            ?? throw new InvalidOperationException("Disciplina origem não encontrada.");
        var destino = await _disciplinas.ObterPorIdAsync(request.DestinoId, cancellationToken)
            ?? throw new InvalidOperationException("Disciplina destino não encontrada.");
        if (origem.CursoEADId != destino.CursoEADId)
            throw new InvalidOperationException("Dependência entre disciplinas deve ser no mesmo curso EAD.");
        return Dependencia.EntreDisciplinas(origem.Id, destino.Id);
    }

    private static TipoDependencia ParseTipo(string tipo)
    {
        return tipo.Trim().ToLowerInvariant() switch
        {
            "conteudo" => TipoDependencia.Conteudo,
            "disciplina" => TipoDependencia.Disciplina,
            _ => throw new ArgumentException("Tipo de dependência inválido. Use conteudo ou disciplina.")
        };
    }
}
