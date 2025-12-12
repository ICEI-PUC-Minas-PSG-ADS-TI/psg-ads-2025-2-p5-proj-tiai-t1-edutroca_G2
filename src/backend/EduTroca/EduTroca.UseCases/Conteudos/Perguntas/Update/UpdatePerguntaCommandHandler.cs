using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities;
using EduTroca.Core.Entities.ConteudoAggregate;
using EduTroca.Core.Specifications;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Conteudos.Perguntas.Update;
public class UpdatePerguntaCommandHandler(
    IRepository<Pergunta> perguntaRepository,
    IRepository<Categoria> categoriaRepository)
    : IRequestHandler<UpdatePerguntaCommand, ErrorOr<Success>>
{
    private readonly IRepository<Pergunta> _perguntaRepository = perguntaRepository;
    private readonly IRepository<Categoria> _categoriaRepository = categoriaRepository;
    public async Task<ErrorOr<Success>> Handle(UpdatePerguntaCommand request, CancellationToken cancellationToken)
    {
        var perguntaByIdSpec = new PerguntaById(request.perguntaId);
        var pergunta = await _perguntaRepository.FirstOrDefaultAsync(perguntaByIdSpec);
        if (pergunta is null)
            return Error.NotFound("Pergunta.NotFound", "Pergunta inexistente.");

        var categoriaByIdSpec = new CategoriaById(request.categoriaId);
        var categoriaExists = await _categoriaRepository.AnyAsync(categoriaByIdSpec);
        if (!categoriaExists)
            return Error.NotFound("Categoria.NotFound", "Categoria inexistente.");

        pergunta.Update(request.titulo, request.descricao, request.categoriaId, request.texto);
        await _perguntaRepository.UpdateAsync(pergunta);
        await _perguntaRepository.SaveChangesAsync();
        return Result.Success;
    }
}
