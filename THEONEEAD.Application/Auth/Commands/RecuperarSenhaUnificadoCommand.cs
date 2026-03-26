using MediatR;
using THEONEEAD.Application.Auth.Dtos;

namespace THEONEEAD.Application.Auth.Commands;

/// <summary>
/// Compatível com fluxo em um único passo (ex.: Clinica RecuperarSenha): chave + código + nova senha, sem JWT intermediário.
/// </summary>
public record RecuperarSenhaUnificadoCommand(
    string ChaveVerificacao,
    string Codigo,
    string NovaSenha,
    string? NovaSenhaConfirmacao) : IRequest<TrocarSenhaResponseDto>;
