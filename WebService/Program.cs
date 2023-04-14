using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Channels;
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

app.MapPost("/register/new", (NewDto newDto) => Register.New(newDto));
app.MapPost("/register/Activate", (CodeDto codeDto) => Register.Activate(codeDto));

app.Lifetime.ApplicationStopping.Register(Register.Dispose);

app.Run();