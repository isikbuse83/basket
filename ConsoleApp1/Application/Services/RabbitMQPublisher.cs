using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using ConsoleApp1.Domain;

using IModel = RabbitMQ.Client.IModel;

namespace ConsoleApp1.Infrastructure.Services
{
    public class RabbitMQPublisher
    {
        private const string QueueName = "order-queue";

        public void PublishOrder(Order order)
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            
            DeclareQueue(channel);

            byte[] messageBody = SerializeOrder(order);

            channel.BasicPublish(
                exchange: "",
                routingKey: QueueName,
                basicProperties: null,
                body: messageBody
            );
        }

        private void DeclareQueue(IModel channel)
        {
            channel.QueueDeclare(
                queue: QueueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
        }

        private byte[] SerializeOrder(Order order)
        {
            var json = JsonSerializer.Serialize(order);
            return Encoding.UTF8.GetBytes(json);
        }
    }
}
