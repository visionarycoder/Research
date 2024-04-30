using Manager.Content.Interface.Models;

namespace Manager.Content.Service.Helpers;

public static class FibonacciExtension 
{

    public static FibonacciDto Convert(this Engine.Calculator.Interface.Models.FibonacciDto source)
    {

        var target = new FibonacciDto
        {
            Position = source.Position,
            Value = source.Value
        };
        return target;
    }

    public static Engine.Calculator.Interface.Models.FibonacciDto Convert(this FibonacciDto source)
    {

        var target = new Engine.Calculator.Interface.Models.FibonacciDto
        {
            Position = source.Position,
            Value = source.Value
        };
        return target;
    }

}