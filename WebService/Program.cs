using RabbitMQ.Client;
using System.Text;
using System.Threading.Channels;
using WebService.Config;
using WebService.Register;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();
var fromMailData = app.Configuration.GetSection("FromMailData");
var mailData = MailData.Instance;
fromMailData.Bind(mailData);

app.MapPost("/register/new", (NewDto newDto) => Register.New(newDto));

app.Lifetime.ApplicationStopping.Register(Register.Dispose);

app.Run();