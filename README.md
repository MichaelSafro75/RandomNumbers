# 🚀 Quick Start Guide - Random Numbers Generator

## Prerequisites

Before you begin, ensure you have:

1. **.NET 10 SDK** installed
   ```bash
   dotnet --version
   # Expected: 10.0.x or higher
   ```

2. **Git** (optional, for cloning)

3. **A text editor or IDE** (VS Code, Visual Studio, JetBrains Rider, etc.)

---

## Getting Started (5 minutes)

### Step 1: Build the Solution

```bash
cd RandomNumbers10000
dotnet build
```

**Expected Output:**
```
Build succeeded.
    0 Warning(s)
    0 Error(s)
```

### Step 2: Run All Tests

```bash
dotnet test
```

**Expected Output:**
```
Test Run Successful.
Total tests: 30
     Passed: 30
     Failed: 0
```

### Step 3: Run the Application

```bash
dotnet run --project src/RandomNumbers10000
```

The application will present an interactive menu:

```
╔═══════════════════════════════════════════════════════════════╗
║         🎲 Random Numbers Generator - Version 1.0             ║
╚═══════════════════════════════════════════════════════════════╝

This application will generate 10,000 unique random numbers
in the range from 1 to 10,000.

═══════════════════════════════════════════════════════════════
📊 Output Format Selection
═══════════════════════════════════════════════════════════════

Choose how you would like to receive the results:
  1. Console (display on screen)
  2. CSV File (with timestamp: RandomNumbers_YYYY-MM-DD_HH-MM-SS.csv)
  3. HTML File (interactive table with sorting & filtering)

Enter your choice (1, 2, or 3): 
```

## Output File Locations

### Console Output
Prints directly to console immediately.

### CSV Output
Default location: `./output/RandomNumbers_YYYY-MM-DD_HH-mm-ss.csv`

Example:
```
./output/RandomNumbers_2026-03-22_14-30-45.csv
```

### HTML Output
Default location: `./output/RandomNumbers_YYYY-MM-DD_HH-mm-ss.html`

Open the HTML file in any modern web browser to view the interactive table with sorting and filtering.

---

## Troubleshooting

### "dotnet: command not found"
- Install .NET 10 SDK: https://dotnet.microsoft.com/download/dotnet
- Verify: `dotnet --version`

### Build errors about Microsoft.Extensions packages
```bash
dotnet nuget locals all --clear
dotnet restore
dotnet build
```

### Cannot write to output directory
1. Select option 1 (default) when prompted - automatically creates `./output/`
2. Or ensure custom directory exists and is writable
3. Or run with elevated permissions if needed

### Tests fail after cloning
```bash
# Clean and rebuild everything
dotnet clean
dotnet restore
dotnet build
dotnet test
```

## Example Usage Scenarios

### Scenario 1: Generate and View in Console

```bash
$ dotnet run --project src/RandomNumbers10000
# Select: 1 (Console)

Output:
═══════════════════════════════════════════════════════════════
Generated 10,000 Unique Random Numbers
═══════════════════════════════════════════════════════════════

📊 Statistics:
  Total Count: 10,000
  Minimum: 1
  Maximum: 10,000
  Sum: 50,005,000
  Average: 5,000.50
...
```

### Scenario 2: Generate CSV for Data Analysis

```bash
$ dotnet run --project src/RandomNumbers10000
# Select: 2 (CSV File)
# Select: 1 (Default output folder)

Output: RandomNumbers_2026-03-22_14-30-45.csv in ./output/
```

Then open in Excel, Python, R, or any analysis tool.

### Scenario 3: Generate Interactive HTML Report

```bash
$ dotnet run --project src/RandomNumbers10000
# Select: 3 (HTML File)
# Select: 1 (Default output folder)

Output: RandomNumbers_2026-03-22_14-30-45.html in ./output/
# Open in browser - click, sort, filter, search, download!
```

---

## Testing

### Run Full Test Suite

```bash
dotnet test
```

### Run Specific Test Class

```bash
dotnet test --filter "RandomNumberGeneratorTests"
```

### Run Single Test Method

```bash
dotnet test --filter "GenerateUniqueRandomNumbers_Generate10000Numbers_Success"
```

## TODO
Add build pipeline


## License

MIT License - See LICENSE file

