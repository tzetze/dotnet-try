using System.Text.Json.Serialization;
using TaskManager.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// --- Register services in the DI (dependency injection) container ---

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Serialize enums as their string name ("High") instead of a number (2).
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Tell the container: whenever something needs an ITaskService, give it a
// single shared InMemoryTaskService (Singleton = one instance for the whole app).
builder.Services.AddSingleton<ITaskService, InMemoryTaskService>();

builder.Services.AddOpenApi();

// CORS: browsers block a page on http://localhost:4200 (Angular) from calling
// an API on a different port unless the API explicitly allows it.
const string AngularCors = "AllowAngular";
builder.Services.AddCors(options =>
{
    options.AddPolicy(AngularCors, policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

// --- Configure the HTTP request pipeline (order matters) ---

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors(AngularCors);
app.UseAuthorization();
app.MapControllers();

app.Run();
