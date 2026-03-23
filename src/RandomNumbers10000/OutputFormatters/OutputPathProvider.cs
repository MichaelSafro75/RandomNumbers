namespace RandomNumbers10000.OutputFormatters;

using Microsoft.Extensions.Logging;

/// <summary>
/// Provides and validates output paths for file operations with user interaction.
/// </summary>
public class OutputPathProvider : IOutputPathProvider
{
    private readonly ILogger<OutputPathProvider> _logger;


    /// <summary>
    /// The default output directory relative to the current working directory.
    /// </summary>
    private const string DefaultOutputDirectory = "output";


    /// <summary>
    /// Initializes a new instance of the <see cref="OutputPathProvider"/> class.
    /// </summary>
    /// <param name="logger">The logger instance for diagnostic logging.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="logger"/> is null.</exception>
    public OutputPathProvider(ILogger<OutputPathProvider> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }


    /// <inheritdoc />
    public Task<string> GetOutputPathAsync(CancellationToken cancellationToken = default)
    {
        Console.WriteLine();
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.WriteLine("📁 Output Location");
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.WriteLine();

        var defaultPath = GetDefaultOutputPath();
        Console.WriteLine($"Default output folder: {defaultPath}");
        Console.WriteLine();
        Console.WriteLine("Options:");
        Console.WriteLine("  1. Use default location (" + defaultPath + ")");
        Console.WriteLine("  2. Specify a custom location");
        Console.WriteLine();
        Console.Write("Enter your choice (1 or 2): ");

        var choice = Console.ReadLine()?.Trim();

        var outputPath = choice == "2" ? GetCustomPath() : defaultPath;

        _logger.LogInformation("Output path selected: {OutputPath}", outputPath);

        return Task.FromResult(outputPath);
    }


    /// <summary>
    /// Gets the default output directory path, creating it if it doesn't exist.
    /// </summary>
    /// <returns>The absolute path to the default output directory.</returns>
    private static string GetDefaultOutputPath()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), DefaultOutputDirectory);

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        return path;
    }


    /// <summary>
    /// Prompts the user for a custom output path and validates it.
    /// </summary>
    /// <returns>A valid custom output path.</returns>
    private string GetCustomPath()
    {
        while (true)
        {
            Console.WriteLine();
            Console.Write("Enter the full path where you want to save the file: ");
            var customPath = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(customPath))
            {
                Console.WriteLine("❌ Path cannot be empty. Please try again.");
                continue;
            }

            try
            {
                // Expand environment variables (e.g., %USERPROFILE%)
                var expandedPath = Environment.ExpandEnvironmentVariables(customPath);

                // Check if the directory exists
                if (!Directory.Exists(expandedPath))
                {
                    Console.WriteLine($"❌ The directory does not exist: {expandedPath}");
                    Console.Write("Would you like to create it? (yes/no): ");
                    var createChoice = Console.ReadLine()?.Trim().ToLowerInvariant();

                    if (createChoice == "yes" || createChoice == "y")
                    {
                        Directory.CreateDirectory(expandedPath);
                        _logger.LogInformation("Created directory: {DirectoryPath}", expandedPath);
                        Console.WriteLine($"✓ Directory created: {expandedPath}");
                        return expandedPath;
                    }

                    continue;
                }

                // Verify we have write permissions by attempting to check for a file
                try
                {
                    var testFile = Path.Combine(expandedPath, ".write_test");
                    File.WriteAllText(testFile, string.Empty);
                    File.Delete(testFile);
                }
                catch (UnauthorizedAccessException ex)
                {
                    _logger.LogError(ex, "No write permission for path: {DirectoryPath}", expandedPath);
                    Console.WriteLine($"❌ No write permission for this directory: {expandedPath}");
                    continue;
                }

                _logger.LogInformation("Custom output path validated: {DirectoryPath}", expandedPath);
                Console.WriteLine($"✓ Path validated: {expandedPath}");
                return expandedPath;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Invalid path provided: {CustomPath}", customPath);
                Console.WriteLine($"❌ Invalid path: {ex.Message}");
                continue;
            }
        }
    }
}

