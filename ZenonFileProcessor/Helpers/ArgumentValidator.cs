namespace ZenonFileProcessor.Helpers
{
    public static class ArgumentValidator
    {
        // Validates the command-line arguments to ensure a single input file path is provided and the file exists
        public static bool Validate(string[] args, out string inputFilePath)
        {
            inputFilePath = string.Empty;

            if (args.Length != 1)
            {
                Console.WriteLine("Error: No input file provided.");
                Console.WriteLine("Usage: ZenonFileProcessor <inputFilePath>");
                return false;
            }

            inputFilePath = args[0];
            if (!File.Exists(inputFilePath))
            {
                Console.WriteLine($"Error: File '{inputFilePath}' not found.");
                return false;
            }

            return true;
        }
    }
}
