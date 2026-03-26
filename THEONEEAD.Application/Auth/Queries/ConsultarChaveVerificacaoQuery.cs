using MediatR;
using THEONEEAD.Application.Common.Interfaces;

namespace THEONEEAD.Application.Auth.Queries;

public record ConsultarChaveVerificacaoQuery(string Chave) : IRequest<bool>;

public class ConsultarChaveVerificacaoQueryHandler : IRequestHandler<ConsultarChaveVerificacaoQuery, bool>
{
    private readonly ICodigoVerificacaoStore _store;

    public ConsultarChaveVerificacaoQueryHandler(ICodigoVerificacaoStore store) => _store = store;

    public Task<bool> Handle(ConsultarChaveVerificacaoQuery request, CancellationToken cancellationToken) =>
        Task.FromResult(_store.ChaveExisteENaoExpirou(request.Chave));
}
