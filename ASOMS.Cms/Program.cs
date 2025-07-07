using ASOMS.Cms.Services.Auth;
using ASOMS.Cms.Services.Interfaces;
using ASOMS.Cms.Services.OrderServices;
using ASOMS.Cms.Services.OrdersInterface;
using ASOMS.Cms.Services.ProductServices;
using ASOMS.Core.Interfaces;
using ASOMS.DAL.EntityFramework;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// CORS allowed origins
string[] allowedOrigins = new[]
{
    "http://localhost:5173",
    "https://asoms.vercel.app"
};

// Define a consistent CORS policy name
const string CorsPolicyName = "AllowFrontend";

// Register CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicyName, policy =>
    {
        policy
            .WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// Register controllers and Swagger
builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderServices>();

// Configure database context
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<CustomDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

// Apply CORS middleware
app.UseCors(CorsPolicyName);

// Serve static files from wwwroot
app.UseStaticFiles();

// Serve files from wwwroot/uploads via /uploads path
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "wwwroot", "uploads")),
    RequestPath = "/uploads"
});

// Enable Swagger only in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable HTTPS redirection and authorization
app.UseHttpsRedirection();
app.UseAuthorization();

// Map controller endpoints and SignalR hub
app.MapControllers();
app.MapHub<ASOMS.Cms.Services.NotificationHub>("/hubs/notifications");

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;

        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogError(exception, "Unhandled exception occurred.");

        context.Response.StatusCode = 500;
        await context.Response.WriteAsync("Internal server error");
    });
});


// Seed database on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CustomDbContext>();
    DataSeeder.Seed(db);
}

app.Run();
