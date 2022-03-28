using System.Net.Http.Json;
using System.Text.Json;

public class APIServo
{
    JsonSerializerOptions options = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    HttpClient _httpClient;
    HttpClient _authClient;
    public APIServo(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("Unauthenticated");
        _authClient = httpClientFactory.CreateClient("ServerAPI");
    }
    
    public async IAsyncEnumerable<Radiko> ŜarĝiguRadikojn(Vorto v)
    {
        var radikoj = await APIPeto<List<Guid>>($"vorto/{v.Id}/radikoj");

        foreach(var r in v.Radikoj)
        {
            yield return new Radiko()
            {
                Id = r.Id,
                RadikaVortoId = r.RadikaVortoId,
                RadikaVorto = await APIPeto<Vorto>($"vorto/{r.RadikaVortoId}"),
                DerivaĵaVortoId = r.DerivaĵaVortoId,
                DerivaĵaVorto = v,
                Ordo = r.Ordo
            };
        }
    }    

    public async IAsyncEnumerable<Traduko> ŜarĝiguTradukojn(Vorto v)
    {
        foreach(var id in await APIPeto<List<Guid>>($"vorto/{v.Id}/tradukoj"))
            yield return await APIPeto<Traduko>($"traduko/{id}");
    }
    
    public async IAsyncEnumerable<Difino> ŜarĝiguDifinojn(Vorto v)
    {
        foreach(var id in await APIPeto<List<Guid>>($"vorto/{v.Id}/difinoj"))
            yield return await APIPeto<Difino>($"difino/{id}");
    }

    public async IAsyncEnumerable<Ekzemplo> ŜarĝiguEkzemplojn(Vorto v)
    {
        foreach(var id in await APIPeto<List<Guid>>($"vorto/{v.Id}/ekzemploj"))
            yield return await APIPeto<Ekzemplo>($"ekzemplo/{id}");
    }

    public async Task<Fonto> ŜarĝiguFonton(Fonto f)
    {
        if(f.KreintoId is not null)
            f.Kreinto =  await APIPeto<Uzanto>(
                $"uzanto/{System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(f.KreintoId))}");
        return f;
    }

    public async Task<T> APIPeto<T>(string adreso)=>
        (await JsonSerializer.DeserializeAsync<T>(
            await _httpClient.GetStreamAsync($"api/{adreso}"), 
            options))!;

    public async Task<T> APIPost<T>(string adreso, T aĵo) =>
        (await (await _httpClient.PostAsJsonAsync($"api/{adreso}",aĵo)).Content.ReadFromJsonAsync<T>(options: options))!;

        
    public async Task<T> APIPetoAuth<T>(string adreso)=>
        (await JsonSerializer.DeserializeAsync<T>(
            await _authClient.GetStreamAsync($"api/{adreso}"), 
            options))!;

    public async Task<T> APIPostAuth<T>(string adreso, T aĵo) =>
        (await (await _authClient.PostAsJsonAsync($"api/{adreso}",aĵo)).Content.ReadFromJsonAsync<T>(options: options))!;

    public async Task<Voĉdono> Voĉdoni(Enhavo e, bool supre) =>
        (await (await _authClient.PostAsJsonAsync($"api/voĉdono/{e.Id}",supre)).Content.ReadFromJsonAsync<Voĉdono>(options: options))!;

}