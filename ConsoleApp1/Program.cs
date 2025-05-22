using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<BasketDb>(options =>
    options.UseInMemoryDatabase("BusketDb"));

var app = builder.Build();

// Swagger yönlendirme middleware
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
