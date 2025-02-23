using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using ZenonFileProcessor.Models;
using ZenonFileProcessor.Services;

namespace ZenonFileProcessor.Tests
{
    // Used IAsyncDisposable to ensure file cleanup even if the test fails.
    public class FileParserTests : IAsyncDisposable
    {
        private readonly string _filePath = "testfile.txt";

        [Fact]
        public async Task ParseFileAsync_ValidFile_ReturnsCorrectData()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<FileParser>>();
            var parser = new FileParser(loggerMock.Object);

            await File.WriteAllLinesAsync(_filePath, new[]
            {
                "-VARIABLES-",
                "@1=100",
                "@2=200",
                "-VALUES-",
                "@1:10.5;123;01.01.2023 12:00:00.000",
                "@2:20.5;456;02.01.2023 12:00:00.000"
            });

            // Act
            await parser.ParseFileAsync(_filePath);

            // Assert
            Assert.Equal(2, parser.Variables.Count);
            Assert.Equal(2, parser.Values.Count);

            // Variable assertions
            Assert.Equal(1, parser.Variables[0].ChannelIndex);
            Assert.Equal(100, parser.Variables[0].VariableId);
            Assert.Equal(2, parser.Variables[1].ChannelIndex);
            Assert.Equal(200, parser.Variables[1].VariableId);

            // Value assertions
            Assert.Equal(1, parser.Values[0].ChannelIndex);
            Assert.Equal(10.5, parser.Values[0].Value);
            Assert.Equal(123, parser.Values[0].Status);
            Assert.Equal(DateTime.ParseExact("01.01.2023 12:00:00.000", "dd.MM.yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture), parser.Values[0].Timestamp);

            Assert.Equal(2, parser.Values[1].ChannelIndex);
            Assert.Equal(20.5, parser.Values[1].Value);
            Assert.Equal(456, parser.Values[1].Status);
            Assert.Equal(DateTime.ParseExact("02.01.2023 12:00:00.000", "dd.MM.yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture), parser.Values[1].Timestamp);
        }

        [Fact]
        public async Task ParseFileAsync_FileNotFound_ThrowsFileNotFoundException()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<FileParser>>();
            var parser = new FileParser(loggerMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<FileNotFoundException>(() => parser.ParseFileAsync("nonexistent.txt"));
        }

        public async ValueTask DisposeAsync()
        {
            if (File.Exists(_filePath))
                await Task.Run(() => File.Delete(_filePath));
        }
    }
}