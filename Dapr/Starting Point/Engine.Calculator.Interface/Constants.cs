using Engine.Calculator.Interface.Models;

namespace Engine.Calculator.Interface;

public static class Constants
{

    public static class Fibonacci
    {

        public static Models.FibonacciDto Empty => new() { Position = -1, Value = -1 };
        public static FibonacciDto Zero => new() { Position = 0, Value = 0 };
        public static FibonacciDto One => new() { Position = 1, Value = 1 };
    }

}