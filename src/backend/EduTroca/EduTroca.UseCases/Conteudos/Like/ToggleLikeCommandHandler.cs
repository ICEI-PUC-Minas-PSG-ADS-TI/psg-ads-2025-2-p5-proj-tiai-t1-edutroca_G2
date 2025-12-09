using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities.ConteudoAggregate;
using EduTroca.Core.Entities.UsuarioAggregate;
using EduTroca.Core.Specifications;
using EduTroca.UseCases.Common.Events;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Conteudos.Like;
public class ToggleLikeCommandHandler(
    IRepository<Conteudo> conteudoRepository,
    IRepository<Usuario> usuarioRepository,
    ICurrentUserService currentUser,
    IPublisher publisher)
    : IRequestHandler<ToggleLikeCommand, ErrorOr<Success>>
{
    private readonly IRepository<Conteudo> _conteudoRepository = conteudoRepository;
    private readonly IRepository<Usuario> _usuarioRepository = usuarioRepository;
    private readonly ICurrentUserService _currentUser = currentUser;
    private readonly IPublisher _publisher = publisher;
    public async Task<ErrorOr<Success>> Handle(ToggleLikeCommand request, CancellationToken cancellationToken)
    {
        var conteudoByIdSpec = new ConteudoById(request.conteudoId);
        var conteudo = await _conteudoRepository.FirstOrDefaultAsync(conteudoByIdSpec);
        if (conteudo is null)
            return Error.NotFound("Conteudo.NotFound", "Conteudo inexistente.");
        var conteudoByLikeSpec = new ConteudoByLike(_currentUser.UserId);
        var liked = await _conteudoRepository.AnyAsync(conteudoByLikeSpec);
        var usuarioByIdSpec = new UsuarioById(_currentUser.UserId, includeDetails: true);
        var usuario = await _usuarioRepository.FirstOrDefaultAsync(usuarioByIdSpec);
        if (liked)
            conteudo.RemoverLike(usuario);
        else
            conteudo.AdicionarLike(usuario);
        var conteudoByDislikeSpec = new ConteudoByDislike(_currentUser.UserId);
        var disliked = await _conteudoRepository.AnyAsync(conteudoByDislikeSpec);
        if (disliked)
            conteudo.RemoverDislike(usuario);
        await _conteudoRepository.UpdateAsync(conteudo);
        await _conteudoRepository.SaveChangesAsync(cancellationToken);
        await _publisher.Publish(new LikeRegistradoEvent(conteudo.AutorId), cancellationToken);
        return Result.Success;
    }
}
