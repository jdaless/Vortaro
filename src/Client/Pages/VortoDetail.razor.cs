using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Text.Json;
using MudBlazor.Services;

namespace vortaro.Client.Pages;

public sealed partial class VortoDetail
{
    [Inject] APIServo APIServo {get; set; } = null!;
    [Inject] AuthenticationStateProvider AuthenticationStateProvider {get; set; }  = null!;

    [Inject] IBreakpointService BreakpointListener { get; set; } = null!;
    [Inject] NavigationManager NavigationManager {get; set; }  = null!;
    [CascadingParameter] string Lingvo {get; set;} = null!;
    [Parameter] public Guid Id {get; set;}

    Vorto? vorto;

    MudBlazor.MudTabs tabs = null!;

    JsonSerializerOptions options = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    private Dictionary<Guid,Fonto>? fontoj = null;

    private List<Lingvo>? lingvoj = null;
    protected override Task OnParametersSetAsync() => OnInitializedAsync();

    bool malebligiPanelojn = true;

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

        await BreakpointListener.Subscribe(b =>
        {
            Console.WriteLine("size: " + b.ToString() + " " + (int)b);
            malebligiPanelojn = b.CompareTo(MudBlazor.Breakpoint.Md) < 0;
            StateHasChanged();
        });
        malebligiPanelojn = await BreakpointListener.IsMediaSize(MudBlazor.Breakpoint.SmAndDown);

        var v = await APIServo.APIPeto<Vorto>($"vorto/{Id}");
        v.Fonto = fontoj[v.FontoId];
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
        await base.OnInitializedAsync(); 
    }

    async Task ŜarĝiguDifinojn()
    {
        vorto!.Difinoj.Clear();
        await foreach(var d in APIServo.ŜarĝiguDifinojn(vorto!))
        {
            d.Fonto = fontoj![d.FontoId];
            vorto!.Difinoj.Add(d);
            await InvokeAsync(StateHasChanged);
        }
    }

    async Task ŜarĝiguTradukojn()
    {
        vorto!.Tradukoj.Clear();
        await foreach(var t in APIServo.ŜarĝiguTradukojn(vorto!))
        {            
            t.Fonto = fontoj![t.FontoId];
            vorto!.Tradukoj.Add(t);
            await InvokeAsync(StateHasChanged);
        }
    }

    async Task ŜarĝiguEkzemplojn()
    {
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
    async Task SendiEnhavo<T>(string enaĵo) where T : Enhavo, new()
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
        if(enhavo is Traduko t) t.LingvoId = Lingvo;
        //sendi enhavon
        Console.WriteLine(JsonSerializer.Serialize(enhavo));
        await APIServo.APIPostAuth(typeof(T).Name.ToLower(), enhavo);
        fontoj = (await APIServo.APIPeto<List<Fonto>>($"fonto"))
                .ToDictionary(f=>f.Id, f=>f);
        foreach(var f in fontoj.Values)
            await APIServo.ŜarĝiguFonton(f);
        await ŜarĝiguNunanTabon(tabs.ActivePanelIndex);
    }    

    Task ŜarĝiguNunanTabon(int i) =>
        i switch
        {
            1 => ŜarĝiguDifinojn(),
            2 => ŜarĝiguTradukojn(),
            3 => ŜarĝiguEkzemplojn(),
            _ => Task.CompletedTask
        };


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