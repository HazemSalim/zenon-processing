using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Xunit;
using ZenonFileProcessor.Models;
using ZenonFileProcessor.Services;

namespace ZenonFileProcessor.Tests
{
    public class ProcessorTests
    {
        [Fact]
        public async Task GetMinMaxResultsAsync_ValidData_ReturnsCorrectResults()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<Processor>>();
            var processor = new Processor(loggerMock.Object);

            var variables = new List<ChannelVariable>
            {
                new ChannelVariable { ChannelIndex = 1, VariableId = 100 }
            };

            var values = new List<MeasurementRecord>
            {
                new MeasurementRecord { ChannelIndex = 1, Value = 10.5, Timestamp = DateTime.ParseExact("01.01.2023 12:00:00.000", "dd.MM.yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture) },
                new MeasurementRecord { ChannelIndex = 1, Value = 20.5, Timestamp = DateTime.ParseExact("02.01.2023 12:00:00.000", "dd.MM.yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture) }
            };

            // Act
            var results = await processor.GetMinMaxResultsAsync(variables, values);

            // Assert
            Assert.Single(results);
            Assert.Equal("100;10.5;01.01.2023 12:00:00.000;20.5;02.01.2023 12:00:00.000", results[0]);
        }

        [Fact]
        public async Task GetMinMaxResultsAsync_NoVariables_ThrowsArgumentException()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<Processor>>();
            var processor = new Processor(loggerMock.Object);

            var variables = new List<ChannelVariable>();
            var values = new List<MeasurementRecord>
            {
                new MeasurementRecord { ChannelIndex = 1, Value = 10.5, Timestamp = DateTime.Now }
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => processor.GetMinMaxResultsAsync(variables, values));
        }

        [Fact]
        public async Task GetMinMaxResultsAsync_NoValues_ThrowsArgumentException()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<Processor>>();
            var processor = new Processor(loggerMock.Object);

            var variables = new List<ChannelVariable>
            {
                new ChannelVariable { ChannelIndex = 1, VariableId = 100 }
            };
            var values = new List<MeasurementRecord>();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => processor.GetMinMaxResultsAsync(variables, values));
        }

        [Fact]
        public async Task GetMinMaxResultsAsync_SingleValue_ReturnsSameMinMax()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<Processor>>();
            var processor = new Processor(loggerMock.Object);

            var variables = new List<ChannelVariable>
            {
                new ChannelVariable { ChannelIndex = 1, VariableId = 100 }
            };

            var values = new List<MeasurementRecord>
            {
                new MeasurementRecord { ChannelIndex = 1, Value = 10.5, Timestamp = DateTime.ParseExact("01.01.2023 12:00:00.000", "dd.MM.yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture) }
            };

            // Act
            var results = await processor.GetMinMaxResultsAsync(variables, values);

            // Assert
            Assert.Single(results);
            Assert.Equal("100;10.5;01.01.2023 12:00:00.000;10.5;01.01.2023 12:00:00.000", results[0]);
        }
    }
}