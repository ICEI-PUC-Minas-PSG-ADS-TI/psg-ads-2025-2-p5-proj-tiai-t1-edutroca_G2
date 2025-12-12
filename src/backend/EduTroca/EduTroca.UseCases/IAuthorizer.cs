using ErrorOr;

namespace EduTroca.UseCases;
public interface IAuthorizer<T> 
{
    Task<ErrorOr<Success>> AuthorizeAsync(T request, CancellationToken cancellationToken);
}
