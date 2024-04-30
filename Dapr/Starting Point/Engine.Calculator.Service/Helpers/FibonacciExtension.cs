using Engine.Calculator.Interface.Models;

namespace Engine.Calculator.Service.Helpers;

public static class FibonacciExtension 
{

    public static int Compare(this FibonacciDto? x, FibonacciDto? y)
    {
            
        switch (x)
        {
            case null when y is null:
                return 0;
            case null: // y is not null
                return 1;
            case not null when y is null:
                return -1;
            default:
                var positionComparison = x.Position.CompareTo(y.Position);
                return positionComparison != 0
                    ? positionComparison
                    : x.Value.CompareTo(y.Value);
        }

    }

    public static FibonacciDto Convert(this Access.Numbers.Interface.Models.FibonacciDto source)
    {

        var target = new FibonacciDto
        {
            Position = source.Position,
            Value = source.Value
        };
        return target;
    }

    public static Access.Numbers.Interface.Models.FibonacciDto Convert(this FibonacciDto source)
    {

        var target = new Access.Numbers.Interface.Models.FibonacciDto
        {
            Position = source.Position,
            Value = source.Value
        };
        return target;
    }

}