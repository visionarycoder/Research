namespace Ifx;

public abstract class Filter
{
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = int.MaxValue;
}