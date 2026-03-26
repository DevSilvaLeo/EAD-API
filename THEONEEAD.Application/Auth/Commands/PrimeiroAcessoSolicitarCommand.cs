using MediatR;
using THEONEEAD.Application.Auth.Dtos;

namespace THEONEEAD.Application.Auth.Commands;

public record PrimeiroAcessoSolicitarCommand(string Cpf) : IRequest<PrimeiroAcessoResponseDto>;
