using Microsoft.Extensions.Logging;
using ZenonFileProcessor.Models;

namespace ZenonFileProcessor.Services
{
    public class Processor
    {
        private readonly ILogger<Processor> _logger;

        // Constructor to initialize the logger for the Processor class
        public Processor(ILogger<Processor> logger)
        {
            _logger = logger; // Assign the logger instance to the class
        }

        // Asynchronous method to calculate the min and max values for each variable
        public async Task<List<string>> GetMinMaxResultsAsync(List<ChannelVariable> variables, List<MeasurementRecord> values)
        {
            // Check if the variables list is null or empty
            if (variables is null || !variables.Any())
            {
                _logger.LogError("Variable list is empty or null."); // Log the error
                throw new ArgumentException("Variable list is empty or null."); // Throw an exception
            }

            // Check if the values list is null or empty
            if (values is null || !values.Any())
            {
                _logger.LogError("Value list is empty or null."); // Log the error
                throw new ArgumentException("Value list is empty or null."); // Throw an exception
            }

            var results = new List<string>(); // Initialize a list to store the result strings

            // Loop through each variable in the variables list
            foreach (var variable in variables)
            {
                // Filter the values based on the variable's ChannelIndex
                var variableValues = values.Where(v => v.ChannelIndex == variable.ChannelIndex).ToList();

                // Check if there are any values for the current variable
                if (variableValues.Any())
                {
                    // Find the record with the minimum value
                    var minRecord = variableValues.MinBy(v => v.Value);
                    // Find the record with the maximum value
                    var maxRecord = variableValues.MaxBy(v => v.Value);

                    // Add the result in the required format to the results list
                    results.Add($"{variable.VariableId};{minRecord.Value};{minRecord.Timestamp:dd.MM.yyyy HH:mm:ss.fff};" +
                                $"{maxRecord.Value};{maxRecord.Timestamp:dd.MM.yyyy HH:mm:ss.fff}");
                }
            }

            // Log the number of results processed
            _logger.LogInformation("Processed {ResultCount} results.", results.Count);

            return await Task.FromResult(results); // Return the results asynchronously
        }
    }
}
