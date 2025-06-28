using System.Text;
using System.Text.Json;
using Steeltoe.Discovery;
using Utilisateur.Dtos;

namespace Utilisateur.Services;

public class LogService
{
    private readonly HttpClient _httpClient;
    private readonly IDiscoveryClient _discoveryClient;

    public LogService(HttpClient httpClient, IDiscoveryClient discoveryClient)
    {
        _httpClient = httpClient;
        _discoveryClient = discoveryClient;
    }

    public async Task SendLogAsync(string message, string source, string ipPort, string code)
    {
        try
        {
            var instances = _discoveryClient.GetInstances("historique-service");
            if (instances.Count == 0)
            {
                Console.WriteLine("Service Historique introuvable.");
                return;
            }

            var baseUri = instances[0].Uri.ToString().TrimEnd('/');

            var log = new LogDto
            {
                IdLog = Guid.NewGuid(),
                Message = message,
                Source = source,
                IpPort = ipPort,
                Code = code
            };
            
            var jsonContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(log), System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{baseUri}/api/logs", jsonContent);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de l'envoi du log : {ex.Message}");
        }
    }
}