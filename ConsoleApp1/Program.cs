using System;
using ConsoleApp1.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using ConsoleApp1.Infrastructure.Services;
using ConsoleApp1.Services;
using DbContext = ConsoleApp1.Data.DbContext;

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
builder.Services.AddDbContext<DbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Swagger root yönlendirmesi
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/swagger");
        return;
    }
    await next();
});

// Swagger sadece geliştirme ortamında aktif
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();