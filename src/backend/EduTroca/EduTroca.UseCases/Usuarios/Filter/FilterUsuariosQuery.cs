using EduTroca.Core.Common;
using EduTroca.UseCases.Common.DTOs;
using EduTroca.UseCases.Usuarios.DTOs;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Usuarios.Filter;
public record FilterUsuariosQuery(string? nome, List<Guid>? categoriasIds, PaginationDTO pagination) 
    : IRequest<ErrorOr<PagedResult<UsuarioDTO>>>;
