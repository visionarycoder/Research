using Engine.Calculator.Interface;
using Manager.Content.Interface;
using Manager.Content.Interface.Models;
using Manager.Content.Service.Helpers;
using Microsoft.Extensions.Logging;
using Fibonacci = Manager.Content.Interface.Constants.Fibonacci;

namespace Manager.Content.Service;

public class ContentManager(ILogger<ContentManager> logger, ICalculatorEngine calculatorEngine) 
    : IContentManager
{

    public async Task<FibonacciDto> CalculateFibonacciAsync(int position)
    {

        try
        {
            logger.LogDebug($"{nameof(ContentManager)}.{nameof(CalculateFibonacciAsync)}({position})");
            var fibonacciDto = await calculatorEngine.CalculateFibonacciAsync(position);
            return fibonacciDto.Convert();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error calculating Fibonacci for position {position}");
            return Fibonacci.Empty;
        }

    }
    
}