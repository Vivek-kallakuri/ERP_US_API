using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using TimeSheetAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200") // Replace with your Angular app URL
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Add Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TimeSheet API", Version = "v1" });
});

// Register DbContext with SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Build the application
var app = builder.Build();

// Configure the HTTP request pipeline for development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TimeSheet API v1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the root URL
    });
}

app.UseCors("AllowAngularApp");

app.UseHttpsRedirection();

app.UseAuthorization();

// Map controllers
app.MapControllers();

// Run the application
app.Run();
