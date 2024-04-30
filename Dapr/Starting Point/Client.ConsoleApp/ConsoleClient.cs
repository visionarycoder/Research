using Manager.Content.Interface;
using Microsoft.Extensions.Logging;

namespace Client.ConsoleApp
{
    public class ConsoleClient(ILogger<ConsoleClient> logger, IContentManager contentManager)
    {

        private const int DefaultPosition = 10;

        public async Task RunAsync(params string[] args)
        {
    
            try
            {
                logger.LogInformation($"{nameof(ConsoleClient)}.{nameof(RunAsync)}({args})");
                var arguments = ConvertToIntegers(args);
                foreach (var arg in arguments)
                {
                    var fibonacciDto = await contentManager.CalculateFibonacciAsync(arg);
                    Console.WriteLine($"Fibonacci value for position {arg} is {fibonacciDto.Value}");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(ConsoleClient)}.{nameof(RunAsync)}()");
            }

            Console.WriteLine("Press any key to exit.");
            Console.Read();
        }

        private static IEnumerable<int> ConvertToIntegers(IEnumerable<string> args)
        {
            
            var integers = new List<int>();
            foreach (var arg in args.Where(i => !string.IsNullOrWhiteSpace(i)))
            {
                if (int.TryParse(arg, out var integer))
                {
                    integers.Add(integer);
                }
            }

            if (integers.Count is 0)
            {
                integers.Add(DefaultPosition);
            }
            return integers;

        }

    }

}
