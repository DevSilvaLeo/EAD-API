using MediatR;
using THEONEEAD.Application.Auth.Dtos;

namespace THEONEEAD.Application.Auth.Commands;

public record TrocarSenhaFrontendCommand(string Senha, string SenhaConfirmacao, string Token)
    : IRequest<TrocarSenhaResponseDto>;
