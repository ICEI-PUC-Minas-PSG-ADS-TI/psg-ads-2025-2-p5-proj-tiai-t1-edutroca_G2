using EduTroca.Core.Abstractions;
using Microsoft.AspNetCore.Hosting;

namespace EduTroca.Infraestructure.Persistence;
public class RootFileService(IWebHostEnvironment webHost) : IFileService
{
    private const string RootFolder = "uploads";
    private readonly IWebHostEnvironment _webHost = webHost;
    public async Task SaveFileAsync(string path, string fileName, Stream stream, CancellationToken cancellationToken)
    {
        var rootPath = _webHost.WebRootPath;
        var uploadsFolder = Path.Combine(rootPath, RootFolder, path);
        var filePath = Path.Combine(uploadsFolder, fileName);
        Directory.CreateDirectory(uploadsFolder);
        using (var fs = new FileStream(filePath, FileMode.Create))
        {
            await stream.CopyToAsync(fs, cancellationToken);
        }
    }
    public Task RemoveFileAsync(string path, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(path))
        {
            return Task.CompletedTask;
        }

        var relativePath = path.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
        var physicalPath = Path.Combine(_webHost.WebRootPath, RootFolder, relativePath);

        if (File.Exists(physicalPath))
        {
            File.Delete(physicalPath);
        }

        return Task.CompletedTask;
    }
}
