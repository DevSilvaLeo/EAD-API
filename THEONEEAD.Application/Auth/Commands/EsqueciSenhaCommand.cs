using MediatR;
using THEONEEAD.Application.Auth.Dtos;

namespace THEONEEAD.Application.Auth.Commands;

public record EsqueciSenhaCommand(string? Email, string? Cpf) : IRequest<EsqueciSenhaResponseDto>;
