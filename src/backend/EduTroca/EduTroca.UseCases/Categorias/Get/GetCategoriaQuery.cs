using EduTroca.UseCases.Categorias.DTOs;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Categorias.Get;
public record GetCategoriaQuery(Guid id) : IRequest<ErrorOr<CategoriaDTO>>;
