using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities.ConteudoAggregate;
using EduTroca.Core.Entities.UsuarioAggregate;
using EduTroca.Core.Enums;
using EduTroca.Core.Specifications;
using MediatR;

namespace EduTroca.UseCases.Common.Events.Handlers;
public class AtualizarNivelUsuarioEventHandler(
    IRepository<Usuario> usuarioRepository,
    IRepository<Conteudo> conteudoRepository)
    : INotificationHandler<LikeRegistradoEvent>
{
    public async Task Handle(LikeRegistradoEvent notification, CancellationToken cancellationToken)
    {
        var autorSpec = new UsuarioById(notification.autorId);
        var autor = await usuarioRepository.FirstOrDefaultAsync(autorSpec);

        if (autor is null) return;

        var conteudosSpec = new ConteudoByAutor(notification.autorId, includeDetails: true);
        var conteudos = await conteudoRepository.ListAsync(conteudosSpec, cancellationToken);

        var totalLikes = conteudos.Sum(c => c.Likes.Count);
        var totalDislikes = conteudos.Sum(c => c.Dislikes.Count);
        var totalConteudos = conteudos.Count;
        if(totalConteudos > 0)
        {
            var pesoDislike = 1.5;
            var saldoEngajamento = totalLikes - (totalDislikes * pesoDislike);
            var mediaPorConteudo = saldoEngajamento / totalConteudos;
            if (mediaPorConteudo >= 10)
            {
                if (autor.Nivel < ENivel.CriadorPleno)
                    autor.SetNivel(ENivel.CriadorPleno);
            }
        }

        await usuarioRepository.UpdateAsync(autor);
        await usuarioRepository.SaveChangesAsync(cancellationToken);
    }
}
