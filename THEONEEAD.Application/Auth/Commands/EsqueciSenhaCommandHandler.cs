using MediatR;
using THEONEEAD.Application.Auth;
using THEONEEAD.Application.Auth.Dtos;
using THEONEEAD.Application.Common.Interfaces;
using THEONEEAD.Domain.Entities;
using THEONEEAD.Domain.Repositories;

namespace THEONEEAD.Application.Auth.Commands;

public class EsqueciSenhaCommandHandler : IRequestHandler<EsqueciSenhaCommand, EsqueciSenhaResponseDto>
{
    private readonly IUsuarioRepository _usuarios;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ICodigoAcessoGerador _gerador;
    private readonly IRecuperacaoSenhaNotificador _notificador;
    private readonly ICodigoVerificacaoStore _store;
    private static readonly TimeSpan Validade = TimeSpan.FromHours(1);

    public EsqueciSenhaCommandHandler(
        IUsuarioRepository usuarios,
        IPasswordHasher passwordHasher,
        ICodigoAcessoGerador gerador,
        IRecuperacaoSenhaNotificador notificador,
        ICodigoVerificacaoStore store)
    {
        _usuarios = usuarios;
        _passwordHasher = passwordHasher;
        _gerador = gerador;
        _notificador = notificador;
        _store = store;
    }

    public async Task<EsqueciSenhaResponseDto> Handle(EsqueciSenhaCommand request, CancellationToken cancellationToken)
    {
        const string msgGenerica = "Se os dados estiverem cadastrados, você receberá um código de verificação no e-mail cadastrado.";

        Usuario? usuario = null;
        var emailBusca = request.Email?.Trim();
        if (!string.IsNullOrWhiteSpace(emailBusca))
            usuario = await _usuarios.ObterPorEmailAsync(emailBusca, cancellationToken);

        if (usuario is null)
        {
            var cpfN = CpfUtil.Normalizar(request.Cpf);
            if (!string.IsNullOrEmpty(cpfN))
                usuario = await _usuarios.ObterPorCpfNormalizadoAsync(cpfN, cancellationToken);
        }

        if (usuario is null)
            return new EsqueciSenhaResponseDto(msgGenerica, null);

        if (usuario.PrimeiroAcessoPendente)
            return new EsqueciSenhaResponseDto(
                "Este cadastro ainda não foi ativado. Utilize o fluxo de primeiro acesso.",
                null);

        var codigo = _gerador.GerarCodigoNumerico();
        var hash = _passwordHasher.Hash(codigo);
        var expira = DateTime.UtcNow.Add(Validade);
        var chave = _store.Registrar(usuario.Id, hash, expira, AuthFluxosCodigo.RecuperacaoSenha);
        await _notificador.NotificarCodigoAsync(
            usuario.Email,
            codigo,
            AuthEmailAssuntos.RecuperacaoSenha,
            expira,
            cancellationToken);

        return new EsqueciSenhaResponseDto(
            "Código de verificação enviado para o email cadastrado. Verifique sua caixa de entrada.",
            chave);
    }
}
