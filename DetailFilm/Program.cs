using DetailFilm.Data;
using DetailFilm.Events;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using Steeltoe.Connector.RabbitMQ;
using Steeltoe.Discovery.Client;
using Steeltoe.Messaging.RabbitMQ.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DetailFilmDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Eureka et RabbitMQ Steeltoe
builder.Services.AddDiscoveryClient(builder.Configuration);
builder.Services.AddRabbitMQConnection(builder.Configuration);
builder.Services.AddRabbitServices(true);
builder.Services.AddRabbitAdmin();
builder.Services.AddRabbitTemplate();
builder.Services.AddRabbitExchange("ms.detailfilm", ExchangeType.Topic);

// Enregistrer RabbitMQProducer avec config dynamique
builder.Services.AddSingleton<IRabbitMQProducer, RabbitMQProducer>();

var app = builder.Build();

app.UseDiscoveryClient();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();