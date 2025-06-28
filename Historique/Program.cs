using Historique.Data;
using Historique.Events;
using Microsoft.EntityFrameworkCore;
using Steeltoe.Connector.RabbitMQ;
using Steeltoe.Discovery.Client;
using Steeltoe.Messaging.RabbitMQ.Config;
using Steeltoe.Messaging.RabbitMQ.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuration de la base de données
builder.Services.AddDbContext<HistoriqueDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Eureka
builder.Services.AddDiscoveryClient(builder.Configuration);

// RabbitMQ Steeltoe
builder.Services.AddRabbitMQConnection(builder.Configuration);
builder.Services.AddRabbitServices(true);
builder.Services.AddRabbitAdmin();
builder.Services.AddRabbitTemplate();
builder.Services.AddRabbitExchange("ms.utilisateur", ExchangeType.TOPIC);

// Register le handler et le listener
builder.Services.AddSingleton<UserCreatedEventHandler>();
builder.Services.AddSingleton<UserUpdatedEventHandler>();
builder.Services.AddSingleton<UserDeletedEventHandler>();

builder.Services.AddRabbitListeners<UserCreatedEventHandler>();
builder.Services.AddRabbitListeners<UserUpdatedEventHandler>();
builder.Services.AddRabbitListeners<UserDeletedEventHandler>();

var app = builder.Build();

Console.WriteLine($"RabbitMQ Host: {builder.Configuration["spring:rabbitmq:host"]}");
Console.WriteLine($"RabbitMQ Username: {builder.Configuration["spring:rabbitmq:username"]}");
Console.WriteLine($"RabbitMQ Password: {builder.Configuration["spring:rabbitmq:password"]}");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDiscoveryClient();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();