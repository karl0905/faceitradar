#!/bin/bash
# Source environment variables from .env files
if [ -f ../../.env ]; then
    source ../../.env
fi
if [ -f ../../db/.env ]; then
    source ../../db/.env
fi

# Build connection string from environment variables
export ConnectionStrings__DefaultConnection="Host=localhost;Port=${POSTGRES_PORT};Database=${POSTGRES_DB};Username=${POSTGRES_USER};Password=${POSTGRES_PASSWORD};"

# Run the migration command with the name passed as an argument
echo "Adding migration: $1"
echo "Using DB: ${POSTGRES_DB}, User: ${POSTGRES_USER}, Port: ${POSTGRES_PORT}"
cd ..  # Go up one directory to the project root
dotnet ef migrations add $1
