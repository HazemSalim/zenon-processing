using Microsoft.Extensions.Logging;
using ZenonFileProcessor.Services;

public class App
{
    // Declare private fields for logging, file parsing, and processing services
    private readonly ILogger<App> _logger;
    private readonly FileParser _parser;
    private readonly Processor _processor;

    // Constructor to inject dependencies into the App class
    public App(ILogger<App> logger, FileParser parser, Processor processor)
    {
        _logger = logger;
        _parser = parser;
        _processor = processor;
    }

    public async Task RunAsync(string inputFilePath, string outputFilePath)
    {
        try
        {
            // Log the start of file parsing
            _logger.LogInformation("Parsing file: {InputFilePath}", inputFilePath);

            // Call the file parser to parse the input file
            await _parser.ParseFileAsync(inputFilePath);

            // Log the start of min/max calculation
            _logger.LogInformation("Calculating min/max results...");

            // Call the processor to get the min/max results from parsed data
            var results = await _processor.GetMinMaxResultsAsync(_parser.Variables, _parser.Values);

            // Write the results to the output file
            await File.WriteAllLinesAsync(outputFilePath, results);

            _logger.LogInformation("Processing complete. Output written to {OutputFilePath}", outputFilePath);
        }
        catch (FileNotFoundException ex)
        {
            _logger.LogError(ex, "File not found: {FilePath}", inputFilePath);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "Permission denied when accessing the file.");
        }
        catch (IOException ex)
        {
            _logger.LogError(ex, "I/O error occurred during processing.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred during processing.");
        }
        finally
        {
            // This block is executed regardless of whether an exception occurred
            // Example of cleanup action or final logging
            _logger.LogInformation("RunAsync method execution finished.");

            // Additional cleanup actions can go here (if any)
        }
    }

}
