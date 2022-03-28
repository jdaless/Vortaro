using System.Text.Json.Serialization;
public abstract class Enhavo
{
    public Guid Id {get;set;}
    [JsonIgnore]
    public Fonto Fonto {get; set;} = null!;
    public Guid FontoId {get; set;}
    [JsonIgnore]

    public Vorto Vorto {get;set;} = null!;
    public Guid VortoId {get;set;}
    public string Teksto {get; set;} = null!;
}