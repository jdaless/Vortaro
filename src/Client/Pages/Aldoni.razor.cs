using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Text.Json;

namespace vortaro.Client.Pages;

public sealed partial class Aldoni
{
    [Inject] APIServo APIServo {get; set; } = null!;
    [Inject] AuthenticationStateProvider AuthenticationStateProvider {get; set; }  = null!;
    [Inject] NavigationManager NavigationManager {get;set;} = null!;

    List<string> validajFinaĵoj = new()
    {
        "-a",
        "-o",
        "-e",
        "-i"
    };

    string Vortspeco 
    {
        get => vortspeco; 
        set
        {
            if(value == "bazvorton")
            {
                teksto = null;
            }
            else if (value == "kunmetaĵon")
            {
                bazvortoj = new();
            }
            vortspeco = value;
        }
    }
    string vortspeco = string.Empty;

    List<Vorto> bazvortoj = new();

    Guid? finaĵo;
    string? teksto;

    string serĉfrazo = string.Empty;
    
    List<Vorto?>? finaĵoj;
    List<Vorto>? bazvortElektoj;

    protected override async Task OnInitializedAsync()
    {
        var vortElektoj = await APIServo.APIPeto<List<Vorto>>(
                $"vorto/{Uri.EscapeDataString("^.$")}");
        finaĵoj = vortElektoj.Where(v => v.ĈuFinaĵo && validajFinaĵoj.Any(s => s == v.Teksto!)).Prepend(null).ToList();
        await InvokeAsync(StateHasChanged);
    }

    async Task Serĉi(string s)
    {        
        serĉfrazo = s;
        if(s != string.Empty)
        {
            var v = await APIServo.APIPeto<List<Vorto>>($"vorto/{Uri.EscapeDataString(s)}");
            if(s == serĉfrazo)
                bazvortElektoj = v.Where(v => v.ĈuRadiko).ToList(); 
        }
        else
            bazvortElektoj = new();
    }

    async Task AldonuVorton()
    {
        if(teksto == string.Empty 
            || (teksto is null && !bazvortoj.Any()))
            return;
        var vorto = new Vorto()
        {
            Teksto = teksto,
            FinaĵoId = finaĵo,
            Radikoj = bazvortoj.Select((v,i) => new Radiko()
            {
                RadikaVortoId = v.Id,
                Ordo = i 
            }).ToList(),
            Fonto = new()
            {
                ĈuUzantkreita = true,
                KreintoId = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.FindFirst("sub")!.Value,
                Signo = "*",
            }
        };
        var v = await APIServo.APIPostAuth("vorto",vorto);
        NavigationManager.NavigateTo($"/vorto/{v.Id}");
    }

}