#!/bin/bash
# Source environment variables from .env files
if [ -f ../../.env ]; then
    source ../../.env
fi

# Build connection string from environment variables
export ConnectionStrings__DefaultConnection="Host=localhost;Port=${POSTGRES_PORT};Database=${POSTGRES_DB};Username=${POSTGRES_USER};Password=${POSTGRES_PASSWORD};"

# Run the database update command
echo "Updating database with environment variables"
echo "Using DB: ${POSTGRES_DB}, User: ${POSTGRES_USER}, Port: ${POSTGRES_PORT}"
cd ..  # Go up one directory to the project root
dotnet ef database update
