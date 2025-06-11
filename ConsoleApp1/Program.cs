using System;
using ConsoleApp1.API.Middlewares;
using ConsoleApp1.Application.Mapping;
using ConsoleApp1.Application.Services;
using ConsoleApp1.DTOs.Request.Validators;
using ConsoleApp1.Infrastructure.Data;
using ConsoleApp1.Test;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// ✅ RabbitMQ Consumer arka plan servisi olarak eklendi
builder.Services.AddHostedService<RabbitMQConsumerService>();

// 🔧 Bağlantı cümlesi appsettings.json'dan alınıyor
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 🎯 FluentValidation + Controllers
builder.Services.AddControllers()
    .AddFluentValidation(fv =>
    {
        fv.RegisterValidatorsFromAssemblyContaining<AddToBasketRequestValidator>();
        fv.AutomaticValidationEnabled = true;
    });

// 🛠️ Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🧩 Servisler
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<RabbitMQPublisher>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<BasketService>();

// 🗺️ AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// 🗃️ DbContext
builder.Services.AddDbContext<BasketDbContext>(options =>
    options.UseSqlServer(connectionString));

// 🌐 Global Hata Yönetimi
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// 🔍 Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 🔁 Ana dizin yönlendirmesi (isteğe bağlı)
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/swagger");
        return;
    }
    await next();
});

app.UseExceptionHandler(); // Global hata middleware
app.UseAuthorization();
app.MapControllers();

UserTest.UserCreateSuccess();

await app.RunAsync();
