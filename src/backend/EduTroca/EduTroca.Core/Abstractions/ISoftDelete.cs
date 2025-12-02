namespace EduTroca.Core.Abstractions;
public interface ISoftDelete
{
    bool IsDeleted { get; }
    DateTime? DeletedOnUtc { get; }
    void Delete();
}
