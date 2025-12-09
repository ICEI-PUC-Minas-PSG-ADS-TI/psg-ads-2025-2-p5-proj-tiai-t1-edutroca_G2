namespace EduTroca.Core.Common;
public record RangeFilter<T> where T : struct, IComparable<T>
{
    public T? Min { get; init; }
    public T? Max { get; init; }

    public bool IsValid()
    {
        if (!Min.HasValue || !Max.HasValue)
            return true;

        return Min.Value.CompareTo(Max.Value) <= 0;
    }

    public static RangeFilter<T>? Create(T? min, T? max)
    {
        if (!min.HasValue && !max.HasValue)
            return null;

        return new RangeFilter<T> { Min = min, Max = max };
    }
}
