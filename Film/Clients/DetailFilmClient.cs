using Film.Dtos;
using Steeltoe.Discovery;

namespace Film.Clients;

public class DetailFilmClient
{
    private readonly IDiscoveryClient _discoveryClient;
    private readonly HttpClient _httpClient;

    public DetailFilmClient(IDiscoveryClient discoveryClient, HttpClient httpClient)
    {
        _discoveryClient = discoveryClient;
        _httpClient = httpClient;
    }

    public async Task<DetailFilmDto> GetDetailFilmAsync(Guid id)
    {
        var instances = _discoveryClient.GetInstances("detailfilm-service");
        var instance = instances.FirstOrDefault();

        if (instance == null)
            throw new Exception("Service detailfilm-service not found");

        var url = $"{instance.Uri}api/detailfilm/{id}";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var detailFilm = await response.Content.ReadFromJsonAsync<DetailFilmDto>();
        return detailFilm;
    }
}