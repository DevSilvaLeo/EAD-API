using MediatR;
using THEONEEAD.Application.Forum.Dtos;

namespace THEONEEAD.Application.Forum.Queries;

public record ObterForumQuery(string TipoRota, string ReferenciaId) : IRequest<ForumThreadResponseDto?>;
