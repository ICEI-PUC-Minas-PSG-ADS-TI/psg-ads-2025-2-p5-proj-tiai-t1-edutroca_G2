namespace EduTroca.Core.Common;
public record DateRangeFilter
{
    public DateTime? From { get; init; }
    public DateTime? To { get; init; }

    public bool IsValid()
    {
        if (!From.HasValue || !To.HasValue)
            return true;

        return From.Value <= To.Value;
    }

    public static DateRangeFilter? Create(DateTime? from, DateTime? to)
    {
        if (!from.HasValue && !to.HasValue)
            return null;

        return new DateRangeFilter { From = from, To = to };
    }
}
