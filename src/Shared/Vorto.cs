using System.Text.Json.Serialization;

public class Vorto
{
    public Guid Id {get; set;}
    public string? Teksto {get; set; }
    public Vorto? Finaĵo {get;set;}
    public Guid? FinaĵoId {get; set;}
    
    public List<Radiko> Radikoj {get;set;} = new();
    [JsonIgnore]
    public List<Radiko> Derivaĵoj {get;set;} = new();
    [JsonIgnore]
    public List<Difino> Difinoj {get; set; } = new();
    [JsonIgnore]
    public List<Ekzemplo> Ekzemploj {get; set; } = new();
    [JsonIgnore]
    public List<Traduko> Tradukoj {get; set; } = new();

    public bool ĈuFinaĵo => Teksto is not null && Teksto.StartsWith("-") && !Teksto.EndsWith("-");

    public bool ĈuRadiko => !Radikoj.Any();

    /// Eldonas la tutan vorton. Se tiu ĉi vorto estas bazforto, eldonu la tekston. Alie, aldonu la
    /// kunmetaĵon de la radikoj kaj finaĵo. Ekz. -a, montr/i, kun/met/aĵ/o
    public override string ToString()
    {
        return Teksto is not null
            ? Teksto + (new string(Finaĵo?.ToString().Trim('-').Prepend('/').ToArray()) ?? string.Empty) //bazvortoj kun/sen finaĵo
            : (string.Join("/",Radikoj.OrderBy(r => r.Ordo).Select(r => r.RadikaVorto.Teksto!.Trim('-'))) + "/" + Finaĵo?.ToString().Trim('-')); //ĉiuj el la radikoj

    }
}