using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory() { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.ExchangeDeclare("dev-pixlpark-ex", ExchangeType.Direct);
var queueName = channel.QueueDeclare(durable: false, exclusive: false).QueueName;

channel.QueueBind(queue: queueName,
    exchange: "dev-pixlpark-ex",
    routingKey: "code");

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, e) =>
{
    var body = e.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"[{DateTime.UtcNow}] {message}");
};

channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
Console.ReadLine();