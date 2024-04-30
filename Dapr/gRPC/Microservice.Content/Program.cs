using System.Diagnostics;
using System.Net.Mime;
using System.Reflection;
using Access.Numbers.Interface;
using Access.Numbers.Service;
using Client.ConsoleApp;
using Engine.Calculator.Interface;
using Engine.Calculator.Service;
using Manager.Content.Interface;
using Manager.Content.Service;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.Metrics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Resource.Data.NumbersDb;

var host = new HostBuilder()
    .ConfigureDefaults(args)
    .UseContentRoot(Directory.GetCurrentDirectory())
    .ConfigureHostOptions((context, options) =>
    {
        
        options.ServicesStartConcurrently = false;
        options.ServicesStopConcurrently = false;
        options.StartupTimeout = TimeSpan.FromSeconds(0);
        options.ShutdownTimeout = TimeSpan.FromSeconds(5);

    })
    .ConfigureHostConfiguration(builder =>
    {

        builder.AddEnvironmentVariables();

    })
    .ConfigureAppConfiguration((context, builder) =>
    {

        var env = context.HostingEnvironment;
        var reloadOnChange = context.Configuration.GetValue<bool?>("AppSettings:ReloadOnChange") ?? true;
        var optional = context.Configuration.GetValue<bool?>("AppSettings:Optional") ?? true;
        if (env.IsDevelopment() && env.ApplicationName is { Length: > 0 })
        {
            var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
            builder.AddUserSecrets(appAssembly, optional: optional, reloadOnChange: reloadOnChange);
        }
        builder.AddJsonFile("AppSettings.json", optional: optional, reloadOnChange: reloadOnChange);
        builder.AddJsonFile($"AppSettings.{env.EnvironmentName}.json", optional: optional, reloadOnChange: reloadOnChange);

    })
    .ConfigureLogging(builder =>
    {

        builder.ClearProviders();
        //builder.AddDebug();
        //builder.AddJsonConsole(config =>
        //{
        //    config.TimestampFormat = "[yyyy-MM-dd HH:mm:ss.fff] ";
        //});
        //builder.SetMinimumLevel(LogLevel.Trace);

    })
    .ConfigureServices((context, services) =>
    {

        var cache = SqliteCacheMode.Shared;
        var databasePath = Path.Join(Environment.CurrentDirectory, "Numbers.db");
        var mode = SqliteOpenMode.ReadWriteCreate;

        Debug.WriteLine($"Cache={cache}");
        Debug.WriteLine($"Database Path={databasePath}");
        Debug.WriteLine($"Mode={mode}");

        var connectionString = new SqliteConnectionStringBuilder
            {
                Cache = cache,
                DataSource = databasePath,
                Mode = mode,
            }.ToString();

        Debug.WriteLine($"Connection String={connectionString}");
        var dbContextOptions = new DbContextOptionsBuilder<NumbersContext>()
            .UseSqlite(connectionString)
            .Options;

        services.AddSingleton(dbContextOptions);
        services.AddDbContext<NumbersContext>(options => 
        {
            options.UseSqlite(connectionString);
        });
        services.AddTransient<ConsoleClient>();
        services.AddTransient<IContentManager, ContentManager>();
        services.AddTransient<ICalculatorEngine, CalculatorEngine>();
        services.AddTransient<INumbersAccess, NumbersAccess>();

    })
    .ConfigureMetrics((context, builder) =>
    {

        builder.EnableMetrics("Fibonacci Microservice");

    })
    .UseConsoleLifetime(options =>
    {

        options.SuppressStatusMessages = false;

    })
    .Build();

var dbContext = host.Services.GetRequiredService<NumbersContext>();
await dbContext.Database.EnsureDeletedAsync();
await dbContext.Database.EnsureCreatedAsync();

var client = host.Services.GetRequiredService<ConsoleClient>();
await client.RunAsync().ConfigureAwait(true);
