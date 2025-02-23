
# Zenon Data Processing Tool

## Task Description & File Processing

### Overview
The task involves processing a text file (`A1.TXT`) containing historical variable values saved by the Zenon system. The file is divided into three primary sections:
1. **`-HEADER-`**: Metadata, including the file name, start, and end timestamps.
2. **`-VARIABLES-`**: Mapping between channel indices and variable IDs.
3. **`-VALUES-`**: Historical data for each variable, containing values, statuses, and timestamps.

The primary objective is to:
- Parse the input file.
- For each variable, compute the minimum and maximum values along with their respective timestamps.
- Output the results in the following format:
  <Variable ID>;<Min Value>;<Min Timestamp>;<Max Value>;<Max Timestamp>

---

### File Structure

1. **Input File (`A1.TXT`)**:
   - Contains structured historical data.
   - Sections:
     - `-HEADER-`: File metadata.
     - `-VARIABLES-`: Mapping of channel indices to variable IDs.
     - `-VALUES-`: Historical data for each channel index.
   
2. **Output File**:
   - The result will list the min and max values for each variable, with corresponding timestamps.
   - The output format is as follows:
     <Variable ID>;<Min Value>;<Min Timestamp>;<Max Value>;<Max Timestamp>

---

## Build & Run Instructions

### Prerequisites
- **.NET Core SDK** (version 8.0 or later) installed.
- A code editor (e.g., Visual Studio or Visual Studio Code).

### Steps to Build & Run

1. **Clone or Download the Code**:
   - Clone the repository using Git:
     git clone https://github.com/HazemSalim/zenon-processing.git
   - Alternatively, download the source code as a ZIP file.

2. **Open the Project**:
   - Open the project in your preferred code editor (e.g., Visual Studio or Visual Studio Code).

3. **Build the Project**:
   - Navigate to the project directory in the terminal and run:
     dotnet build

4. **Run the Program**:
   - After building, run the program with the input file as an argument:
     dotnet run -- <path-to-A1.TXT>
   - Example:
     dotnet run -- data/A1.TXT

5. **Output**:
   - The program will generate an output file (e.g., `output.txt`) in the same directory as the input file.

---

## Running Tests

### Prerequisites
- **.NET Core SDK** (version 8.0 or later) installed.
- **xUnit** (included in the project).

### Steps to Run Tests

1. Navigate to the test project directory:
   cd ZenonFileProcessor.Tests

2. Run the tests using:
   dotnet test

### Test Cases

The following test cases are included:

- **FileParserTests**:
  - `ParseFileAsync_ValidFile_ReturnsCorrectData`: Verifies correct parsing of a valid file.
  - `ParseFileAsync_FileNotFound_ThrowsFileNotFoundException`: Ensures correct handling of a missing file.
  
- **ProcessorTests**:
  - `GetMinMaxResultsAsync_ValidData_ReturnsCorrectResults`: Verifies min/max value calculations.
  - `GetMinMaxResultsAsync_NoVariables_ThrowsArgumentException`: Tests handling of an empty variables list.
  - `GetMinMaxResultsAsync_NoValues_ThrowsArgumentException`: Ensures empty values list is handled properly.
  - `GetMinMaxResultsAsync_SingleValue_ReturnsSameMinMax`: Verifies behavior for a single value.

---

## Additional Information

### Error Handling
- The program gracefully handles common errors, such as missing files or invalid data formats, with errors being logged either to the console or to a log file.

### Portability
- Built in **C# using .NET Core**, the program is cross-platform and runs on **Windows**, **Linux**, and **macOS**.

### Performance
- The program is optimized to handle large input files efficiently.

### Logging
- Detailed logging is provided to aid in debugging and monitoring.

---

## Key Features

- **Input Parsing**: Efficient parsing of the `-HEADER-`, `-VARIABLES-`, and `-VALUES-` sections.
- **Min/Max Calculation**: Accurate computation of the min and max values for each variable.
- **Output Generation**: Generates a formatted output file with the required data.
- **Error Handling**: Robust handling of invalid data and missing files.
- **Cross-Platform**: Runs on multiple operating systems.
- **Unit Testing**: Comprehensive tests ensure the correctness of parsing and processing.

---

## Importance of This Task

### Data Analysis
The program simplifies the analysis of historical data, helping identify trends, anomalies, and key metrics (such as min/max values).

### Automation
It automates the extraction and summarization of data, saving time and reducing the need for manual processing.

### Scalability
The program efficiently handles large datasets, making it ideal for industrial applications.

### Portability
Being cross-platform, the program can be deployed in various environments, ensuring versatility.

### Error Handling
Its robust error handling ensures reliability in production environments.

---

## Conclusion
The Zenon Data Processing Tool is a powerful, efficient, and scalable solution for processing and analyzing historical data. Its cross-platform compatibility and comprehensive features make it ideal for a wide range of applications, from industrial data processing to data-driven decision-making. By automating data analysis tasks, it can help save time, improve accuracy, and enhance decision-making.
