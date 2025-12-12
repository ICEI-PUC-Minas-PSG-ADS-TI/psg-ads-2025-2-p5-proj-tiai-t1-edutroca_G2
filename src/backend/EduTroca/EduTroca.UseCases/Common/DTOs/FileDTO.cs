namespace EduTroca.UseCases.Common.DTOs;
public class FileDTO
{
    public Stream Stream { get; private set; }
    public string ContentType { get; private set; }
    public long Length { get; private set; }

    public FileDTO(Stream stream, string contentType, long length)
    {
        Stream = stream;
        ContentType = contentType;
        Length = length;
    }
}
