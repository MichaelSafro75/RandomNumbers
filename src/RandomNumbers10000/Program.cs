using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RandomNumbers10000.OutputFormatters;
using RandomNumbers10000.Services;


const int TotalNumbers = 10_000;
const int MinValue = 1;
const int MaxValue = 10_000;


// Setup dependency injection
var services = new ServiceCollection();
ConfigureServices(services);
var serviceProvider = services.BuildServiceProvider();

// Get the logger
var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

try
{
    logger.LogInformation("Application started - Random Numbers Generator");
    DisplayWelcomeMessage();

    // Get user input for output format
    var outputFormatter = SelectOutputFormat(serviceProvider);
    if (outputFormatter == null)
    {
        Console.WriteLine("\n❌ No valid output format selected. Exiting.");
        Environment.Exit(1);
    }

    // Get output path for file-based formatters
    var outputPath = outputFormatter.FormatName != "Console"
        ? await serviceProvider.GetRequiredService<IOutputPathProvider>().GetOutputPathAsync()
        : string.Empty;

    // Generate the random numbers
    Console.WriteLine();
    Console.WriteLine("Generating random numbers...");
    var stopwatch = System.Diagnostics.Stopwatch.StartNew();

    var randomNumberGenerator = serviceProvider.GetRequiredService<IRandomNumberGenerator>();
    var randomNumbers = randomNumberGenerator.GenerateUniqueRandomNumbers(TotalNumbers, MinValue, MaxValue);

    stopwatch.Stop();
    logger.LogInformation("Generated {Count} numbers in {ElapsedMilliseconds}ms", randomNumbers.Count, stopwatch.ElapsedMilliseconds);

    // Format and output the numbers
    await outputFormatter.FormatAndOutputAsync(randomNumbers, outputPath);

    logger.LogInformation("Application completed successfully in {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
    Console.WriteLine($"✓ Total execution time: {stopwatch.ElapsedMilliseconds}ms");
}
catch (Exception ex)
{
    logger.LogError(ex, "An unexpected error occurred");
    Console.WriteLine($"\n❌ Error: {ex.Message}");
    Environment.Exit(1);
}


/// <summary>
/// Configures the dependency injection container.
/// </summary>
/// <param name="services">The service collection to configure.</param>
static void ConfigureServices(IServiceCollection services)
{
    // Logging

    services.AddLogging(config =>
    {
        config.AddConsole();
        config.SetMinimumLevel(LogLevel.Information);
    });


    // Services
    services.AddSingleton<IRandomNumberGenerator, RandomNumberGenerator>();
    services.AddSingleton<IOutputPathProvider, OutputPathProvider>();


    // Output formatters
    services.AddSingleton<ConsoleOutputFormatter>();
    services.AddSingleton<CsvFileOutputFormatter>();
}


/// <summary>
/// Displays a welcome message to the user.
/// </summary>
static void DisplayWelcomeMessage()
{
    Console.WriteLine();
    Console.WriteLine("╔═══════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║         🎲 Random Numbers Generator - Version 1.0             ║");
    Console.WriteLine("╚═══════════════════════════════════════════════════════════════╝");
    Console.WriteLine();
    Console.WriteLine($"This application will generate {TotalNumbers:N0} unique random numbers");
    Console.WriteLine($"in the range from {MinValue:N0} to {MaxValue:N0}.");
    Console.WriteLine();
}


/// <summary>
/// Prompts the user to select an output format.
/// </summary>
/// <param name="serviceProvider">The service provider for resolving formatters.</param>
/// <returns>The selected output formatter, or null if no valid selection was made.</returns>
static IRandomNumberOutputFormatter? SelectOutputFormat(IServiceProvider serviceProvider)
{
    Console.WriteLine("═══════════════════════════════════════════════════════════════");
    Console.WriteLine("📊 Output Format Selection");
    Console.WriteLine("═══════════════════════════════════════════════════════════════");
    Console.WriteLine();

    Console.WriteLine("Choose how you would like to receive the results:");
    Console.WriteLine("  1. Console (display on screen)");
    Console.WriteLine("  2. CSV File (with timestamp: RandomNumbers_YYYY-MM-DD_HH-MM-SS.csv)");
    Console.WriteLine();
    Console.Write("Enter your choice (1, or 2): ");

    var choice = Console.ReadLine()?.Trim();


    return choice switch
    {
        "1" => serviceProvider.GetRequiredService<ConsoleOutputFormatter>(),
        "2" => serviceProvider.GetRequiredService<CsvFileOutputFormatter>(),
        _ => null
    };
}

