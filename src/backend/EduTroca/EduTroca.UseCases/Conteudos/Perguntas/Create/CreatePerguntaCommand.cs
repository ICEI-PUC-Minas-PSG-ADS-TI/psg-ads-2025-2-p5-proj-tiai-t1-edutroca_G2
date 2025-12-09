using EduTroca.UseCases.Common.DTOs;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Conteudos.Perguntas.Create;
public record CreatePerguntaCommand(string titulo, string descricao, Guid categoriaId, string texto) 
    : IRequest<ErrorOr<Guid>>;
