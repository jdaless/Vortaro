using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;

namespace vortaro.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DifinoController : ControllerBase
{
    private readonly ILogger<VortoController> _logger;

    private readonly VortaroContext _context;

    public DifinoController(ILogger<VortoController> logger, VortaroContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet("{id:Guid}")]
    public async Task<Difino> Get(Guid id)
    {
        var difino = await _context.FindAsync<Difino>(id);
        if(difino is null) 
        {
            NotFound();
            return null!;
        }
        return difino;
    }

    [HttpGet("{serĉfrazo}")]
    public IEnumerable<Vorto> Get(string serĉfrazo, bool off = true, bool uzf = true)
    {
        var s = Komuna.TraktiSerĉfrazon(serĉfrazo);

        // se la uzanto ne uzas RegEsp, ni povas simple kontroli
        // .Contains en la db   
        var q = Regex.Escape(s) == s
            ? _context.Difinoj
                .Where(e => 
                    ((off && !e.Fonto.ĈuUzantkreita) || (uzf && e.Fonto.ĈuUzantkreita))
                        && e.Teksto.Contains(serĉfrazo))
            : _context.Difinoj
                .Where(e => 
                        ((off && !e.Fonto.ĈuUzantkreita) || (uzf && e.Fonto.ĈuUzantkreita))
                            && Regex.IsMatch(e.Teksto, s));
                            
        var a = q.Select(e => e.Vorto).ToArray();

        foreach(var v in a) _context.Entry(v).Reference(v=>v.Fonto).Load();
        return a;    
    }

    
    [HttpPost]
    [Authorize]
    public async Task<Difino> Post(Difino d)
    {
        _context.Add(d);
        await _context.SaveChangesAsync();
        return d;
    }
    
    [HttpDelete("{id:Guid}")]
    [Authorize]
    public async Task Delete(Guid id)
    {
        var uid = HttpContext.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")!.Value;
        var e = (await _context.Difinoj.FindAsync(id))!;
        _context.Entry(e).Reference(e=>e.Fonto).Load();
        if(e.Fonto.KreintoId != uid) throw new UnauthorizedAccessException();
        _context.Remove(e);
        _context.Remove(e.Fonto);
        await _context.SaveChangesAsync();
    }
}