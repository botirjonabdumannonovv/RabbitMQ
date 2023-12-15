using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

ConnectionFactory connectionFactory = new ConnectionFactory();

IConnection connection = connectionFactory.CreateConnection();

IChannel channel = await connection.CreateChannelAsync();

#region queue declarations
await channel.QueueDeclareAsync(
    queue:"NotificationQueue",
    durable:true,
    exclusive:false,
    autoDelete:false);
#endregion

EventingBasicConsumer consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, eventArgs) =>
{
    var body = eventArgs.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);

    Console.WriteLine($"$Notification: {message}");
};

await channel.BasicConsumeAsync(
    queue:"NotificationQueue",
    autoAck:false,
    consumer);

Console.ReadKey();    