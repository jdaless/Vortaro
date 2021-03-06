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
        vorto = await APIServo.APIPeto<Vorto>($"vorto/{Id}");
        var l = new List<Radiko>();
        await foreach(var r in APIServo.ŜarguRadikojn(vorto!))
        {
            l.Add(r);
        }
        vorto.Radikoj = l;

        _ = Task.Run(async ()=>
        {
            lingvoj = await APIServo.APIPeto<List<Lingvo>>($"lingvo");
            await InvokeAsync(StateHasChanged);
        });   
            
        _ = Task.Run(async ()=>
        {    
            if(vorto!.FinaĵoId is not null)
                vorto.Finaĵo = await APIServo.APIPeto<Vorto>($"vorto/{vorto.FinaĵoId}");
            await InvokeAsync(StateHasChanged);
        });

        _ = Task.Run(async ()=>
        {                         
            fontoj = (await APIServo.APIPeto<List<Fonto>>($"fonto"))
                    .ToDictionary(f=>f.Id, f=>f);
            foreach(var f in fontoj.Values)
                await APIServo.ŜarguFonton(f);
            vorto.Fonto = fontoj[vorto.FontoId];
            await InvokeAsync(StateHasChanged);
        });

        await BreakpointListener.Subscribe(b =>
        {
            Console.WriteLine("size: " + b.ToString() + " " + (int)b);
            malebligiPanelojn = b.CompareTo(MudBlazor.Breakpoint.Md) < 0;
            StateHasChanged();
        });

        malebligiPanelojn = await BreakpointListener.IsMediaSize(MudBlazor.Breakpoint.SmAndDown);

        

        await InvokeAsync(StateHasChanged);
        await base.OnInitializedAsync(); 
    }

    async Task ŜarguDifinojn()
    {
        if(vorto!.Difinoj.Any()) vorto!.Difinoj.Clear();
        await foreach(var d in APIServo.ŜarguDifinojn(vorto!))
        {
            d.Fonto = fontoj![d.FontoId];
            vorto!.Difinoj.Add(d);
            await InvokeAsync(StateHasChanged);
        }
    }

    async Task ŜarguTradukojn()
    {
        if(vorto!.Tradukoj.Any()) vorto!.Tradukoj.Clear();
        await foreach(var t in APIServo.ŜarguTradukojn(vorto!))
        {            
            t.Fonto = fontoj![t.FontoId];
            vorto!.Tradukoj.Add(t);
            await InvokeAsync(StateHasChanged);
        }
    }

    async Task ŜarguEkzemplojn()
    {
        if(vorto!.Ekzemploj.Any()) vorto!.Ekzemploj.Clear();
        await foreach(var e in APIServo.ŜarguEkzemplojn(vorto!))
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

        fonto = await APIServo.APIPostAuth("fonto", fonto);

        var montrEnhavo = new T()
        {
            Teksto = enaĵo,
            Fonto = fonto,
            Vorto = vorto
        };
        
        switch(montrEnhavo)
        {
            case Traduko t:
                t.LingvoId = Lingvo;
                vorto.Tradukoj.Add(t);
                break;
            case Difino d:
                vorto.Difinoj.Add(d);
                break;
            case Ekzemplo e:
                vorto.Ekzemploj.Add(e);
                break;
        }
        await InvokeAsync(StateHasChanged);

        //sendi fonton kaj ricevi novan ID 
        var enhavo = new T()
        {
            Teksto = enaĵo,
            FontoId = fonto.Id,
            VortoId = vorto!.Id
        };
        //sendi enhavon
        await APIServo.APIPostAuth(typeof(T).Name.ToLower(), enhavo);
        fontoj = (await APIServo.APIPeto<List<Fonto>>($"fonto"))
                .ToDictionary(f=>f.Id, f=>f);
        foreach(var f in fontoj.Values)
            await APIServo.ŜarguFonton(f);
        await ŜarguNunanTabon(tabs.ActivePanelIndex);
    }    

    Task ŜarguNunanTabon(int i) =>
        i switch
        {
            1 => ŜarguDifinojn(),
            2 => ŜarguTradukojn(),
            3 => ŜarguEkzemplojn(),
            _ => Task.CompletedTask
        };


    [Authorize]
    async Task Malsuprigu(Enhavo enhavo)
    {
        await APIServo.Voĉdoni(enhavo, false);
        fontoj = (await APIServo.APIPeto<List<Fonto>>($"fonto"))
                .ToDictionary(f=>f.Id, f=>f);
        foreach(var f in fontoj.Values)
            await APIServo.ŜarguFonton(f);
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
            await APIServo.ŜarguFonton(f);
        foreach(var d in vorto!.Difinoj) d.Fonto = fontoj[d.FontoId];
        foreach(var e in vorto!.Ekzemploj) e.Fonto = fontoj[e.FontoId];
        foreach(var t in vorto!.Tradukoj) t.Fonto = fontoj[t.FontoId];
        await InvokeAsync(StateHasChanged);
    }

    [Authorize]
    async Task Forigi(Enhavo enhavo)
    {        
        await APIServo.APIDeleteAuth(enhavo.GetType().Name.ToLower(), enhavo.Id);
        
        await ŜarguNunanTabon(tabs.ActivePanelIndex);
    }


}