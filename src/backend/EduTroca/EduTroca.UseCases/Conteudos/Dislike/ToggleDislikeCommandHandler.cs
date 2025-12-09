using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities.ConteudoAggregate;
using EduTroca.Core.Entities.UsuarioAggregate;
using EduTroca.Core.Specifications;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Conteudos.Dislike;
public class ToggleDislikeCommandHandler(
    IRepository<Conteudo> conteudoRepository,
    IRepository<Usuario> usuarioRepository,
    ICurrentUserService currentUser)
    : IRequestHandler<ToggleDislikeCommand, ErrorOr<Success>>
{
    private readonly IRepository<Conteudo> _conteudoRepository = conteudoRepository;
    private readonly IRepository<Usuario> _usuarioRepository = usuarioRepository;
    private readonly ICurrentUserService _currentUser = currentUser;
    public async Task<ErrorOr<Success>> Handle(ToggleDislikeCommand request, CancellationToken cancellationToken)
    {
        var conteudoByIdSpec = new ConteudoById(request.conteudoId);
        var conteudo = await _conteudoRepository.FirstOrDefaultAsync(conteudoByIdSpec);
        if (conteudo is null)
            return Error.NotFound("Conteudo.NotFound", "Conteudo inexistente.");
        var conteudoByDislikeSpec = new ConteudoByDislike(_currentUser.UserId);
        var disliked = await _conteudoRepository.AnyAsync(conteudoByDislikeSpec);
        var usuarioByIdSpec = new UsuarioById(_currentUser.UserId, includeDetails: true);
        var usuario = await _usuarioRepository.FirstOrDefaultAsync(usuarioByIdSpec);
        if (disliked)
            conteudo.RemoverDislike(usuario);
        else
            conteudo.AdicionarDislike(usuario);
        var conteudoByLikeSpec = new ConteudoByLike(_currentUser.UserId);
        var liked = await _conteudoRepository.AnyAsync(conteudoByLikeSpec);
        if (liked)
            conteudo.RemoverLike(usuario);
        await _conteudoRepository.UpdateAsync(conteudo);
        await _conteudoRepository.SaveChangesAsync(cancellationToken);
        return Result.Success;
    }
}
