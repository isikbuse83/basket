using System;
using ConsoleApp1.Application.Mapping;
using ConsoleApp1.Application.Services;
using ConsoleApp1.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// 🔧 Bağlantı cümlesi appsettings.json'dan alınıyor
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Servis kayıtları
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<RabbitMQPublisher>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<BasketService>();
builder.Services.AddAutoMapper(typeof(MappingProfile));


// DbContext kayıt
builder.Services.AddDbContext<BasketDbContext>(options =>
    options.UseSqlServer(connectionString));

//auto mapper projenin tamamına eklensin diye 
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/swagger");
        return;
    }
    await next();
});

app.UseAuthorization();
app.MapControllers();
await app.RunAsync();