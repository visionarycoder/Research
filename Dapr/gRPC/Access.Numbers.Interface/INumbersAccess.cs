using Access.Numbers.Interface.Models;

namespace Access.Numbers.Interface;

public interface INumbersAccess
{
    Task<ICollection<FibonacciDto>> FindFibonacciAsync(FibonacciFilter filter);
    Task<FibonacciDto> GetFibonacciAsync(int position);
    Task<bool> SaveFibonacciAsync(FibonacciDto fibonacci);
    Task<bool> DeleteFibonacciAsync(int position);
}