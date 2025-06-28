using Microsoft.EntityFrameworkCore;
using Steeltoe.Connector.RabbitMQ;
using Steeltoe.Discovery.Client;
using Steeltoe.Messaging.RabbitMQ.Config;
using Steeltoe.Messaging.RabbitMQ.Extensions;
using Utilisateur.Data;
using Utilisateur.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuration de la base de données
builder.Services.AddDbContext<UtilisateurDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Eureka
builder.Services.AddDiscoveryClient(builder.Configuration);

// RabbitMQ Steeltoe
builder.Services.AddRabbitMQConnection(builder.Configuration);
builder.Services.AddRabbitServices(true);
builder.Services.AddRabbitAdmin();
builder.Services.AddRabbitTemplate();
builder.Services.AddRabbitExchange("ms.utilisateur", ExchangeType.TOPIC);

// Event Publisher
builder.Services.AddSingleton<EventPublisher>();
builder.Services.AddSingleton<IRabbitMQProducer, RabbitMQProducer>();
builder.Services.AddScoped<IEventPublisher, EventPublisher>();

var app = builder.Build();

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