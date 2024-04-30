using Access.Numbers.Interface;
using Access.Numbers.Interface.Models;
using Access.Numbers.Service.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Resource.Data.NumbersDb;
using Resource.Data.NumbersDb.Models;

namespace Access.Numbers.Service;

public class NumbersAccess(ILogger<NumbersAccess> logger, DbContextOptions<NumbersContext> options) 
    : INumbersAccess
{

    public async Task<ICollection<FibonacciDto>> FindFibonacciAsync(FibonacciFilter filter)
    {
        
        logger.LogDebug($"{nameof(NumbersAccess)}.{nameof(FindFibonacciAsync)}({filter})");
        if (filter.Take <= 0)
        {
            logger.LogDebug($"Returning an empty list.  Take='{filter.Take}'");
            return new List<FibonacciDto>();
        }

        var db = new NumbersContext(options);
        var query = db.FibonacciNumbers.AsQueryable();

        query = filter.InAscendingOrder 
            ? query.OrderBy(f => f.Id) 
            : query.OrderByDescending(f => f.Id);

        if (filter.Skip > 0)
        {
            query = query.Skip(filter.Skip);
        }
        query = query.Take(filter.Take);

        var dbObjs = await query.ToListAsync();
        var bizObjs = dbObjs.Convert();
        logger.LogDebug($"Returning an values. Count='{bizObjs.Count}'");
        return bizObjs;

    }

    public async Task<FibonacciDto> GetFibonacciAsync(int position)
    {

        logger.LogDebug($"{nameof(NumbersAccess)}.{nameof(GetFibonacciAsync)}({position})");
        var db = new NumbersContext(options);
        var dbObj = await db.FibonacciNumbers.SingleOrDefaultAsync(f => f.Id == position);
        if (dbObj is null)
        {
            logger.LogDebug($"Fibonacci value for position {position} not found.");
            return await Task.FromResult(Constants.Fibonacci.Empty);
        }
        logger.LogDebug($"Fibonacci value for position {position} found. Value='{dbObj.Value}'");
        return dbObj.Convert();

    }

    public async Task<bool> SaveFibonacciAsync(FibonacciDto fibonacci)
    {
        
        logger.LogDebug($"{nameof(NumbersAccess)}.{nameof(SaveFibonacciAsync)}({fibonacci})");
        var db = new NumbersContext(options);
        var dbObj = db.FibonacciNumbers.SingleOrDefault(f => f.Id == fibonacci.Position);
        if (dbObj is not null)
        {
            logger.LogDebug($"Fibonacci value for position {fibonacci.Position} already exists.");
            return false;
        }

        dbObj = new Fibonacci
        {
            Id = fibonacci.Position,
            Value = fibonacci.Value
        };
        db.FibonacciNumbers.Add(dbObj);
        var count = await db.SaveChangesAsync();
        logger.LogDebug($"Saved {count} Fibonacci value for position {fibonacci.Position} with value {fibonacci.Value}.");
        return count > 0;

    }

    public async Task<bool> DeleteFibonacciAsync(int position)
    {

        logger.LogDebug($"{nameof(NumbersAccess)}.{nameof(DeleteFibonacciAsync)}({position})");
        var db = new NumbersContext(options);
        db.FibonacciNumbers.Remove(new Fibonacci { Id = position });
        var count = await db.SaveChangesAsync();
        logger.LogDebug($"Count='{count}'");
        return count > 0;

    }

}