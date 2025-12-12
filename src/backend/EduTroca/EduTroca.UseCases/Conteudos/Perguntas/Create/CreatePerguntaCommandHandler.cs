using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities;
using EduTroca.Core.Entities.ConteudoAggregate;
using EduTroca.Core.Entities.UsuarioAggregate;
using EduTroca.Core.Specifications;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Conteudos.Perguntas.Create;
public class CreatePerguntaCommandHandler(
    IRepository<Conteudo> conteudoRepository,
    IRepository<Usuario> usuarioRepository,
    IRepository<Categoria> categoriaRepository,
    ICurrentUserService currentUser)
    : IRequestHandler<CreatePerguntaCommand, ErrorOr<Guid>>
{
    private readonly IRepository<Conteudo> _conteudoRepository = conteudoRepository;
    private readonly IRepository<Usuario> _usuarioRepository = usuarioRepository;
    private readonly IRepository<Categoria> _categoriaRepository = categoriaRepository;
    private readonly ICurrentUserService _currentUser = currentUser;

    public async Task<ErrorOr<Guid>> Handle(CreatePerguntaCommand request, CancellationToken cancellationToken)
    {
        var categoryByIdSpec = new CategoriaById(request.categoriaId);
        var categoriaExists = await _categoriaRepository
            .AnyAsync(categoryByIdSpec, cancellationToken);

        if (!categoriaExists)
            return Error.NotFound("Categoria.NotFound", "Categoria inexistente.");

        var pergunta = new Pergunta(
            request.titulo,
            request.descricao,
            _currentUser.UserId,
            request.categoriaId,
            request.texto
        );

        var conteudoByAutorSpec = new ConteudoByAutor(_currentUser.UserId);
        var hasConteudo = await _conteudoRepository.AnyAsync(conteudoByAutorSpec, cancellationToken);
        if (!hasConteudo)
        {
            var usuarioByIdSpec = new UsuarioById(_currentUser.UserId);
            var usuario = await _usuarioRepository.FirstOrDefaultAsync(usuarioByIdSpec, cancellationToken);
            usuario.SetNivel(Core.Enums.ENivel.Criador);
            await _usuarioRepository.UpdateAsync(usuario);
        }

        await _conteudoRepository.AddAsync(pergunta, cancellationToken);
        await _conteudoRepository.SaveChangesAsync(cancellationToken);

        return pergunta.Id;
    }
}
