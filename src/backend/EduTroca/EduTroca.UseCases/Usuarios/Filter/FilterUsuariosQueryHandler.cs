using EduTroca.Core.Abstractions;
using EduTroca.Core.Common;
using EduTroca.Core.Entities.UsuarioAggregate;
using EduTroca.Core.Specifications;
using EduTroca.UseCases.Usuarios.DTOs;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Usuarios.Filter;
public class FilterUsuariosQueryHandler(
    IRepository<Usuario> usuarioRepository)
    : IRequestHandler<FilterUsuariosQuery, ErrorOr<PagedResult<UsuarioDTO>>>
{
    private readonly IRepository<Usuario> _usuarioRepository = usuarioRepository;
    public async Task<ErrorOr<PagedResult<UsuarioDTO>>> Handle(FilterUsuariosQuery request, CancellationToken cancellationToken)
    {
        var usuariosByFilterSpecification = new UsuarioByFilter(request.nome, request.categoriasIds);
        var usuarios = await _usuarioRepository.ListPagedAsync(
            request.pagination.PageNumber,
            request.pagination.PageSize,
            usuariosByFilterSpecification);
        return usuarios.Map(UsuarioDTO.FromUsuario);
    }
}
