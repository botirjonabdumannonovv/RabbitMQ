using System.Text;
using RabbitMQ.Client;

ConnectionFactory connectionFactory = new ConnectionFactory();

IConnection connection = connectionFactory.CreateConnection();

IChannel channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync(
    exchange: "test",
    type: ExchangeType.Fanout,
    durable: true,
    autoDelete: false);

#region Queue Declaration
await channel.QueueDeclareAsync(
    queue: "NotificationQueue",
    durable: true,
    exclusive: false,
    autoDelete: false);

await channel.QueueDeclareAsync(
    queue: "VerificationQueue",
    durable: true,
    exclusive: false,
    autoDelete: false);

await channel.QueueDeclareAsync(
    queue: "RenderingQueue",
    durable: true,
    exclusive: false,
    autoDelete: false);
#endregion

#region Queue Bind
await channel.QueueBindAsync(
    queue: "NotificationQueue",
    exchange: "test",
    routingKey: "key1");

await channel.QueueBindAsync(
    queue: "VerificationQueue",
    exchange: "test",
    routingKey: "key2");

await channel.QueueBindAsync(
    queue: "RenderingQueue",
    exchange: "test",
    routingKey: "key3");
#endregion

string message = "Let's test direct exchange with same routing key";

byte[] body = Encoding.UTF8.GetBytes(message);

await channel.BasicPublishAsync(
    exchange: "test",
    routingKey: "key",
    body);

Console.ReadKey();