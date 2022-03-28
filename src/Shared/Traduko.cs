using System.Text.Json.Serialization;

public class Traduko : Enhavo
{
    [JsonIgnore]
    public Lingvo Lingvo {get; set;} = null!;
    public string LingvoId {get; set;} = null!;
}