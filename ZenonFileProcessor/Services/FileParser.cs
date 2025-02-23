using Microsoft.Extensions.Logging;
using System.Globalization;
using ZenonFileProcessor.Models;

namespace ZenonFileProcessor.Services
{
    public class FileParser
    {
        private readonly ILogger<FileParser> _logger;

        // Constructor to initialize the logger for the FileParser class
        public FileParser(ILogger<FileParser> logger)
        {
            _logger = logger; // Assign the logger instance to the class
        }

        // Properties to hold parsed variables and values
        public List<ChannelVariable> Variables { get; private set; } = new();
        public List<MeasurementRecord> Values { get; private set; } = new();

        // Asynchronous method to parse the file and extract variables and values
        public async Task ParseFileAsync(string filePath)
        {
            // Check if the file exists, otherwise log an error and throw an exception
            if (!File.Exists(filePath))
            {
                _logger.LogError("File not found: {FilePath}", filePath);
                throw new FileNotFoundException($"The file '{filePath}' does not exist.");
            }

            try
            {
                using (var reader = new StreamReader(filePath))
                {
                    bool isVariableSection = false;
                    bool isValueSection = false;

                    // Read lines one by one
                    string? line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        // Handle the VARIABLE and VALUES sections as before
                        if (line.StartsWith("-VARIABLES-"))
                        {
                            isVariableSection = true;
                            isValueSection = false;
                            _logger.LogInformation("Started processing VARIABLE section.");
                        }
                        else if (line.StartsWith("-VALUES-"))
                        {
                            isVariableSection = false;
                            isValueSection = true;
                            _logger.LogInformation("Started processing VALUES section.");
                            continue; // Skip to the next line after this marker
                        }

                        // Parse Variables section
                        if (isVariableSection && line.StartsWith("@"))
                        {
                            var parts = line.Split('=');
                            if (parts.Length == 2)
                            {
                                if (int.TryParse(parts[0][1..], out int channelIndex) && int.TryParse(parts[1], out int variableId))
                                {
                                    Variables.Add(new ChannelVariable { ChannelIndex = channelIndex, VariableId = variableId });
                                    _logger.LogDebug("Parsed variable: ChannelIndex = {ChannelIndex}, VariableId = {VariableId}", channelIndex, variableId);
                                }
                                else
                                {
                                    _logger.LogWarning("Invalid format for variable line: {Line}", line);
                                }
                            }
                        }

                        // Parse Values section
                        else if (isValueSection && line.StartsWith("@"))
                        {
                            var parts = line.Split(':', 2);
                            if (parts.Length == 2)
                            {
                                var dataParts = parts[1].Split(';');
                                if (dataParts.Length == 3 &&
                                    int.TryParse(parts[0][1..], out int channelIndex) &&
                                    double.TryParse(dataParts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out double value) &&
                                    long.TryParse(dataParts[1], out long status) &&
                                    DateTime.TryParseExact(dataParts[2].Trim(), "dd.MM.yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime timestamp))
                                {
                                    Values.Add(new MeasurementRecord { ChannelIndex = channelIndex, Value = value, Status = status, Timestamp = timestamp });
                                }
                                else
                                {
                                    _logger.LogWarning("Invalid value format for line: {Line}", line);
                                }
                            }
                            else
                            {
                                _logger.LogWarning("Invalid value line format. Expected exactly one ':' in line: {Line}", line);
                            }
                        }
                    }
                }

                // Check if no values were parsed
                if (!Values.Any())
                {
                    _logger.LogError("No values were parsed from the file: {FilePath}", filePath);
                    throw new ArgumentException("Value list is empty or null.");
                }

                // Log successful parsing
                _logger.LogInformation("Successfully parsed {VariableCount} variables and {ValueCount} values from {FilePath}", Variables.Count, Values.Count, filePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while parsing file: {FilePath}", filePath);
                throw;
            }
        }
    }
}
