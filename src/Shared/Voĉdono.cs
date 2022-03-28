using System.Text.Json.Serialization;

public class Voĉdono
{
    public Guid Id {get; set; }
    public string UzantoId {get; set;} = null!;
    [JsonIgnore]
    public Uzanto Uzanto {get; set;} = null!;
    public Guid EnhavoId {get; set;}
    [JsonIgnore]
    public Enhavo Enhavo {get; set;} = null!;
    public bool ĈuSupraPoento {get; set; }
}