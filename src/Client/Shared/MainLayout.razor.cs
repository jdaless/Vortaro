using Microsoft.AspNetCore.Components;
using System.Diagnostics;
using System.Text.Json;
using MudBlazor;
using System.Globalization;

namespace vortaro.Client.Shared;

public partial class MainLayout
{
    [Inject] LokaMemoro LokaMemoro {get; set; } = null!;
    [Inject] APIServo APIServo {get; set; } = null!;
    [Inject] NavigationManager NavigationManager {get; set;} = null!;
    
    public string Lingvo {get; set; } = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
    private Dictionary<string, Lingvo>? lingvoj = null;

    private bool malhela = false;
    private MudThemeProvider _mudThemeProvider = null!;

    private string serĉLing = string.Empty;

    public MainLayout() : base()
    {
        var dark = new MudTheme().PaletteDark;
        dark.Primary = Colors.Green.Darken2;
        dark.Secondary = Colors.Orange.Darken2;
        Theme.PaletteDark = dark;
    }

    protected override async Task OnInitializedAsync()
    {
        lingvoj = (await APIServo.APIPeto<List<Lingvo>>($"lingvo")).Where(l=> l.Id != "eo").ToDictionary(l=>l.Id, l=>l);   
        
        var memLing = await LokaMemoro.PreniString("lingvo");
        if(memLing is not null && memLing != "null") Lingvo = memLing;

        if(!lingvoj.ContainsKey(Lingvo)) Lingvo = "en";

        await base.OnInitializedAsync();
    }
    //protected override void OnParametersSet() => StateHasChanged();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            malhela = await _mudThemeProvider.GetSystemPreference();
            StateHasChanged();
        }
    }

    void ŜanĝiLingvon(string lingvo)
    {
        if(Lingvo != lingvo) 
        {
            Lingvo = lingvo;
            LokaMemoro.Meti("lingvo", Lingvo);
        }      
    }

    private string prepariLingvon(string s) => s[..(s.IndexOf("(")-1)];

    public MudTheme Theme = new()
    {
        Palette = new()
        {
            Primary = Colors.Green.Default,
            Secondary = Colors.Orange.Default
        }
    };
}