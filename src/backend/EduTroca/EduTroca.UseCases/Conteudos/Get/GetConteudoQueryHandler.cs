using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities.ConteudoAggregate;
using EduTroca.Core.Specifications;
using EduTroca.UseCases.Conteudos.DTOs;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Conteudos.Get;
public class GetConteudoQueryHandler(IRepository<Conteudo> conteudoRepository, ICurrentUserService currentUser)
    : IRequestHandler<GetConteudoQuery, ErrorOr<ConteudoDTO>>
{
    private readonly IRepository<Conteudo> _conteudoRepository = conteudoRepository;
    private readonly ICurrentUserService _currentUser = currentUser;
    public async Task<ErrorOr<ConteudoDTO>> Handle(GetConteudoQuery request, CancellationToken cancellationToken)
    {
        var conteudoByIdSpec = new ConteudoById(request.id, includeDetails: true);
        var conteudo = await _conteudoRepository.FirstOrDefaultAsync(conteudoByIdSpec);
        if (conteudo is null)
            return Error.NotFound("Video.NotFound", "Video inexistente.");
        conteudo.RegistrarVisualizacao();
        await _conteudoRepository.UpdateAsync(conteudo);
        await _conteudoRepository.SaveChangesAsync();
        var liked = await _conteudoRepository.AnyAsync(new ConteudoByLike(_currentUser.UserId));
        var disliked = await _conteudoRepository.AnyAsync(new ConteudoByDislike(_currentUser.UserId));
        return ConteudoDTO.FromConteudo(conteudo, liked, disliked);
    }
}
