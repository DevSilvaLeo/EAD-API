using MediatR;
using THEONEEAD.Application.Auth.Dtos;
using THEONEEAD.Application.Common.Interfaces;
using THEONEEAD.Domain.Repositories;

namespace THEONEEAD.Application.Auth.Commands;

public class RecuperarSenhaUnificadoCommandHandler : IRequestHandler<RecuperarSenhaUnificadoCommand, TrocarSenhaResponseDto>
{
    private readonly ICodigoVerificacaoStore _store;
    private readonly IUsuarioRepository _usuarios;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWorkSeminario _uow;

    public RecuperarSenhaUnificadoCommandHandler(
        ICodigoVerificacaoStore store,
        IUsuarioRepository usuarios,
        IPasswordHasher passwordHasher,
        IUnitOfWorkSeminario uow)
    {
        _store = store;
        _usuarios = usuarios;
        _passwordHasher = passwordHasher;
        _uow = uow;
    }

    public async Task<TrocarSenhaResponseDto> Handle(RecuperarSenhaUnificadoCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.ChaveVerificacao))
            return new TrocarSenhaResponseDto("Chave de verificação ausente. Solicite novamente o código.");

        var confirmacao = request.NovaSenhaConfirmacao ?? request.NovaSenha;
        if (!string.Equals(request.NovaSenha, confirmacao, StringComparison.Ordinal))
            return new TrocarSenhaResponseDto("As senhas não coincidem.");

        if (!SenhaPolitica.Validar(request.NovaSenha, out var erroSenha))
            return new TrocarSenhaResponseDto(erroSenha);

        if (!_store.TentarValidarEConsumir(
                request.ChaveVerificacao.Trim(),
                request.Codigo.Trim(),
                out var usuarioId,
                out _))
            return new TrocarSenhaResponseDto("Código inválido ou expirado. Verifique e tente novamente.");

        var usuario = await _usuarios.ObterPorIdAsync(usuarioId, cancellationToken);
        if (usuario is null)
            return new TrocarSenhaResponseDto("Usuário não encontrado.");

        var hash = _passwordHasher.Hash(request.NovaSenha);
        if (usuario.PrimeiroAcessoPendente)
            usuario.ConcluirPrimeiroAcesso(hash);
        else
            usuario.TrocarSenhaComCodigo(hash);

        await _uow.SaveChangesAsync(cancellationToken);
        return new TrocarSenhaResponseDto("Senha alterada com sucesso! Você já pode fazer login com a nova senha.");
    }
}
