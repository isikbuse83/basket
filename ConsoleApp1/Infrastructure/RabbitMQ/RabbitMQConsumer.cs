using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ConsoleApp1.DTOs.Response;

public class RabbitMQConsumerService : BackgroundService
{
    private const string QueueName = "order-queue";
    private readonly ConnectionFactory _factory;

    public RabbitMQConsumerService()
    {
        _factory = new ConnectionFactory { HostName = "localhost" };
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var connection = _factory.CreateConnection();
        var channel = connection.CreateModel();

        channel.QueueDeclare(queue: QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var json = Encoding.UTF8.GetString(ea.Body.ToArray());
            var orderDto = JsonSerializer.Deserialize<OrderResponse>(json);
            Console.WriteLine($"📦 Sipariş: {orderDto.User.UserName} - {orderDto.Product.ProductName} x{orderDto.Quantity}");
        };

        channel.BasicConsume(queue: QueueName, autoAck: true, consumer: consumer);

        Console.WriteLine("🔁 Sipariş kuyruğu dinleniyor...");

        // BackgroundService sonsuz döngüye girmemeli, task olarak çalışmalı
        return Task.CompletedTask;
    }
}