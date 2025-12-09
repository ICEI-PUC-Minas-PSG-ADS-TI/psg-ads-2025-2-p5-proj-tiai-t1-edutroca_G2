using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities;
using EduTroca.Core.Entities.ConteudoAggregate;
using EduTroca.Core.Specifications;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Conteudos.Comment;
public class CommentCommandHandler(
    IRepository<Conteudo> conteudoRepository,
    IRepository<Comentario> comentarioRepository,
    ICurrentUserService currentUser)
    : IRequestHandler<CommentCommand, ErrorOr<Success>>
{
    private readonly IRepository<Conteudo> _conteudoRepository = conteudoRepository;
    private readonly IRepository<Comentario> _comentarioRepository = comentarioRepository;
    private readonly ICurrentUserService _currentUser = currentUser;
    public async Task<ErrorOr<Success>> Handle(CommentCommand request, CancellationToken cancellationToken)
    {
        var conteudoByIdSpec = new ConteudoById(request.conteudoId, includeDetails: true);
        var conteudo = await _conteudoRepository.AnyAsync(conteudoByIdSpec);
        if (!conteudo)
            return Error.NotFound("Conteudo.NotFound", "Conteudo inexistente.");
        var comentario = new Comentario(request.texto, _currentUser.UserId, request.conteudoId);
        await _comentarioRepository.AddAsync(comentario);
        await _conteudoRepository.SaveChangesAsync(cancellationToken);
        return Result.Success;
    }
}
