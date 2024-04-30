using Manager.Content.Interface.Models;

namespace Manager.Content.Interface;

public interface IContentManager
{
    Task<FibonacciDto> CalculateFibonacciAsync(int position);
}