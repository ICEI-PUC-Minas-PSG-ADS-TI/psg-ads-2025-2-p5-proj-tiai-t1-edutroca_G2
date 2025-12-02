namespace EduTroca.UseCases.Common.DTOs;
public class PictureDTO
{
    public Stream Stream { get; private set; }
    public string ContentType { get; private set; }
    public long Length { get; private set; }

    public PictureDTO(Stream stream, string contentType, long length)
    {
        Stream = stream;
        ContentType = contentType;
        Length = length;
    }
}
