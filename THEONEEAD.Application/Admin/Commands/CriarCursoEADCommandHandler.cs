using MediatR;
using THEONEEAD.Application.Admin.Dtos;
using THEONEEAD.Domain.Entities;
using THEONEEAD.Domain.Repositories;

namespace THEONEEAD.Application.Admin.Commands;

public class CriarCursoEADCommandHandler : IRequestHandler<CriarCursoEADCommand, RecursoCriadoResponseDto>
{
    private readonly ICursoEADRepository _cursos;
    private readonly IUnitOfWorkSeminario _uow;

    public CriarCursoEADCommandHandler(ICursoEADRepository cursos, IUnitOfWorkSeminario uow)
    {
        _cursos = cursos;
        _uow = uow;
    }

    public async Task<RecursoCriadoResponseDto> Handle(CriarCursoEADCommand request, CancellationToken cancellationToken)
    {
        if (await _cursos.ExistePorCursoLegadoIdAsync(request.CursoLegadoId, cancellationToken))
            throw new InvalidOperationException("Já existe um curso EAD para este curso legado.");

        var curso = CursoEAD.Criar(request.CursoLegadoId, request.Nome, request.Sequencial);
        _cursos.Adicionar(curso);
        await _uow.SaveChangesAsync(cancellationToken);
        return new RecursoCriadoResponseDto(curso.Id);
    }
}
