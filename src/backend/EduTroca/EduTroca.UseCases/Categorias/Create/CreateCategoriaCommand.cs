using EduTroca.Core.Entities;
using EduTroca.UseCases.Categorias.DTOs;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Categorias.Create;
public record CreateCategoriaCommand(string nome, string descricao):IRequest<ErrorOr<CategoriaDTO>>;
