using System.Diagnostics;
using System.Reflection.Emit;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Resource.Data.NumbersDb.Models;

namespace Resource.Data.NumbersDb;

public class NumbersContext(DbContextOptions<NumbersContext> options) 
    : DbContext(options)
{

    public DbSet<Fibonacci> FibonacciNumbers { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        Debug.WriteLine($"{nameof(NumbersContext)}.{nameof(OnModelCreating)}({modelBuilder}");
        OnModelCreating(modelBuilder.Entity<Fibonacci>());
        base.OnModelCreating(modelBuilder);
        Debug.WriteLine($"{nameof(NumbersContext)}.{nameof(OnModelCreating)}({modelBuilder}) done");

    }

    private void OnModelCreating(EntityTypeBuilder<Fibonacci> entity)
    {

        Debug.WriteLine($"{nameof(NumbersContext)}.{nameof(OnModelCreating)}({entity}");
        entity.ToTable(nameof(Fibonacci));
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).ValueGeneratedOnAdd();
        entity.Property(e => e.Value).IsRequired();
        Debug.WriteLine($"{nameof(NumbersContext)}.{nameof(OnModelCreating)}({entity}) done");
    }

}