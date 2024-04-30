using Engine.Calculator.Interface.Models;

namespace Engine.Calculator.Interface;

public interface ICalculatorEngine
{

    Task<FibonacciDto> CalculateFibonacciAsync(int position);

}