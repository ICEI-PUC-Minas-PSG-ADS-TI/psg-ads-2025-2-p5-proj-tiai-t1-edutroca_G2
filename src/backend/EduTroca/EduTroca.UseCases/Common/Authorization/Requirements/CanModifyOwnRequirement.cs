using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities.ConteudoAggregate;
using EduTroca.Core.Specifications;
using EduTroca.UseCases.Common.Authorization.Contexts;
using ErrorOr;

namespace EduTroca.UseCases.Common.Authorization.Requirements;
public class CanModifyOwnRequirement(IRepository<Conteudo> conteudoRepository)
    : IAuthorizationRequirement<ContentModificationContext>
{
    public async Task<ErrorOr<Success>> EvaluateAsync(
        ContentModificationContext context,
        CancellationToken cancellationToken)
    {

        var spec = new ConteudoById(context.ContentId);
        var conteudo = await conteudoRepository
            .FirstOrDefaultAsync(spec, cancellationToken);

        if (conteudo is null)
            return Error.NotFound("Conteudo.NotFound", "Conteudo inexistente.");

        if (conteudo.AutorId != context.CurrentUserId)
            return Error.Forbidden("Authorization.Forbidden",
                "Usuarios só podem modificar os próprios conteudos.");

        return Result.Success;
    }
}
