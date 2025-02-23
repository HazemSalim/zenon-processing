using Microsoft.Extensions.DependencyInjection;
using ZenonFileProcessor.Helpers;

class Program
{
    // The entry point of the application
    static async Task Main(string[] args)
    {
        // Validate command-line arguments and retrieve the input file path
        if (!ArgumentValidator.Validate(args, out string inputFilePath))
            return; // If validation fails, exit the program

        // Construct the output file path by appending "output.txt" to the input file's directory
        string outputFilePath = Path.Combine(Path.GetDirectoryName(inputFilePath) ?? "", "output.txt");

        // Set up the dependency injection container
        using var serviceProvider = Startup.ConfigureServices();

        // Retrieve the 'App' instance from the DI container
        var app = serviceProvider.GetRequiredService<App>();

        // Run the application asynchronously with the input and output file paths
        await app.RunAsync(inputFilePath, outputFilePath);
    }
}
