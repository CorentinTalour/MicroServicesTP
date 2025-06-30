using Film.Data;
using Film.Events;
using Microsoft.EntityFrameworkCore;
using Steeltoe.Connector.RabbitMQ;
using Steeltoe.Discovery.Client;
using Steeltoe.Messaging.RabbitMQ.Config;
using Steeltoe.Messaging.RabbitMQ.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Configuration CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Connexion PostgreSQL
builder.Services.AddDbContext<FilmsDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Eureka
builder.Services.AddDiscoveryClient(builder.Configuration);

// RabbitMQ Steeltoe (Style Utilisateur)
builder.Services.AddRabbitMQConnection(builder.Configuration);
builder.Services.AddRabbitServices(true);
builder.Services.AddRabbitAdmin();
builder.Services.AddRabbitTemplate();
builder.Services.AddRabbitExchange("ms.film", ExchangeType.TOPIC);

// Event Publisher
builder.Services.AddScoped<EventPublisher>();
builder.Services.AddScoped<IEventPublisher, EventPublisher>();
builder.Services.AddScoped<IRabbitMQProducer, RabbitMQProducer>();

// API Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowAll");

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