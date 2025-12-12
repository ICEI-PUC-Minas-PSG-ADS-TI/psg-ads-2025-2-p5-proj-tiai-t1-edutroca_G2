using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities.ConteudoAggregate;
using EduTroca.Core.Specifications;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Conteudos.Perguntas.Delete;
public class DeletePerguntaCommandHandler(IRepository<Conteudo> conteudoRepository)
    : IRequestHandler<DeletePerguntaCommand, ErrorOr<Success>>
{
    private readonly IRepository<Conteudo> _conteudoRepository = conteudoRepository;
    public async Task<ErrorOr<Success>> Handle(DeletePerguntaCommand request, CancellationToken cancellationToken)
    {
        var conteudoByIdSpec = new ConteudoById(request.perguntaId);
        var pergunta = await _conteudoRepository.FirstOrDefaultAsync(conteudoByIdSpec, cancellationToken);
        if(pergunta is null)
            return Error.NotFound("Pergunta.NotFound", "Pergunta inexistente.");
        await _conteudoRepository.RemoveAsync(pergunta);
        await _conteudoRepository.SaveChangesAsync(cancellationToken);
        return Result.Success;
    }
}
