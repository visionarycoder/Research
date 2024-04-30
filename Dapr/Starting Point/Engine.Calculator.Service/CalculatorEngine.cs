using Access.Numbers.Interface;
using Access.Numbers.Interface.Models;
using Engine.Calculator.Interface;
using Engine.Calculator.Service.Helpers;
using Microsoft.Extensions.Logging;
using FibonacciDto = Engine.Calculator.Interface.Models.FibonacciDto;
using Constants = Engine.Calculator.Interface.Constants;

namespace Engine.Calculator.Service;

public class CalculatorEngine(ILogger<CalculatorEngine> logger, INumbersAccess numbersAccess) 
    : ICalculatorEngine
{


    public async Task<FibonacciDto> CalculateFibonacciAsync(int position)
    {

        try
        {
            logger.LogDebug($"Calculating Fibonacci for position {position}");
            var filter = new FibonacciFilter
            {
                InAscendingOrder = false, 
                Skip = 0,
                Take = 2, // Get the first two Fibonacci numbers to avoid an additional database call
            };
            var fibonacciCollection = await numbersAccess.FindFibonacciAsync(filter);
            
            // Load the first two Fibonacci numbers if they are not already in the database
            switch (fibonacciCollection.Count)
            {
                case 0:
                    await InsertZeroValue(fibonacciCollection);
                    await InsertOneValue(fibonacciCollection); // Add the largest value first
                    fibonacciCollection = fibonacciCollection.Reverse().ToList();
                    break;
                case 1:
                    await InsertOneValue(fibonacciCollection);
                    fibonacciCollection = fibonacciCollection.Reverse().ToList();
                    break;
            }

            var a = fibonacciCollection.First();
            if (a.Position == position)
            {
                return a.Convert();
            }

            switch (position)
            {
                case 0:
                    return Constants.Fibonacci.Zero;
                case 1:
                    return Constants.Fibonacci.One;
            }

            var b = fibonacciCollection.Last();
            while (a.Position < position)
            {
                var temp = new Access.Numbers.Interface.Models.FibonacciDto
                {
                    Position = a.Position + 1,
                    Value = a.Value + b.Value
                };
                await numbersAccess.SaveFibonacciAsync(temp);
                b = a;
                a = temp;
            }
            return a.Convert();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error calculating Fibonacci");
            return Constants.Fibonacci.Empty;
        }

    }

    private async Task InsertOneValue(ICollection<Access.Numbers.Interface.Models.FibonacciDto> fibonacciCollection)
    {
        await numbersAccess.SaveFibonacciAsync(Constants.Fibonacci.One.Convert()).ConfigureAwait(false);
        fibonacciCollection.Add(Constants.Fibonacci.One.Convert());
    }

    private async Task InsertZeroValue(ICollection<Access.Numbers.Interface.Models.FibonacciDto> fibonacciCollection)
    {
        await numbersAccess.SaveFibonacciAsync(Constants.Fibonacci.Zero.Convert()).ConfigureAwait(false);
        fibonacciCollection.Add(Constants.Fibonacci.Zero.Convert());
    }

}