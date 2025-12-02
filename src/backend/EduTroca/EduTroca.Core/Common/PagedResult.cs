namespace EduTroca.Core.Common;
public class PagedResult<T>(List<T> items, int totalCount, int pageNumber, int pageSize)
{
    public List<T> Items { get; } = items;
    public int TotalCount { get; } = totalCount;
    public int PageNumber { get; } = pageNumber;
    public int PageSize { get; } = pageSize;
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasNextPage => PageNumber * PageSize < TotalCount;
    public bool HasPreviousPage => PageNumber > 1;

    public PagedResult<TDest> Map<TDest>(Func<T, TDest> mapFunc)
    {
        var destinationItems = Items.Select(mapFunc).ToList();

        return new PagedResult<TDest>(
            destinationItems,
            TotalCount,
            PageNumber,
            PageSize);
    }
}
