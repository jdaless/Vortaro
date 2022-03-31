public class Fonto
{
    public Guid Id {get; set;}
    public bool ĈuUzantkreita {get; set;}

    public int Favoreco {get; set;}

    public string? Ligilo{get; set;}

    public string? Titolo{get; set;}
    public string? KreintoId {get; set;}

    public Uzanto? Kreinto{get; set;}
    public List<Voĉdono> Voĉdonoj {get; set;} = new();

    public string? Signo {get; set;}
}