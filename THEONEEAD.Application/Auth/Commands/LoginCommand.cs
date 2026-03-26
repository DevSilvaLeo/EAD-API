using MediatR;
using THEONEEAD.Application.Auth.Dtos;

namespace THEONEEAD.Application.Auth.Commands;

public record LoginCommand(string Email, string Senha) : IRequest<LoginResponseDto>;
