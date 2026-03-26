using THEONEEAD.Application.PortalAluno;

namespace THEONEEAD.Application.Common.Interfaces;

public interface IPortalAlunoLeituraRepository
{
    Task<IReadOnlyList<FinanceiroItemDto>> ListarFinanceiroAsync(long alunoLegadoId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AvisoDto>> ListarAvisosAsync(long alunoLegadoId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<EventoCalendarioDto>> ListarEventosCalendarioAsync(long alunoLegadoId, CancellationToken cancellationToken = default);
    Task<DadosAlunoSolicitacaoDto?> ObterDadosAlunoAsync(long alunoLegadoId, string email, string nome, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TipoSolicitacaoDto>> ListarTiposSolicitacaoAsync(CancellationToken cancellationToken = default);
    Task<SolicitacaoAcademicaResponseDto> RegistrarSolicitacaoAsync(long alunoLegadoId, SolicitacaoAcademicaRequestDto request, CancellationToken cancellationToken = default);
}
