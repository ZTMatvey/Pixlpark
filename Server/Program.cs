using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Server.Config;

var factory = new ConnectionFactory() { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.ExchangeDeclare("dev-pixlpark-ex", ExchangeType.Direct);
var queueName = channel.QueueDeclare(durable: false, exclusive: false).QueueName;

channel.QueueBind(queue: queueName, exchange: "dev-pixlpark-ex", routingKey: "code");

var mailData = MailData.Instance;
mailData.SMTP = "smtp.gmail.com";
mailData.Port = 587;
mailData.Address = "radchenkom864@gmail.com";
mailData.Password = "clbcuoaoxhpzurks";
SendMessage("zenoteper@icloud.com", "123");

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, e) =>
{
    var body = e.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);

    var match = Regex.Match(message, "(.+)\\|(.+)");
    if (match.Groups.Count < 3) return;

    var email = match.Groups[1].Value;
    var code = match.Groups[2].Value;

    Console.WriteLine($"[{DateTime.UtcNow}] {email} code: {code}");

    SendMessage(email, code.ToString());
};

static void SendMessage(string toEMail, string code)
{
    var mailData = MailData.Instance;

    var from = new MailAddress(mailData.Address, "no-reply-pixlpark-by-matvey");
    var to = new MailAddress(toEMail);
    using var message = new MailMessage(from, to);
    message.Body = $"Код подтверждения аккаунта Pixlpark: {code}";
    message.Subject = "Подтверждение аккаунта Pixlpark";

    using var smtpClient = new SmtpClient(mailData.SMTP, mailData.Port);
    smtpClient.Credentials = new NetworkCredential(mailData.Address, mailData.Password);
    smtpClient.EnableSsl = true;

    smtpClient.Send(message);
}

channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
Console.ReadLine();