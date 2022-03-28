using System.Text.Json.Serialization;

public class Radiko
{
    [JsonIgnore]
    public Guid Id {get; set;}
    [JsonIgnore]
    public Vorto RadikaVorto {get; set;} = null!;
    public Guid RadikaVortoId {get; set;}
    [JsonIgnore]
    public Vorto DerivaĵaVorto {get; set;} = null!;
    public Guid DerivaĵaVortoId {get; set;}
    public int Ordo {get; set;}
}