using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Channels;
using WebService.Config;
using WebService.Register;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();

var app = builder.Build();

app.UseCors(builder =>
{
    builder.AllowAnyOrigin();
    builder.AllowAnyHeader();
    builder.AllowAnyMethod();
    //builder.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod();
});

app.UseHttpsRedirection();

var fromMailData = app.Configuration.GetSection("FromMailData");
var mailData = MailData.Instance;
fromMailData.Bind(mailData);

app.MapPost("/register/new", (NewDto newDto) => Register.New(newDto));

app.Lifetime.ApplicationStopping.Register(Register.Dispose);

app.Run();