using FaceItRadar.Data;
using FaceItRadar.Features.Users;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// Load environment variables
DotNetEnv.Env.Load(".env");

var builder = WebApplication.CreateBuilder(args);

// Register DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var host = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true"
            ? "postgres"  // Use container name when in Docker
            : "localhost";
    var connectionString = $"Host={host};" +
                          $"Port={Environment.GetEnvironmentVariable("DB_PORT")};" +
                          $"Database={Environment.GetEnvironmentVariable("DB_NAME")};" +
                          $"Username={Environment.GetEnvironmentVariable("DB_USER")};" +
                          $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};";
    Console.WriteLine($"Using connection string: {connectionString}");
    options.UseNpgsql(connectionString);
});

// Register user service 
builder.Services.AddScoped<IUserService, UserService>();

// Health check endpoint 
builder.Services.AddHealthChecks();

// Add problem details service
builder.Services.AddProblemDetails();

// Configure controllers with consistent error handling for validation errors
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var firstError = context.ModelState.Values
                .SelectMany(v => v.Errors)
                .FirstOrDefault()?.ErrorMessage;

            var response = new
            {
                status = "error",
                message = firstError ?? "Validation failed",
                errors = context.ModelState
                    .Where(e => e.Value?.Errors.Count > 0)
                    .Select(e => new
                    {
                        field = e.Key,
                        message = e.Value?.Errors.FirstOrDefault()?.ErrorMessage ?? "Unknown error"
                    }).ToList()
            };

            return new BadRequestObjectResult(response);
        };
    });

// Add OpenAPI
builder.Services.AddOpenApi();

var app = builder.Build();

// Add exception handler middleware
app.UseExceptionHandler(appBuilder =>
{
    appBuilder.Run(async context =>
    {
        context.Response.ContentType = "application/json";
        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
        var exception = exceptionHandlerFeature?.Error;

        // Set appropriate status code based on exception type
        context.Response.StatusCode = exception switch
        {
            InvalidOperationException when exception.Message.Contains("already exists") => StatusCodes.Status409Conflict,
            ArgumentException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        // Create consistent error response
        var response = new
        {
            status = "error",
            message = exception?.Message ?? "An unexpected error occurred",
            errors = exception != null
                ? new[]
                {
                    new
                    {
                        field = exception is ArgumentException argEx ? argEx.ParamName ?? "" : "",
                        message = exception.Message
                    }
                }
                : Array.Empty<object>()
        };

        // Serialize and write the response
        await context.Response.WriteAsJsonAsync(response);
    });
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// app.UseHttpsRedirection();  // Commented out for Docker compatibility

// Map endpoints
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
