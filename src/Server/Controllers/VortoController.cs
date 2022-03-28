using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

namespace vortaro.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class VortoController : ControllerBase
{
    private readonly ILogger<VortoController> _logger;

    private readonly VortaroContext _context;

    public VortoController(ILogger<VortoController> logger, VortaroContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet("{serĉfrazo}")]
    [AllowAnonymous]
    public IEnumerable<Vorto> Get(string serĉfrazo)
    {
        var s = Komuna.TraktiSerĉfrazon(serĉfrazo);

        // se la uzanto ne uzas RegEsp, ni povas simple kontroli
        // .Contains en la db   
        var param = new SqliteParameter("p0",s);
        if(System.Text.RegularExpressions.Regex.Escape(s) == s)
        {
            var petastring = 
                @$"SELECT A.*
                FROM Vortoj AS A 
                LEFT JOIN Vortoj AS F ON A.FinaĵoId = F.Id 
                LEFT JOIN 
                    (
                    SELECT L2.DerivaĵaVortoId, group_concat(REPLACE(L2.Teksto,""-"",""""),"""") as Teksto 
                    FROM 
                        (
                        SELECT L1.DerivaĵaVortoId, R.Teksto FROM Radiko AS L1 
                        LEFT JOIN Vortoj AS R ON R.Id = L1.RadikaVortoId 
                        ORDER BY L1.Ordo
                        ) AS L2
                    GROUP BY L2.DerivaĵaVortoId 
                    ) 
                    AS L ON A.Id = L.DerivaĵaVortoId

                WHERE instr(REPLACE(IFNULL(A.Teksto,L.Teksto)||IFNULL(F.Teksto,""""),""-"",""""), ""{param.Value}"") > 0";
            var q = _context.Vortoj
                .FromSqlRaw(petastring, param);                      
            return q.ToArray();
        }
        else
        {
            var petastring = 
                @$"SELECT A.*
                FROM Vortoj AS A 
                LEFT JOIN Vortoj AS F ON A.FinaĵoId = F.Id 
                LEFT JOIN 
                    (
                    SELECT L2.DerivaĵaVortoId, group_concat(REPLACE(L2.Teksto,""-"",""""),"""") as Teksto 
                    FROM 
                        (
                        SELECT L1.DerivaĵaVortoId, R.Teksto FROM Radiko AS L1 
                        LEFT JOIN Vortoj AS R ON R.Id = L1.RadikaVortoId 
                        ORDER BY L1.Ordo
                        ) AS L2
                    GROUP BY L2.DerivaĵaVortoId 
                    ) 
                    AS L ON A.Id = L.DerivaĵaVortoId

                WHERE regexp(""{param.Value}"",REPLACE(IFNULL(A.Teksto,L.Teksto)||IFNULL(F.Teksto,""""),""-"","""")) > 0";
            var q = _context.Vortoj
                .FromSqlRaw(petastring, param);                      
            return q.ToArray();
        }
    }
    
    [HttpGet("{id:Guid}")]
    public async Task<Vorto> GetVorto(Guid id)
    {
        var vorto = await _context.FindAsync<Vorto>(id);
        if(vorto is null) 
        {
            NotFound();
            return null!;
        }
        return vorto;
    }
    
    [HttpGet("{id:Guid}/radikoj")]
    public async Task<IEnumerable<Guid>> GetRadikoj(Guid id)
    {
        var vorto = await _context.FindAsync<Vorto>(id);
        if(vorto is null) 
        {
            NotFound();
            return null!;
        }
        _context.Entry(vorto).Collection(v=>v.Radikoj).Load();
        return vorto.Radikoj.OrderBy(r => r.Ordo).Select(t => t.RadikaVortoId);
    }

    [HttpGet("{id:Guid}/tradukoj")]
    public async Task<IEnumerable<Guid>> GetTradukoj(Guid id)
    {
        var vorto = await _context.FindAsync<Vorto>(id);
        if(vorto is null) 
        {
            NotFound();
            return null!;
        }
        _context.Entry(vorto).Collection(v=>v.Tradukoj).Load();
        foreach(var t in vorto.Tradukoj)
            _context.Entry(t).Reference(t=>t.Fonto).Load();
        return vorto.Tradukoj.OrderByDescending(t => t.Fonto.Favoreco).Select(t => t.Id);
    }
    
    [HttpGet("{id:Guid}/difinoj")]
    public async Task<IEnumerable<Guid>> GetDifinoj(Guid id)
    {
        var vorto = await _context.FindAsync<Vorto>(id);
        if(vorto is null) 
        {
            NotFound();
            return null!;
        }
        _context.Entry(vorto).Collection(v=>v.Difinoj).Load();
        foreach(var d in vorto.Difinoj)
            _context.Entry(d).Reference(d=>d.Fonto).Load();
        return vorto.Difinoj.OrderByDescending(d => d.Fonto.Favoreco).Select(d => d.Id);
    }
    
    [HttpGet("{id:Guid}/ekzemploj")]
    public async Task<IEnumerable<Guid>> GetEkzemploj(Guid id)
    {
        var vorto = await _context.FindAsync<Vorto>(id);
        if(vorto is null) 
        {
            NotFound();
            return null!;
        }
        _context.Entry(vorto).Collection(v=>v.Ekzemploj).Load();
        foreach(var e in vorto.Ekzemploj)
            _context.Entry(e).Reference(e=>e.Fonto).Load();
        return vorto.Ekzemploj.OrderByDescending(e => e.Fonto.Favoreco).Select(e => e.Id);
    }

    [Authorize]
    [HttpPost]
    public async Task<Vorto> Post(Vorto v)
    {
        _context.Add(v);
        await _context.SaveChangesAsync();
        return v;
    }
}
