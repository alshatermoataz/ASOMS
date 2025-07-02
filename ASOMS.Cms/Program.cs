using ASOMS.Cms.Services.Auth;
using ASOMS.Cms.Services.Interfaces;
using ASOMS.Cms.Services.OrderServices;
using ASOMS.Cms.Services.OrdersInterface;
using ASOMS.Cms.Services.ProductServices;
using ASOMS.Core.Interfaces;
using ASOMS.DAL.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string[] allowedOrigins = new[]
{
    "http://localhost:5173",
    "https://asoms.vercel.app"
};

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});


builder.Services.AddControllers();
// Register SignalR
builder.Services.AddSignalR();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderServices>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<CustomDbContext>(options =>
    options.UseNpgsql(connectionString));


//builder.Services.AddDbContext<CustomDbContext>(options =>
//    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddDbContext<CustomDbContext>(options =>
//    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
//           .EnableSensitiveDataLogging()
//           .LogTo(Console.WriteLine, LogLevel.Information));



var app = builder.Build();

app.UseCors("AllowFrontend");


// Serve wwwroot
app.UseStaticFiles();

// Serve uploads
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "wwwroot", "uploads")),
    RequestPath = "/uploads"
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
// Map the SignalR hub endpoint
app.MapHub<ASOMS.Cms.Services.NotificationHub>("/hubs/notifications");


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CustomDbContext>();
    DataSeeder.Seed(db);
}


app.Run();
