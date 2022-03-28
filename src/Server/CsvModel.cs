using CsvHelper.Configuration.Attributes;

public class CsvModel
{
    public string Elemento {get; set;} = null!;
    public string Bazformo {get; set;} = null!;
    [Name("Oficiala difino")]
    public string OficialaDifino {get;set;} = null!;
    public string France {get;set;} = null!;
    public string Angle {get;set;} = null!;
    public string Germane {get;set;} = null!;
    public string Pole {get;set;} = null!;
    public string Ruse {get;set;} = null!;

}