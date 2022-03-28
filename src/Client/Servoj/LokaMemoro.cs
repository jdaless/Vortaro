using System.Text.Json;
using Microsoft.JSInterop;

public class LokaMemoro
{
    private IJSRuntime _jsRuntime;

    public LokaMemoro(IJSRuntime jSRuntime) => _jsRuntime = jSRuntime;

    public void Meti<T>(string aĵo, T datumo)
    {       
        _ = _jsRuntime.InvokeVoidAsync("localStorage.setItem",aĵo,datumo);
    }

    public async Task<bool?> PreniBool(string aĵo)
    {
        var r =  await _jsRuntime.InvokeAsync<JsonElement>("localStorage.getItem",aĵo);
        return r.ValueKind is JsonValueKind.Null ? null : bool.Parse(r.Deserialize<string>()!);
    }
    
    public ValueTask<string?> PreniString(string aĵo) => 
        _jsRuntime.InvokeAsync<string?>("localStorage.getItem",aĵo);
}