using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using ZenonFileProcessor.Services;
using System;
using System.IO;

public static class Startup
{
    // ConfigureServices method sets up dependency injection
    public static ServiceProvider ConfigureServices()
    {
        // Get the current date for log file name
        string logFileName = Path.Combine("logs", $"log_{DateTime.Now:yyyyMMdd}.txt");

        try
        {
            // Configure Serilog for file logging with date-based filenames
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()  // Set minimum log level to Debug
                .WriteTo.Console() // Log to Console
                .WriteTo.File(logFileName, rollingInterval: RollingInterval.Day) // Log to file with daily rolling interval
                .CreateLogger();
        }
        catch (Exception ex)
        {
            // Handle logger setup failure
            Console.WriteLine($"Serilog setup failed: {ex.Message}");
            throw; // Re-throw the exception
        }

        // Return the configured service provider
        return new ServiceCollection()
            .AddLogging(builder => builder.AddSerilog()) // Add Serilog to DI
            .AddSingleton<App>()
            .AddSingleton<FileParser>()
            .AddSingleton<Processor>()
            .BuildServiceProvider();
    }

    // Call this method to ensure proper disposal of Serilog
    public static void DisposeLogger()
    {
        Log.CloseAndFlush();
    }
}
