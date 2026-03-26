using THEONEEAD.Domain.Enums;
using THEONEEAD.Domain.Forum;

namespace THEONEEAD.Domain.Repositories;

public interface IForumRepository
{
    Task<ForumThread?> ObterPorReferenciaAsync(ForumTipoReferencia tipo, string referenciaId, CancellationToken cancellationToken = default);
    Task SalvarAsync(ForumThread thread, CancellationToken cancellationToken = default);
}
