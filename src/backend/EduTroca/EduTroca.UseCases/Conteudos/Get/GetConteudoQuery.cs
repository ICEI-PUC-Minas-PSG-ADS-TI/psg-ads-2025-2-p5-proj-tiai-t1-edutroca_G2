using EduTroca.UseCases.Conteudos.DTOs;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Conteudos.Get;
public record GetConteudoQuery(Guid id) : IRequest<ErrorOr<ConteudoDTO>>;
