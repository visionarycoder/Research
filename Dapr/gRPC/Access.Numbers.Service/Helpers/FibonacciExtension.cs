using Access.Numbers.Interface.Models;
using Resource.Data.NumbersDb.Models;

namespace Access.Numbers.Service.Helpers;

public static class FibonacciExtension
{

    public static FibonacciDto Convert(this Fibonacci source)
    {
        var target = new FibonacciDto
        {
            Position = source.Id,
            Value = source.Value
        };
        return target;
    }

    public static Fibonacci Convert(this FibonacciDto source)
    {
        var target = new Fibonacci
        {
            Id = source.Position,
            Value = source.Value
        };
        return target;
    }

    public static ICollection<FibonacciDto> Convert(this ICollection<Fibonacci> source)
    {
        var target = source.Select(f => f.Convert()).ToList();
        return target;
    }

    public static ICollection<Fibonacci> Convert(this ICollection<FibonacciDto> source)
    {
        var target = source.Select(f => f.Convert()).ToList();
        return target;
    }

}