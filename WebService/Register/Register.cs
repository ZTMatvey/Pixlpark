using RabbitMQ.Client;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace WebService.Register
{
    public sealed class Register
    {
        private const string EMAIL_PATTERN = "^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$";

        private static readonly IConnection s_connection;
        private static readonly IModel s_channel;
        private static readonly List<Tuple<string, string>> s_pairs = new List<Tuple<string, string>>();

        static Register()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            s_connection = factory.CreateConnection();
            s_channel = s_connection.CreateModel();

            s_channel.ExchangeDeclare("dev-pixlpark-ex", ExchangeType.Direct);
        }

        public static IResult Activate(CodeDto codeDto)
        {
            var pair = s_pairs.FirstOrDefault(e => e.Item1 == codeDto.EMail);
            if (pair == null) return TypedResults.StatusCode(400);

            if (pair.Item2 == codeDto.Code)
            {
                s_pairs.Remove(pair);
                return TypedResults.Ok(new Response() { success = 1 });
            }

            return TypedResults.StatusCode(409);
        }
        public static IResult New(NewDto newDto)
        {
            if (s_pairs.Any(e => e.Item1 == newDto.EMail)) return TypedResults.Ok(new Response() { success = 1 });

            if (!CheckEMail(newDto?.EMail ?? string.Empty)) return TypedResults.StatusCode(400);

            var code = Random.Shared.Next(10_000, 99_999);
            var message = $"{newDto!.EMail}|{code}";
            var body = Encoding.UTF8.GetBytes(message);

            s_pairs.Add(new Tuple<string, string>(newDto!.EMail, code.ToString()));
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
