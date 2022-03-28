using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Text.Json;

namespace vortaro.Client.Pages;

public sealed partial class VortoDetail
{
    [Inject] IHttpClientFactory HttpClientFactory {get; set; } = null!;
    [Inject] APIServo APIServo {get; set; } = null!;
    [Inject] AuthenticationStateProvider AuthenticationStateProvider {get; set; }  = null!;

    [Parameter] public Guid Id {get; set;}

    Vorto? vorto;

    JsonSerializerOptions options = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    private Dictionary<Guid,Fonto>? fontoj = null;

    private bool ĉuAldoniEkzemplo = false;
    private string ekzemplo = string.Empty;
    private bool ĉuAldoniDifino = false;
    private string difino = string.Empty;
    private string? ĉuAldoniTraduko = null;
    private string traduko = string.Empty;

    private List<Lingvo>? lingvoj = null;
    protected override Task OnParametersSetAsync() => OnInitializedAsync();

    protected override async Task OnInitializedAsync()
    {  

        _ = Task.Run(async ()=>
            {
                lingvoj = await APIServo.APIPeto<List<Lingvo>>($"lingvo");
                await InvokeAsync(StateHasChanged);
            });                            
        fontoj = (await APIServo.APIPeto<List<Fonto>>($"fonto"))
                .ToDictionary(f=>f.Id, f=>f);
        foreach(var f in fontoj.Values)
            await APIServo.ŜarĝiguFonton(f);


        var v = await APIServo.APIPeto<Vorto>($"vorto/{Id}");

        if(v!.FinaĵoId is not null)
            v.Finaĵo = await APIServo.APIPeto<Vorto>($"vorto/{v.FinaĵoId}");
        
        var radj = new List<Radiko>();
        await foreach(var r in APIServo.ŜarĝiguRadikojn(v!))
        {
            radj.Add(r);
        }
        v.Radikoj = radj.ToList();
        await InvokeAsync(StateHasChanged);
        vorto = v;
        await ŜarĝiguEnhavojn();
        await base.OnInitializedAsync(); 
    }

    async Task ŜarĝiguEnhavojn()
    {
        vorto!.Tradukoj.Clear();
        await foreach(var t in APIServo.ŜarĝiguTradukojn(vorto!))
        {            
            t.Fonto = fontoj![t.FontoId];
            vorto!.Tradukoj.Add(t);
            await InvokeAsync(StateHasChanged);
        }
        vorto!.Difinoj.Clear();
        await foreach(var d in APIServo.ŜarĝiguDifinojn(vorto!))
        {
            d.Fonto = fontoj![d.FontoId];
            vorto!.Difinoj.Add(d);
            await InvokeAsync(StateHasChanged);
        }
        vorto!.Ekzemploj.Clear();
        await foreach(var e in APIServo.ŜarĝiguEkzemplojn(vorto!))
        {
            e.Fonto = fontoj![e.FontoId];
            vorto!.Ekzemploj.Add(e);
            await InvokeAsync(StateHasChanged);
        }

    }

    string RadikaString(string s)
    {
        if(s.EndsWith('-'))
        {
            return s[..^1] + "/";
        }
        if(s.StartsWith('-'))
        {
            return s;
        }
        return s + "/";
    }

    [Authorize]
    async Task SendiEnhavo<T>(string enaĵo, string? ling = null) where T : Enhavo, new()
    {
        if(string.IsNullOrEmpty(enaĵo)) return;
        var fonto = new Fonto()
        {
            ĈuUzantkreita = true,
            KreintoId = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.FindFirst("sub")!.Value
        };
        //sendi fonton kaj ricevi novan ID 
        fonto = await APIServo.APIPostAuth("fonto", fonto);
        var enhavo = new T()
        {
            Teksto = enaĵo,
            FontoId = fonto.Id,
            VortoId = vorto!.Id
        };
        if(enhavo is Traduko t) t.LingvoId = ling!;
        //sendi enhavon
        Console.WriteLine(JsonSerializer.Serialize(enhavo));
        await APIServo.APIPostAuth(typeof(T).Name.ToLower(), enhavo);
        ĉuAldoniDifino = false;
        difino = string.Empty;
        ĉuAldoniEkzemplo = false;
        ekzemplo = string.Empty;
        ĉuAldoniTraduko =  null;
        ekzemplo= string.Empty;
        fontoj = (await APIServo.APIPeto<List<Fonto>>($"fonto"))
                .ToDictionary(f=>f.Id, f=>f);
        foreach(var f in fontoj.Values)
            await APIServo.ŜarĝiguFonton(f);
        await ŜarĝiguEnhavojn();
    }

    [Authorize]
    async Task Malsuprigu(Enhavo enhavo)
    {
        await APIServo.Voĉdoni(enhavo, false);
        fontoj = (await APIServo.APIPeto<List<Fonto>>($"fonto"))
                .ToDictionary(f=>f.Id, f=>f);
        foreach(var f in fontoj.Values)
            await APIServo.ŜarĝiguFonton(f);
        foreach(var d in vorto!.Difinoj) d.Fonto = fontoj[d.FontoId];
        foreach(var e in vorto!.Ekzemploj) e.Fonto = fontoj[e.FontoId];
        foreach(var t in vorto!.Tradukoj) t.Fonto = fontoj[t.FontoId];
        await InvokeAsync(StateHasChanged);
    }

    [Authorize]
    async Task Suprigu(Enhavo enhavo)
    {
        await APIServo.Voĉdoni(enhavo, true);
        fontoj = (await APIServo.APIPeto<List<Fonto>>($"fonto"))
                .ToDictionary(f=>f.Id, f=>f);
        foreach(var f in fontoj.Values)
            await APIServo.ŜarĝiguFonton(f);
        foreach(var d in vorto!.Difinoj) d.Fonto = fontoj[d.FontoId];
        foreach(var e in vorto!.Ekzemploj) e.Fonto = fontoj[e.FontoId];
        foreach(var t in vorto!.Tradukoj) t.Fonto = fontoj[t.FontoId];
        await InvokeAsync(StateHasChanged);
    }


}