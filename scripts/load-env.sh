#!/bin/bash
# Load Environment Variables from .env file
# Usage: source ./scripts/load-env.sh

ENV_FILE=".env"

if [ ! -f "$ENV_FILE" ]; then
    echo "❌ File .env not found. Please create it from env.example"
    echo "💡 Copy command: cp env.example .env"
    return 1 2>/dev/null || exit 1
fi

echo "🔄 Loading environment variables from $ENV_FILE..."

# Load variables and export them
set -a
source "$ENV_FILE"
set +a

echo "✅ Environment variables loaded successfully!"
echo "💡 You can now run: dotnet run --project src/Blazor" 