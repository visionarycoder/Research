namespace Engine.Calculator.Interface.Models;

public record FibonacciDto
{
    public int Position { get; set; }
    public long Value { get; set; }
}