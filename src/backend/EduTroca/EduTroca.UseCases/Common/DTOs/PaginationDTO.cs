namespace EduTroca.UseCases.Common.DTOs;
public class PaginationDTO
{
    public int PageNumber { get; private set; }
    public int PageSize { get; private set; }

    public PaginationDTO(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}
