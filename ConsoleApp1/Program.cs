using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ConsoleApp1.Data;

var builder = WebApplication.CreateBuilder(args);


var connectionString = "Server=LAPTOP-6PECIQ5E\\SQLEXPRESS;Database=Basket;Trusted_Connection=True;TrustServerCertificate=True;";

// Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<BasketDb>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

// Swagger routing 
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/swagger");
        return;
    }
    await next();
});

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.Run();

