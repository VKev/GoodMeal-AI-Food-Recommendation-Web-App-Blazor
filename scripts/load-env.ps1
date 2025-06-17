# Load Environment Variables from .env file
# Usage: .\scripts\load-env.ps1

$envFile = ".env"

if (-Not (Test-Path $envFile)) {
    Write-Host "File .env not found. Please create it from env.example" -ForegroundColor Red
    Write-Host "Copy command: cp env.example .env" -ForegroundColor Yellow
    exit 1
}

Write-Host "Loading environment variables from $envFile..." -ForegroundColor Green

Get-Content $envFile | ForEach-Object {
    if ($_ -match "^\s*#" -or $_ -match "^\s*$") {
        # Skip comments and empty lines
        return
    }
    
    $parts = $_ -split "=", 2
    if ($parts.Length -eq 2) {
        $name = $parts[0].Trim()
        $value = $parts[1].Trim()
        
        # Remove quotes if present
        if ($value.StartsWith('"') -and $value.EndsWith('"')) {
            $value = $value.Substring(1, $value.Length - 2)
        }
        
        [Environment]::SetEnvironmentVariable($name, $value, "Process")
        Write-Host "Set $name" -ForegroundColor Gray
    }
}

Write-Host "Environment variables loaded successfully!" -ForegroundColor Green
Write-Host "You can now run: dotnet run --project src/Blazor" -ForegroundColor Yellow 