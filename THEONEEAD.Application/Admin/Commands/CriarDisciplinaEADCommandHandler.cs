using MediatR;
using THEONEEAD.Application.Admin.Dtos;
using THEONEEAD.Domain.Repositories;

namespace THEONEEAD.Application.Admin.Commands;

public class CriarDisciplinaEADCommandHandler : IRequestHandler<CriarDisciplinaEADCommand, RecursoCriadoResponseDto>
{
    private readonly ICursoEADRepository _cursos;
    private readonly IUnitOfWorkSeminario _uow;

    public CriarDisciplinaEADCommandHandler(ICursoEADRepository cursos, IUnitOfWorkSeminario uow)
    {
        _cursos = cursos;
        _uow = uow;
    }

    public async Task<RecursoCriadoResponseDto> Handle(CriarDisciplinaEADCommand request, CancellationToken cancellationToken)
    {
        var curso = await _cursos.ObterPorIdComDisciplinasEConteudosAsync(request.CursoEADId, cancellationToken)
            ?? throw new InvalidOperationException("Curso EAD não encontrado.");

        var disciplina = curso.AdicionarDisciplina(request.Nome, request.Ordem);
        await _uow.SaveChangesAsync(cancellationToken);
        return new RecursoCriadoResponseDto(disciplina.Id);
    }
}
