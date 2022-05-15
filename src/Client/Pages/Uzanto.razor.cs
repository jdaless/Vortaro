using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Text.Json;
using MudBlazor.Services;

namespace vortaro.Client.Pages;

public sealed partial class Uzanto
{
    [Inject] APIServo APIServo {get; set; } = null!;
    [CascadingParameter] string Lingvo {get; set;} = null!;
    [Parameter] public Guid Id {get; set;}
    
    //vortaro.Shared.Uzanto uzanto;

    protected override async Task OnInitializedAsync()
    {  
        //uzanto = APIServo.APIPeto<Uzanto>($"uzanto/{Id}");
    }
}