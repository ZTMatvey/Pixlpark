using RabbitMQ.Client;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using WebService.Config;

namespace WebService.Register
{
    public sealed class Register
    {
        private const string EMAIL_PATTERN = "^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$";

        private static readonly IConnection s_connection;
        private static readonly IModel s_channel;

        static Register()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            s_connection = factory.CreateConnection();
            s_channel = s_connection.CreateModel();

            s_channel.ExchangeDeclare("dev-pixlpark-ex", ExchangeType.Direct);
        }

        public static IResult New(NewDto newDto)
        {
            if (!CheckEMail(newDto?.EMail ?? string.Empty)) return TypedResults.StatusCode(400);

            var code = Random.Shared.Next(10_000, 99_999);
            var message = $"{newDto!.EMail}|{code}";
            var body = Encoding.UTF8.GetBytes(message);

            SendMessage(newDto!.EMail, code.ToString());

            s_channel.BasicPublish(exchange: "dev-pixlpark-ex", routingKey: "code", body: body);

            return TypedResults.Ok(new Response() { success = 1});
        }

        private static void SendMessage(string toEMail, string code)
        {
            var mailData = MailData.Instance;

            var from = new MailAddress(mailData.Address, "no-reply-pixlpark-by-matvey");
            var to = new MailAddress(toEMail);
            var message = new MailMessage(from, to);
            message.Body = $"Код подтверждения аккаунта Pixlpark: {code}";
            message.Subject = "Подтверждения акканта Pixlpark";

            var smtpClient = new SmtpClient(mailData.SMTP, mailData.Port)
            {
                Credentials = new NetworkCredential(mailData.Address, mailData.Password),
                EnableSsl = true
            };

            smtpClient.Send(message);
        }

        private static bool CheckEMail(string email)
        {
            return Regex.Match(email, EMAIL_PATTERN).Success;
        }

        public static void Dispose()
        {
            s_channel.Dispose();
            s_connection.Dispose();
        }
    }
}
