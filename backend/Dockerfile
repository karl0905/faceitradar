# Production Dockerfile for FaceItRadar Backend
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["faceitradar.csproj", "./"]
RUN dotnet restore "faceitradar.csproj"

# Copy the rest of the source code
COPY . .

# Build and publish the application
RUN dotnet publish "faceitradar.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Build the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Set environment variables
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

# Copy the published application
COPY --from=build /app/publish .

# Expose port 80
EXPOSE 80

# Set the entry point for the container
ENTRYPOINT ["dotnet", "faceitradar.dll"]
