namespace EduTroca.UseCases.Common.Authorization.Policies;
public interface IAuthorizationPolicy
{
    string Name { get; }
    string Description { get; }
}
