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
        private static readonly List<Tuple<string, string>> _pairs = new List<Tuple<string, string>>();

        static Register()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            s_connection = factory.CreateConnection();
            s_channel = s_connection.CreateModel();

            s_channel.ExchangeDeclare("dev-pixlpark-ex", ExchangeType.Direct);
        }

        public static IResult New(NewDto newDto)
        {
            if (_pairs.Any(e => e.Item1 == newDto.EMail)) return TypedResults.Ok(new Response() { success = 1 });

            if (!CheckEMail(newDto?.EMail ?? string.Empty)) return TypedResults.StatusCode(400);

            var code = Random.Shared.Next(10_000, 99_999);
            var message = $"{newDto!.EMail}|{code}";
            var body = Encoding.UTF8.GetBytes(message);

            s_channel.BasicPublish(exchange: "dev-pixlpark-ex", routingKey: "code", body: body);

            return TypedResults.Ok(new Response() { success = 1 });
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
