using EduTroca.UseCases.Common.DTOs;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Conteudos.Videos.Create;
public record CreateVideoCommand(string titulo, string descricao, Guid categoriaId, FileDTO video, FileDTO imagem) 
    : IRequest<ErrorOr<Guid>>;
