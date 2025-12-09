using EduTroca.UseCases.Common.DTOs;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Conteudos.Videos.Update;
public record UpdateVideoCommand(Guid videoId, string titulo, string descricao, Guid categoriaId, FileDTO imagem) 
    : IRequest<ErrorOr<Success>>;
