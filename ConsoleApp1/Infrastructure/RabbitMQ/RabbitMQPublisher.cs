using System.Text;
using System.Text.Json;
using AutoMapper;
using ConsoleApp1.Domain.Entities;
using ConsoleApp1.DTOs.Response;
using RabbitMQ.Client;

public class RabbitMQPublisher
{
    private readonly IMapper _mapper;
    private const string QueueName = "order-queue";

    public RabbitMQPublisher(IMapper mapper)
    {
        _mapper = mapper;
    }

    public void PublishOrder(Order order)
    {
        // DTO'ya dönüştür
        var orderDto = _mapper.Map<OrderResponse>(order);

        // JSON'a çevir
        var json = JsonSerializer.Serialize(orderDto);
        var body = Encoding.UTF8.GetBytes(json);

        // RabbitMQ'ya gönder
        var factory = new ConnectionFactory { HostName = "localhost" };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

        channel.BasicPublish(
            exchange: "",
            routingKey: QueueName,
            basicProperties: null,
            body: body
        );
    }
}