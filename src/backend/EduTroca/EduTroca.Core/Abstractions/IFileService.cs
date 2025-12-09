namespace EduTroca.Core.Abstractions;
public interface IFileService
{
    Task SaveFileAsync(string path, string fileName, Stream stream, CancellationToken cancellationToken);
    Task RemoveFileAsync(string path, CancellationToken cancellationToken = default);
}
