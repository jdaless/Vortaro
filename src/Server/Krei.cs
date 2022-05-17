using System.Xml.Linq;
using CsvHelper;

public static class Krei
{
    public static void KreiDatumbazon(IEnumerable<CsvModel> records, VortaroContext context)
    {

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        Console.WriteLine("Kreante...");
        

        Lingvo eo = null!;
        Lingvo en = null!;
        Lingvo fr = null!;
        Lingvo de = null!;
        Lingvo pl = null!;
        Lingvo ru = null!;
        Lingvo es = null!;
        Lingvo it = null!;
        Lingvo ca = null!;
        Lingvo pt = null!;
        Lingvo zh = null!;

        context.AddRange(
            eo = new Lingvo() { Id = "eo",Nomo="Esperanto" },
            en = new Lingvo() { Id = "en",Nomo="La Angla (English)" },
            fr = new Lingvo() { Id = "fr",Nomo="La Franca (Française)" },
            de = new Lingvo() { Id = "de",Nomo="La Germana (Deutsch)" },
            pl = new Lingvo() { Id = "pl",Nomo="La Pola (Polski)" },
            ru = new Lingvo() { Id = "ru",Nomo="La Rusa (Русский)" },
            es = new Lingvo() { Id = "es",Nomo="La Hispana (Español)" },
            it = new Lingvo() { Id = "it",Nomo="La Itala (Italiano)" },
            ca = new Lingvo() { Id = "ca",Nomo="La Kataluna (Català)" },
            pt = new Lingvo() { Id = "pt",Nomo="La Portugala (Português)" },
            zh = new Lingvo() { Id = "zh",Nomo="La Ĉina (中文)" }
        );
        
        Fonto fundamento = null!;
        Fonto akademiaKorekto = null!;
        Fonto unuaAldono = null!;
        Fonto duaAldono = null!;
        Fonto triaAldono = null!;
        Fonto analogAldono = null!;
        Fonto misAldono = null!;
        Fonto kvaraAldono = null!;
        Fonto kvinaAldono = null!;
        Fonto sesaAldono = null!;
        Fonto endAldono = null!;
        Fonto sepaAldono = null!;
        Fonto okaAldono = null!;
        Fonto naŭaAldono = null!;

        context.Fontoj.AddRange(
            fundamento = new() 
            { 
                ĈuUzantkreita = false, 
                Favoreco = int.MaxValue-1, 
                Ligilo = "https://www.akademio-de-esperanto.org/fundamento/universala_vortaro.html", 
                Titolo = "Fundamento de Esperanto",
                Signo = "F",
            },
            akademiaKorekto = new() 
            { 
                ĈuUzantkreita = false, 
                Favoreco = int.MaxValue, 
                Ligilo = "https://www.akademio-de-esperanto.org/akademia_vortaro/klarigoj.html", 
                Titolo = "Akademiaj Korektoj de la Universala Vortaro",
                Signo = "AK",
            },
            unuaAldono = new()
            {
                ĈuUzantkreita = false,
                Favoreco = int.MaxValue - 2,
                Ligilo = "https://www.akademio-de-esperanto.org/akademia_vortaro/klarigoj.html#OA1",
                Titolo = "Unua Oficiala Aldono al Universala Vortaro",
                Signo = "1"
            },
            duaAldono = new()
            {
                ĈuUzantkreita = false,
                Favoreco = int.MaxValue - 3,
                Ligilo = "https://www.akademio-de-esperanto.org/akademia_vortaro/klarigoj.html#OA2",
                Titolo = "Dua Oficiala Aldono al Universala Vortaro",
                Signo = "2"
            },
            triaAldono = new()
            {
                ĈuUzantkreita = false,
                Favoreco = int.MaxValue - 4,
                Ligilo = "https://www.akademio-de-esperanto.org/akademia_vortaro/klarigoj.html#OA3",
                Titolo = "Tria Oficiala Aldono al Universala Vortaro",
                Signo = "3"
            },
            analogAldono = new()
            {
                ĈuUzantkreita = false,
                Favoreco = int.MaxValue - 5,
                Ligilo = "https://www.akademio-de-esperanto.org/akademia_vortaro/klarigoj.html#decido-1923",
                Titolo = "Aldono de la elemento ANALOG/",
                Signo = "*"
            },
            misAldono = new()
            {
                ĈuUzantkreita = false,
                Favoreco = int.MaxValue - 6,
                Ligilo = "https://www.akademio-de-esperanto.org/akademia_vortaro/klarigoj.html#decido-1929",
                Titolo = "Aldono de la elemento MIS/",
                Signo = "*"
            },
            kvaraAldono = new()
            {
                ĈuUzantkreita = false,
                Favoreco = int.MaxValue - 7,
                Ligilo = "https://www.akademio-de-esperanto.org/akademia_vortaro/klarigoj.html#OA4",
                Titolo = "Kvara Aldono al Universala Vortaro",
                Signo = "4"
            },
            kvinaAldono = new()
            {
                ĈuUzantkreita = false,
                Favoreco = int.MaxValue - 8,
                Ligilo = "https://www.akademio-de-esperanto.org/akademia_vortaro/klarigoj.html#OA56",
                Titolo = "Kvina Aldono al Universala Vortaro",
                Signo = "5"
            },
            sesaAldono = new()
            {
                ĈuUzantkreita = false,
                Favoreco = int.MaxValue - 9,
                Ligilo = "https://www.akademio-de-esperanto.org/akademia_vortaro/klarigoj.html#OA56",
                Titolo = "Sesa Aldono al Universala Vortaro",
                Signo = "6"
            },
            endAldono = new()
            {
                ĈuUzantkreita = false,
                Favoreco = int.MaxValue - 10,
                Ligilo = "https://www.akademio-de-esperanto.org/akademia_vortaro/klarigoj.html#end",
                Titolo = "Aldono de la elemento END/",
                Signo = "*"
            },
            sepaAldono = new()
            {
                ĈuUzantkreita = false,
                Favoreco = int.MaxValue - 11,
                Ligilo = "https://www.akademio-de-esperanto.org/akademia_vortaro/klarigoj.html#OA7",
                Titolo = "Sepa Oficiala Aldono al Universala Vortaro",
                Signo = "7"
            },
            okaAldono = new()
            {
                ĈuUzantkreita = false,
                Favoreco = int.MaxValue - 2,
                Ligilo = "https://www.akademio-de-esperanto.org/akademia_vortaro/klarigoj.html#OA8",
                Titolo = "Oka Oficiala Aldono al Universala Vortaro",
                Signo = "8"
            },
            naŭaAldono = new()
            {
                ĈuUzantkreita = false,
                Favoreco = int.MaxValue - 2,
                Ligilo = "https://www.akademio-de-esperanto.org/akademia_vortaro/klarigoj.html#OA9",
                Titolo = "Naŭa Oficiala Aldono al Universala Vortaro",
                Signo = "9"
            }
        );

        context.SaveChanges();

        Vorto oFinaĵo = new()
        {
            Id = Guid.NewGuid(),
            Teksto="-o",
            FontoId=fundamento.Id,
        };        
        Vorto aFinaĵo = new()
        {
            Id = Guid.NewGuid(),
            Teksto="-a",
            FontoId=fundamento.Id,
        };
        Vorto eFinaĵo = new()
        {
            Id = Guid.NewGuid(),
            Teksto="-e",
            FontoId=fundamento.Id,
        };
        Vorto iFinaĵo = new()
        {
            Id = Guid.NewGuid(),
            Teksto="-i",
            FontoId=fundamento.Id,
        };
        Vorto jFinaĵo = new()
        {
            Id = Guid.NewGuid(),
            Teksto="-j",
            FontoId=fundamento.Id,
        };
        Vorto nFinaĵo = new()
        {
            Id = Guid.NewGuid(),
            Teksto="-n",
            FontoId=fundamento.Id,
        };
        Vorto isFinaĵo = new()
        {
            Id = Guid.NewGuid(),
            Teksto="-is",
            FontoId=fundamento.Id,
        };
        Vorto asFinaĵo = new()
        {
            Id = Guid.NewGuid(),
            Teksto="-as",
            FontoId=fundamento.Id,
        };
        Vorto osFinaĵo = new()
        {
            Id = Guid.NewGuid(),
            Teksto="-os",
            FontoId=fundamento.Id,
        };
        Vorto usFinaĵo = new()
        {
            Id = Guid.NewGuid(),
            Teksto="-us",
            FontoId=fundamento.Id,
        };
        Vorto uFinaĵo = new()
        {
            Id = Guid.NewGuid(),
            Teksto="-u",
            FontoId=fundamento.Id,
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

        context.SaveChanges();

        Console.WriteLine("Kreante vortojn...");

        var derivaĵoj = new List<(CsvModel, Vorto)>();
        foreach(var e in records)
        {

            var fonto = e.Statuso.Split(' ')[0] switch
            {
                "F" => fundamento,
                "1OA" => unuaAldono,
                "2OA" => duaAldono,
                "3OA" => triaAldono,
                "4OA" => kvaraAldono,
                "5OA" => kvinaAldono,
                "6OA" => sesaAldono,
                "7OA" => sepaAldono,
                "8OA" => okaAldono,
                "9OA" => naŭaAldono,
                "1923" => analogAldono,
                "1929" => misAldono,
                "1953" => endAldono,
                _ => throw new NotImplementedException()
            };

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
                    FontoId = fonto.Id
                };

            v.Tradukoj = new();

            if(!string.IsNullOrEmpty(e.France) )
                v.Tradukoj.Add(
                    new ()
                    {
                        FontoId = fonto.Id == fundamento.Id ? (e.France.EndsWith("AK") ? akademiaKorekto.Id : fundamento.Id) : fonto.Id,
                        LingvoId = fr.Id,
                        Teksto = e.France.EndsWith("AK") ? e.France[..^2] : e.France
                    }
                );
            if(!string.IsNullOrEmpty(e.Angle) )
                v.Tradukoj.Add(
                    new()
                    {
                        FontoId = fonto.Id == fundamento.Id ? (e.Angle.EndsWith("AK") ? akademiaKorekto.Id : fundamento.Id) : fonto.Id,
                        LingvoId = en.Id,
                        Teksto = e.Angle.EndsWith("AK") ? e.Angle[..^2] : e.Angle
                    }
                );
            if(!string.IsNullOrEmpty(e.Germane) )
                v.Tradukoj.Add(
                    new()
                    {
                        FontoId = fonto.Id == fundamento.Id ? (e.Germane.EndsWith("AK") ? akademiaKorekto.Id : fundamento.Id) : fonto.Id,
                        LingvoId = de.Id,
                        Teksto = e.Germane.EndsWith("AK") ? e.Germane[..^2] : e.Germane
                    }
                );
            if(!string.IsNullOrEmpty(e.Pole) )
                v.Tradukoj.Add(
                    new()
                    {
                        FontoId = fonto.Id == fundamento.Id ? (e.Pole.EndsWith("AK") ? akademiaKorekto.Id : fundamento.Id) : fonto.Id,
                        LingvoId = pl.Id,
                        Teksto = e.Pole.EndsWith("AK") ? e.Pole[..^2] : e.Pole
                    }
                );
            if(!string.IsNullOrEmpty(e.Ruse) )
                v.Tradukoj.Add(
                    new()
                    {
                        FontoId = fonto.Id == fundamento.Id ? (e.Ruse.EndsWith("AK") ? akademiaKorekto.Id : fundamento.Id) : fonto.Id,
                        LingvoId = ru.Id,
                        Teksto = e.Ruse.EndsWith("AK") ? e.Ruse[..^2] : e.Ruse
                    }
                );
            if(!string.IsNullOrEmpty(e.Hispane) )
                v.Tradukoj.Add(
                    new()
                    {
                        FontoId = fonto.Id == fundamento.Id ? (e.Hispane.EndsWith("AK") ? akademiaKorekto.Id : fundamento.Id) : fonto.Id,
                        LingvoId = es.Id,
                        Teksto = e.Hispane.EndsWith("AK") ? e.Hispane[..^2] : e.Hispane
                    }
                );
            if(!string.IsNullOrEmpty(e.Itale) )
                v.Tradukoj.Add(
                    new()
                    {
                        FontoId = fonto.Id == fundamento.Id ? (e.Itale.EndsWith("AK") ? akademiaKorekto.Id : fundamento.Id) : fonto.Id,
                        LingvoId = it.Id,
                        Teksto = e.Itale.EndsWith("AK") ? e.Itale[..^2] : e.Itale
                    }
                );
            if(!string.IsNullOrEmpty(e.Katalune) )
                v.Tradukoj.Add(
                    new()
                    {
                        FontoId = fonto.Id == fundamento.Id ? (e.Ruse.EndsWith("AK") ? akademiaKorekto.Id : fundamento.Id) : fonto.Id,
                        LingvoId = ca.Id,
                        Teksto = e.Katalune.EndsWith("AK") ? e.Katalune[..^2] : e.Katalune
                    }
                );
            if(!string.IsNullOrEmpty(e.Portugale) )
                v.Tradukoj.Add(
                    new()
                    {
                        FontoId = fonto.Id == fundamento.Id ? (e.Ruse.EndsWith("AK") ? akademiaKorekto.Id : fundamento.Id) : fonto.Id,
                        LingvoId = pt.Id,
                        Teksto = e.Portugale.EndsWith("AK") ? e.Portugale[..^2] : e.Portugale
                    }
                );
            if(!string.IsNullOrEmpty(e.OficialaDifino))
                v.Difinoj = new()
                {
                    new()
                    {
                        FontoId = fonto.Id,
                        Teksto = e.OficialaDifino
                    }
                };
            
            if(v.Teksto != null)
                context.Add(v);
            else
                derivaĵoj.Add((e,v));

            Console.WriteLine(v.Teksto);
        }

        context.SaveChanges();
        
        Console.WriteLine("Plenumante kunmetaĵojn...");

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

    public static async Task ImportiRevon(string loko, VortaroContext context)
    {
        var revFonto = new Fonto()
        {
            Signo = "RV",
            Ligilo = "https://retavortaro.de/revo/dlg/index-2d.html",
            ĈuUzantkreita = false,
            Titolo = "Reta Vortaro",
            Favoreco = 0
        };

        if(!context.Fontoj.Any(f=>f.Titolo == revFonto.Titolo))
        {
            context.Fontoj.AddRange(
                revFonto

            );
            await context.SaveChangesAsync();
        }


        var finaĵoj = context.Vortoj.Where(v => v.ĈuFinaĵo).ToDictionary(v => v.Teksto!.TrimStart('-'), v=>v.Id);

        foreach(var f in System.IO.Directory.GetFiles(loko))
        {
            Vorto? vorto;

            var artikoloj = XElement.Load(new System.IO.FileStream(f, FileMode.Open)).Elements("art");

            foreach(var a in artikoloj)
            {
                var kap = a.Element("kap")!;
                var rad = kap.Element("rad")!.Value;
                vorto = context.Vortoj.SingleOrDefault(v => v.Teksto == rad);
                if(vorto == null)
                {
                    vorto = new Vorto
                    {
                        Teksto = rad,
                        FinaĵoId = finaĵoj[kap.Value.Substring(kap.Value.IndexOf("/"),1)]
                    };
                    var ofc = kap.Element("ofc")!;
                    if(ofc.Value == "*")
                        vorto.FontoId = revFonto.Id;
                    else
                        vorto.FontoId = (await context.Fontoj.FindAsync(ofc.Value))!.Id;
                }
            }
        }

        await context.SaveChangesAsync();
    }
}