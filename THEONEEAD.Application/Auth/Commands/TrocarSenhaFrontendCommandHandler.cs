using MediatR;
using THEONEEAD.Application.Auth;
using THEONEEAD.Application.Auth.Dtos;
using THEONEEAD.Application.Common.Interfaces;
using THEONEEAD.Domain.Repositories;

namespace THEONEEAD.Application.Auth.Commands;

public class TrocarSenhaFrontendCommandHandler : IRequestHandler<TrocarSenhaFrontendCommand, TrocarSenhaResponseDto>
{
    private readonly IUsuarioRepository _usuarios;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwt;
    private readonly IUnitOfWorkSeminario _uow;

    public TrocarSenhaFrontendCommandHandler(
        IUsuarioRepository usuarios,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwt,
        IUnitOfWorkSeminario uow)
    {
        _usuarios = usuarios;
        _passwordHasher = passwordHasher;
        _jwt = jwt;
        _uow = uow;
    }

    public async Task<TrocarSenhaResponseDto> Handle(TrocarSenhaFrontendCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Token))
            return new TrocarSenhaResponseDto("Sessão expirada. Solicite novamente a recuperação de senha.");

        if (!string.Equals(request.Senha, request.SenhaConfirmacao, StringComparison.Ordinal))
            return new TrocarSenhaResponseDto("As senhas não coincidem.");

        if (!SenhaPolitica.Validar(request.Senha, out var erroSenha))
            return new TrocarSenhaResponseDto(erroSenha);

        var userId = _jwt.ObterUsuarioIdDoTokenRedefinicao(request.Token.Trim());
        if (userId is null)
            return new TrocarSenhaResponseDto("Sessão expirada. Solicite novamente a recuperação de senha.");

        var usuario = await _usuarios.ObterPorIdAsync(userId.Value, cancellationToken);
        if (usuario is null)
            return new TrocarSenhaResponseDto("Usuário não encontrado.");

        var hash = _passwordHasher.Hash(request.Senha);
        if (usuario.PrimeiroAcessoPendente)
            usuario.ConcluirPrimeiroAcesso(hash);
        else
            usuario.TrocarSenhaComCodigo(hash);

        await _uow.SaveChangesAsync(cancellationToken);
        return new TrocarSenhaResponseDto("Senha alterada com sucesso! Você já pode fazer login com a nova senha.");
    }
}
