using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

ConnectionFactory connectionFactory = new ConnectionFactory();

IConnection connection = connectionFactory.CreateConnection();

IChannel channel = await connection.CreateChannelAsync();
#region Queue Declarations
await channel.QueueDeclareAsync(
    queue: "RenderingQueue",
    durable: true,
    exclusive: false,
    autoDelete: false);
#endregion

EventingBasicConsumer consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, eventArgs) =>
{
    var body = eventArgs.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);

    Console.WriteLine($"Rendering: {message}");
};

await channel.BasicConsumeAsync(
    queue: "RenderingQueue",
    autoAck: false,
    consumer);

Console.ReadKey();