using MediatR;
using THEONEEAD.Application.Auth.Dtos;

namespace THEONEEAD.Application.Auth.Commands;

public record ValidarCodigoCommand(string Codigo, string? ChaveVerificacao) : IRequest<ValidarCodigoResponseDto>;
