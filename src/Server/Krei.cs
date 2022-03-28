using CsvHelper;

public static class Krei
{
    public static void KreiDatumbazon(IEnumerable<CsvModel> records, VortaroContext context)
    {

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        Console.WriteLine("kreante...");
        

        Lingvo eo = null!;
        Lingvo en = null!;
        Lingvo fr = null!;
        Lingvo de = null!;
        Lingvo pl = null!;
        Lingvo ru = null!;
        Lingvo zh = null!;

        context.AddRange(
                eo = new Lingvo() { Id = "eo",Nomo="Esperanto" },
                en = new Lingvo() { Id = "en",Nomo="La Angla (English)" },
                fr = new Lingvo() { Id = "fr",Nomo="La Franca (Française)" },
                de = new Lingvo() { Id = "de",Nomo="La Germana (Deutsch)" },
                pl = new Lingvo() { Id = "pl",Nomo="La Pola (Polski)" },
                ru = new Lingvo() { Id = "ru",Nomo="La Rusa (Русский)" },
                zh = new Lingvo() { Id = "zh",Nomo="La Ĉina (中文)" }
            );
        
        Fonto fundamento = null!;
        Fonto akademiaKorekto = null!;

        context.AddRange(
                fundamento = new Fonto() 
                { 
                    ĈuUzantkreita = false, 
                    Favoreco = int.MaxValue-1, 
                    Ligilo = "https://www.akademio-de-esperanto.org/fundamento/universala_vortaro.html", 
                    Titolo = "Fundamento de Esperanto",
                },
                akademiaKorekto = new Fonto() 
                { 
                    ĈuUzantkreita = false, 
                    Favoreco = int.MaxValue, 
                    Ligilo = "https://www.akademio-de-esperanto.org/akademia_vortaro/klarigoj.html", 
                    Titolo = "Akademiaj Korektoj de la Universala Vortaro",
                }
            );
        Vorto oFinaĵo = new()
        {
            Id = Guid.NewGuid(),
            Teksto="-o",
        };        
        Vorto aFinaĵo = new()
        {
            Id = Guid.NewGuid(),
            Teksto="-a",
        };
        Vorto eFinaĵo = new()
        {
            Id = Guid.NewGuid(),
            Teksto="-e",
        };
        Vorto iFinaĵo = new()
        {
            Id = Guid.NewGuid(),
            Teksto="-i",
        };
        Vorto jFinaĵo = new()
        {
            Id = Guid.NewGuid(),
            Teksto="-j",
        };
        Vorto nFinaĵo = new()
        {
            Id = Guid.NewGuid(),
            Teksto="-n",
        };
        Vorto isFinaĵo = new()
        {
            Id = Guid.NewGuid(),
            Teksto="-is",
        };
        Vorto asFinaĵo = new()
        {
            Id = Guid.NewGuid(),
            Teksto="-as",
        };
        Vorto osFinaĵo = new()
        {
            Id = Guid.NewGuid(),
            Teksto="-os",
        };
        Vorto usFinaĵo = new()
        {
            Id = Guid.NewGuid(),
            Teksto="-us",
        };
        Vorto uFinaĵo = new()
        {
            Id = Guid.NewGuid(),
            Teksto="-u",
        };

        context.AddRange(
                aFinaĵo,
                asFinaĵo,
                eFinaĵo,
                iFinaĵo,
                isFinaĵo,
                jFinaĵo,
                nFinaĵo,
                oFinaĵo,
                osFinaĵo,
                uFinaĵo,
                usFinaĵo
            );

        var derivaĵoj = new List<(CsvModel, Vorto)>();
        foreach(var e in records)
        {
            e.Elemento = e.Elemento.Split(' ').First();
            if(e.Elemento.StartsWith('-') && !e.Elemento.EndsWith('-'))
                continue;

            var teksto = e.Elemento.Trim('/').Count(c=>c=='/') > 0
                    ? null
                    : e.Elemento.Trim('/').ToLower();

            var v = context.Vortoj.FirstOrDefault(vort => vort.Teksto == teksto) is Vorto vorto 
                ? vorto 
                : new Vorto
                {
                    FinaĵoId = e.Bazformo.StartsWith('-') || e.Bazformo.Length == 1
                        ? null
                        : e.Bazformo.Split(" ").First().Split('/').Last() switch 
                        {
                            "o" => oFinaĵo.Id,
                            "a" => aFinaĵo.Id,
                            "i" => iFinaĵo.Id,
                            "e" => eFinaĵo.Id,
                            "j" => jFinaĵo.Id,
                            _ => null
                        },
                    Teksto = teksto,
                    
                };

            if(!string.IsNullOrEmpty(e.France) )
                v.Tradukoj = new()
                {
                    new ()
                    {
                        FontoId = e.France.EndsWith("AK") ? akademiaKorekto.Id : fundamento.Id,
                        LingvoId = fr.Id,
                        Teksto = e.France.EndsWith("AK") ? e.France[..^2] : e.France
                    },
                    new()
                    {
                        FontoId = e.Angle.EndsWith("AK") ? akademiaKorekto.Id : fundamento.Id,
                        LingvoId = en.Id,
                        Teksto = e.Angle.EndsWith("AK") ? e.Angle[..^2] : e.Angle
                    },
                    new()
                    {
                        FontoId = e.Germane.EndsWith("AK") ? akademiaKorekto.Id : fundamento.Id,
                        LingvoId = de.Id,
                        Teksto = e.Germane.EndsWith("AK") ? e.Germane[..^2] : e.Germane
                    },
                    new()
                    {
                        FontoId = e.Pole.EndsWith("AK") ? akademiaKorekto.Id : fundamento.Id,
                        LingvoId = pl.Id,
                        Teksto = e.Pole.EndsWith("AK") ? e.Pole[..^2] : e.Pole
                    },
                    new()
                    {
                        FontoId = e.Ruse.EndsWith("AK") ? akademiaKorekto.Id : fundamento.Id,
                        LingvoId = ru.Id,
                        Teksto = e.Ruse.EndsWith("AK") ? e.Ruse[..^2] : e.Ruse
                    },
                };

            if(!string.IsNullOrEmpty(e.OficialaDifino))
                v.Difinoj = new()
                {
                    new()
                    {
                        FontoId = fundamento.Id,
                        Teksto=e.OficialaDifino
                    }
                };
            
            if(v.Teksto != null)
                context.Add(v);
            else
                derivaĵoj.Add((e,v));
        }
        context.SaveChanges();

        foreach(var (e,v) in derivaĵoj)
        {
            var bazvortoj = e.Elemento.Trim().ToLower().Split('/').Where(s => !string.IsNullOrEmpty(s)).ToList();
            Console.Write("\n"+ e.Elemento.Trim().ToLower() + " ");
            foreach(var b in bazvortoj)
                Console.Write(b + ", ");
            var tuples = bazvortoj
                .Select((b,num) => 
                    Tuple.Create(
                        Prompt(
                            context.Vortoj.Where(v=> v.Teksto != null && b == v.Teksto.Trim('-') && !v.Difinoj.Any()),
                            v=>
                            {
                                context.Entry(v).Reference(v=>v.Finaĵo).Load();
                                return v.Teksto+v.Finaĵo?.Teksto;
                            }
                        ),
                        num))
                .ToArray();
            foreach(var (r,ordo) in tuples[..^1])
            {
                v.Radikoj.Add(new Radiko{RadikaVortoId = r.Id, Ordo=ordo});
            }
            if(!tuples[^1].Item1.ĈuFinaĵo)
                v.Radikoj.Add(new Radiko{RadikaVortoId = tuples[^1].Item1.Id, Ordo=tuples[^1].Item2});
            context.Add(v);
        }
        context.SaveChanges();

        Console.WriteLine("kreinte!");
    }

    static T Prompt<T>(IEnumerable<T> options, Func<T,string> display)
    {
        var l = options.ToList();
        if(l.Count == 1) return l[0];
        if(l.Count == 0) throw new ArgumentException();
        Console.WriteLine();
        for(int i=0;i<l.Count;i++)
        {
            Console.WriteLine(i+". "+display(l[i]));
        }
        Console.Write("> ");
        return l[int.Parse(Console.ReadLine()!)];
    }
}