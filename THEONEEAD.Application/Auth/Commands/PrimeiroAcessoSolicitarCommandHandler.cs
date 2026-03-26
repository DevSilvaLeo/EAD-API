using MediatR;
using THEONEEAD.Application.Auth;
using THEONEEAD.Application.Auth.Dtos;
using THEONEEAD.Application.Common.Interfaces;
using THEONEEAD.Domain.Entities;
using THEONEEAD.Domain.Repositories;

namespace THEONEEAD.Application.Auth.Commands;

public class PrimeiroAcessoSolicitarCommandHandler : IRequestHandler<PrimeiroAcessoSolicitarCommand, PrimeiroAcessoResponseDto>
{
    private readonly IUsuarioRepository _usuarios;
    private readonly IEstudanteReadRepository _estudantes;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ICodigoAcessoGerador _gerador;
    private readonly IRecuperacaoSenhaNotificador _notificador;
    private readonly ICodigoVerificacaoStore _store;
    private readonly IUnitOfWorkSeminario _uow;
    private static readonly TimeSpan Validade = TimeSpan.FromHours(1);

    public PrimeiroAcessoSolicitarCommandHandler(
        IUsuarioRepository usuarios,
        IEstudanteReadRepository estudantes,
        IPasswordHasher passwordHasher,
        ICodigoAcessoGerador gerador,
        IRecuperacaoSenhaNotificador notificador,
        ICodigoVerificacaoStore store,
        IUnitOfWorkSeminario uow)
    {
        _usuarios = usuarios;
        _estudantes = estudantes;
        _passwordHasher = passwordHasher;
        _gerador = gerador;
        _notificador = notificador;
        _store = store;
        _uow = uow;
    }

    public async Task<PrimeiroAcessoResponseDto> Handle(PrimeiroAcessoSolicitarCommand request, CancellationToken cancellationToken)
    {
        const string msgGenerica = "Se o CPF estiver cadastrado, você receberá um código de verificação no e-mail cadastrado.";

        var cpfN = CpfUtil.Normalizar(request.Cpf);
        if (string.IsNullOrEmpty(cpfN))
            return new PrimeiroAcessoResponseDto("Informe o CPF cadastrado.", null);

        var aluno = await _estudantes.ObterPorCpfSomenteDigitosAsync(cpfN, cancellationToken);
        if (aluno is null || string.IsNullOrEmpty(aluno.EmailNormalizado))
            return new PrimeiroAcessoResponseDto(msgGenerica, null);

        var usuario = await _usuarios.ObterPorEmailAsync(aluno.EmailNormalizado, cancellationToken);
        if (usuario is null)
        {
            var novo = Usuario.CriarRegistroAlunoPrimeiroAcesso(
                aluno.EmailNormalizado,
                _passwordHasher.Hash(Guid.NewGuid().ToString("N")));
            _usuarios.Adicionar(novo);
            await _uow.SaveChangesAsync(cancellationToken);
            usuario = await _usuarios.ObterPorEmailAsync(aluno.EmailNormalizado, cancellationToken)
                ?? throw new InvalidOperationException("Falha ao criar usuário EAD.");
        }

        if (!usuario.PrimeiroAcessoPendente)
            return new PrimeiroAcessoResponseDto("Este CPF já concluiu o primeiro acesso. Faça login.", null);

        var codigo = _gerador.GerarCodigoNumerico();
        var hash = _passwordHasher.Hash(codigo);
        var expira = DateTime.UtcNow.Add(Validade);
        var chave = _store.Registrar(usuario.Id, hash, expira, AuthFluxosCodigo.PrimeiroAcesso);
        await _notificador.NotificarCodigoAsync(
            aluno.EmailNormalizado,
            codigo,
            AuthEmailAssuntos.PrimeiroAcesso,
            expira,
            cancellationToken);

        return new PrimeiroAcessoResponseDto(
            "Código de verificação enviado para o email cadastrado. Verifique sua caixa de entrada.",
            chave);
    }
}
