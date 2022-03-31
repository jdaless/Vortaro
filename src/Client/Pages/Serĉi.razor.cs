
using Microsoft.AspNetCore.Components;
using System.Diagnostics;
using System.Text.Json;

namespace vortaro.Client.Pages;

public sealed partial class Serĉi
{
    [Inject] NavigationManager NavigationManager {get; set;} = null!;

    [Inject] IHttpClientFactory HttpClientFactory {get; set; } = null!;
    [Inject] LokaMemoro LokaMemoro {get; set; } = null!;
    
    [Inject] APIServo APIServo {get; set; } = null!;

    [Parameter]
    public string Serĉfrazo 
    {
        get => serĉfrazo;
        set => serĉfrazo = value ?? string.Empty;
    }
    private string serĉfrazo = string.Empty;

    
    [Parameter]
    public string Serĉtipo 
    {
        get => serĉtipo;
        set => serĉtipo = value ?? string.Empty;
    }
    private string serĉtipo = string.Empty;

    [Parameter]
    [SupplyParameterFromQuery(Name = "baz")]
    public bool? ĈuMontriBazvortojn 
    {
        get => baz; 
        set 
        {
            if(init & baz != (baz = value ?? baz))  
            {
                LokaMemoro.Meti("baz", baz);
                TraktiElektaŜanĝo();
            }
        } 
    }
    private bool baz = true;

    [Parameter]
    [SupplyParameterFromQuery(Name = "fin")]
    public bool? ĈuMontriFinaĵojn
    {
        get => fin; 
        set 
        {
            if(init & fin != (fin = value ?? fin)) 
            {
                LokaMemoro.Meti("fin", fin);
                TraktiElektaŜanĝo();
            }
        } 
    }
    private bool fin = true;
    

    [Parameter]
    [SupplyParameterFromQuery(Name = "kun")]
    public bool? ĈuMontriKunmetaĵojn
    {
        get => kun; 
        set 
        {
            if(init & kun != (kun = value ?? kun)) 
            {
                LokaMemoro.Meti("kun", kun);
                TraktiElektaŜanĝo();
            }
        } 
    }
    private bool kun = true;
    

    [Parameter]
    [SupplyParameterFromQuery(Name = "dif")]
    public bool? ĈuMontriDifinojn
    {
        get => dif; 
        set 
        {
            if(init & dif != (dif = value ?? dif)) 
            {
                LokaMemoro.Meti("dif", dif);
                TraktiElektaŜanĝo();
            }
        } 
    }
    private bool dif = true;


    [Parameter]
    [SupplyParameterFromQuery(Name = "ekz")]
    public bool? ĈuMontriEkzemplojn
    {
        get => ekz; 
        set 
        {
            if(init & ekz != (ekz = value ?? ekz)) 
            {
                LokaMemoro.Meti("ekz", ekz);
                TraktiElektaŜanĝo();
            }
        } 
    }
    private bool ekz = true;
    

    [Parameter]
    [SupplyParameterFromQuery(Name = "tra")]
    public bool? ĈuMontriTradukojn
    {
        get => tra; 
        set 
        {
            if(init & tra != (tra = value ?? tra)) 
            {
                LokaMemoro.Meti("tra", tra);
                TraktiElektaŜanĝo();
            }
        } 
    }
    private bool tra = true;


    [Parameter]
    [SupplyParameterFromQuery(Name = "lingvo")]
    public string? Lingvo
    {
        get => lingvo; 
        set 
        {
            if(init & lingvo != (lingvo = value ?? lingvo)) 
            {
                LokaMemoro.Meti("lingvo", lingvo);
                TraktiElektaŜanĝo();
            }
        } 
    }
    private string lingvo = "en";

    [Parameter]
    [SupplyParameterFromQuery(Name = "off")]
    public bool? UzuOficialajnFontojn
    {
        get => off; 
        set 
        {
            if(init & off != (off = value ?? off)) 
            {
                LokaMemoro.Meti("off", off);
                TraktiElektaŜanĝo();
            }
        } 
    }
    private bool off = true;

    [Parameter]
    [SupplyParameterFromQuery(Name = "uzf")]
    public bool? UzuUzantajnFontojn
    {
        get => uzf; 
        set 
        {   
            if(init & uzf != (uzf = value ?? uzf)) 
            {
                LokaMemoro.Meti("uzf", uzf);
                TraktiElektaŜanĝo();
            }
        } 
    }
    private bool uzf = true;

    public IEnumerable<string> TrovuStrings 
    {
        get
        {
            var l = new List<string>();
            if(ĈuMontriBazvortojn!.Value) l.Add("bazvortojn");
            if(ĈuMontriFinaĵojn!.Value) l.Add("finaĵojn");
            if(ĈuMontriKunmetaĵojn!.Value) l.Add("kunmetaĵojn");
            return l;
        }
        set
        {
            if(ĈuMontriBazvortojn != value.Contains("bazvortojn"))
                ĈuMontriBazvortojn = value.Contains("bazvortojn");
            if(ĈuMontriFinaĵojn != value.Contains("finaĵojn"))
                ĈuMontriFinaĵojn = value.Contains("finaĵojn");
            if(ĈuMontriKunmetaĵojn != value.Contains("kunmetaĵojn"))
                ĈuMontriKunmetaĵojn = value.Contains("kunmetaĵojn");
        }
    }
    
    public IEnumerable<string> FontoStrings 
    {
        get
        {
            var l = new List<string>();
            if(UzuOficialajnFontojn!.Value) l.Add("oficialaj");
            if(UzuUzantajnFontojn!.Value) l.Add("uzantaj");
            return l;
        }
        set
        {
            if(UzuOficialajnFontojn != value.Contains("oficialaj"))
                UzuOficialajnFontojn = value.Contains("oficialaj");
            if(UzuUzantajnFontojn != value.Contains("uzantaj"))
                UzuUzantajnFontojn = value.Contains("uzantaj");
        }
    }

    JsonSerializerOptions options = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    HttpClient _httpClient = null!;

    private List<Lingvo>? lingvoj = null;

    private Dictionary<Guid,Fonto>? fontoj = null;


    private List<Vorto>? ŜarĝitajVortoj;

    private bool init = false;

    private int tuto = 0;

    private int ŝarĝitaj => ŜarĝitajVortoj?.Count ?? 0;
    private TimeSpan ŝarĝtempo;

    private bool montriĈiujn = false;

    protected override async Task OnInitializedAsync()
    {  
        _httpClient = HttpClientFactory.CreateClient("Unauthenticated");
        _ = ŜarĝiguVortojn();
        _ = Task.Run(async ()=>
            {
                lingvoj = await APIServo.APIPeto<List<Lingvo>>($"lingvo");
                await InvokeAsync(StateHasChanged);
            });                            
        _ = Task.Run(async ()=>
            fontoj = (await APIServo.APIPeto<List<Fonto>>($"fonto"))
                .ToDictionary(f=>f.Id, f=>f));
        _ = Task.Run(async ()=> baz = await LokaMemoro.PreniBool("baz") ?? true);
        _ = Task.Run(async ()=> fin = await LokaMemoro.PreniBool("fin") ?? true);
        _ = Task.Run(async ()=> kun = await LokaMemoro.PreniBool("kun") ?? true);
        _ = Task.Run(async ()=> dif = await LokaMemoro.PreniBool("dif") ?? true);
        _ = Task.Run(async ()=> ekz = await LokaMemoro.PreniBool("ekz") ?? true);
        _ = Task.Run(async ()=> tra = await LokaMemoro.PreniBool("tra") ?? true);
        _ = Task.Run(async ()=> lingvo = await LokaMemoro.PreniString("lingvo") ?? "en");
        _ = Task.Run(async ()=> off = await LokaMemoro.PreniBool("off") ?? true);
        _ = Task.Run(async ()=> uzf = await LokaMemoro.PreniBool("uzf") ?? true);
        await base.OnInitializedAsync(); 
        init = true;
    }

    protected override Task OnParametersSetAsync() => ŜarĝiguVortojn();    

    public void TraktiFrazoŜanĝo(ChangeEventArgs a)
    {
        NavigationManager.NavigateTo(
            PetaFrazo($"/serĉi/{Serĉtipo}/{Uri.EscapeDataString( a.Value!.ToString()!)}")); 
    }
    
    public void TraktiTipoŜanĝo(object a)
    {
        NavigationManager.NavigateTo(
            PetaFrazo($"/serĉi/{a.ToString()!}/{Uri.EscapeDataString(Serĉfrazo)}"));
    }

    public void TraktiElektaŜanĝo()
    {
        NavigationManager.NavigateTo(
            PetaFrazo($"/serĉi/{Serĉtipo}/{Uri.EscapeDataString(Serĉfrazo)}"));
    }

    public async Task ŜarĝiguVortojn()
    {
        var s = new Stopwatch();
        s.Start();
        var cacheSerĉfrazo = Serĉfrazo.ToString();
        
        var nePleneŜarĝigitaj = new List<Vorto>();
        if(Serĉfrazo != string.Empty)
        {
             var v = 
            await APIServo.APIPeto<List<Vorto>>(
                $"{Serĉtipo}/{(Serĉtipo == "traduko" ? Lingvo+"/" : string.Empty)}{Uri.EscapeDataString(Serĉfrazo)}?off={UzuOficialajnFontojn}&uzf={UzuUzantajnFontojn}");

            nePleneŜarĝigitaj = v; 
        }
        tuto = nePleneŜarĝigitaj.Count;       
        await InvokeAsync(StateHasChanged);
        if(nePleneŜarĝigitaj is null) return;
        ŜarĝitajVortoj = new();
        
        foreach(var v in nePleneŜarĝigitaj)
        {
            if(cacheSerĉfrazo != Serĉfrazo) break; 
            if((!ĈuMontriBazvortojn!.Value && v.ĈuRadiko && !v.ĈuFinaĵo)
                || (!ĈuMontriKunmetaĵojn!.Value && !v.ĈuRadiko)
                || (!ĈuMontriFinaĵojn!.Value && v.ĈuFinaĵo))
                {
                    tuto -= 1;
                    continue;
                }

            List<Task> taskoj = new();

            if(v.FinaĵoId is not null)
                taskoj.Add(
                    Task.Run(async()=>v.Finaĵo = await APIServo.APIPeto<Vorto>($"vorto/{v.FinaĵoId}")));

            if(ĈuMontriKunmetaĵojn!.Value)
                taskoj.Add(
                    Task.Run(async()=>
                    {
                        var radj = new List<Radiko>();
                        await foreach(var r in APIServo.ŜarĝiguRadikojn(v))
                        {
                            radj.Add(r);
                        }
                        v.Radikoj = radj.ToList();
                    }));
            var t = await APIServo.UnuajEnhavoj(v, Lingvo!, UzuOficialajnFontojn!.Value, UzuUzantajnFontojn!.Value);
            if(t.t is not null && ĈuMontriTradukojn!.Value) v.Tradukoj.Add(t.t);
            if(t.d is not null && ĈuMontriDifinojn!.Value) v.Difinoj.Add(t.d);
            if(t.e is not null && ĈuMontriEkzemplojn!.Value) v.Ekzemploj.Add(t.e);
            await Task.WhenAll(taskoj);

            // vorto maplenas laŭ la nunaj agordoj
            if(!montriĈiujn && !(v.Tradukoj.Any() || v.Ekzemploj.Any() || v.Difinoj.Any()))
            {
                tuto -= 1;
                continue;
            }
            if(cacheSerĉfrazo != Serĉfrazo) break; 
            if(!ŜarĝitajVortoj.Any(vort => vort.Id == v.Id)) ŜarĝitajVortoj.Add(v);
            ŝarĝtempo = s.Elapsed;
            await InvokeAsync(StateHasChanged);
        }
    }

    public string PetaFrazo(string uri) 
    {
        var elektoj = new Dictionary<string,object?>();

        if(Lingvo is not null && Lingvo != "en") elektoj["lingvo"] = Lingvo;
        if(!(ĈuMontriBazvortojn ?? true)) elektoj["baz"] = false;
        if(!(ĈuMontriFinaĵojn ?? true))  elektoj["fin"] = false; 
        if(!(ĈuMontriKunmetaĵojn ?? true)) elektoj["kun"] = false; 
        if(!(ĈuMontriDifinojn?? true)) elektoj["dif"] = false; 
        if(!(ĈuMontriEkzemplojn ?? true)) elektoj["ekz"] = false; 
        if(!(ĈuMontriTradukojn ?? true)) elektoj["tra"] = false; 
        if(!(UzuOficialajnFontojn ?? true)) elektoj["off"] = false; 
        if(!(UzuUzantajnFontojn ?? true)) elektoj["uzf"] = false; 

        return NavigationManager.GetUriWithQueryParameters(uri, elektoj);
    }
}