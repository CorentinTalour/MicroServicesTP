using Film.Clients;
using Film.Data;
using Film.Events;
using Microsoft.EntityFrameworkCore;
using Steeltoe.Connector.RabbitMQ;
using Steeltoe.Discovery;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Eureka;
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

builder.Services.AddHttpClient<DetailFilmClient>((sp, client) =>
{
    var discoveryClient = sp.GetRequiredService<IDiscoveryClient>();
    var instance = discoveryClient.GetInstances("detailfilm-service").FirstOrDefault();
    if (instance == null) throw new Exception("DetailFilm service not found");
    client.BaseAddress = instance.Uri;
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

builder.Services.AddHttpClient<DetailFilmClient>();
builder.Services.AddDiscoveryClient(builder.Configuration);

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