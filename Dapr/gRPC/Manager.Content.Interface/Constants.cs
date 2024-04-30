using Manager.Content.Interface.Models;

namespace Manager.Content.Interface;

public static class Constants
{

    public static class Fibonacci
    {

        public static FibonacciDto Empty => new() { Position = -1, Value = -1 };

    }

}