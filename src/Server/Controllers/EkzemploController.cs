using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;

namespace vortaro.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EkzemploController : ControllerBase
{
    private readonly ILogger<VortoController> _logger;

    private readonly VortaroContext _context;

    public EkzemploController(ILogger<VortoController> logger, VortaroContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet("{id:Guid}")]
    public async Task<Ekzemplo> Get(Guid id)
    {
        var ekzemplo = await _context.FindAsync<Ekzemplo>(id);
        if(ekzemplo is null) 
        {
            NotFound();
            return null!;
        }
        return ekzemplo;
    }
    
    [HttpGet("{serĉfrazo}")]
    public IEnumerable<Vorto> Get(string serĉfrazo, bool off = true, bool uzf = true)
    {
        var s = Komuna.TraktiSerĉfrazon(serĉfrazo);

        // se la uzanto ne uzas RegEsp, ni povas simple kontroli
        // .Contains en la db   
        if(Regex.Escape(s) == s)
        {
            var a = _context.Ekzemploj
                .Where(e => 
                    ((off && !e.Fonto.ĈuUzantkreita) || (uzf && e.Fonto.ĈuUzantkreita))
                        && e.Teksto.Contains(serĉfrazo))
                .Select(e => e.Vorto)
                .ToArray();
            foreach(var v in a) _context.Entry(v).Reference(v=>v.Fonto).Load();
            return a;
            
        }
        else
        {
            var a = _context.Ekzemploj
                .Where(e => 
                        ((off && !e.Fonto.ĈuUzantkreita) || (uzf && e.Fonto.ĈuUzantkreita))
                            && Regex.IsMatch(e.Teksto, s))
                .Select(e => e.Vorto)
                .ToArray();  
            foreach(var v in a) _context.Entry(v).Reference(v=>v.Fonto).Load();
            return a;
        }  
    }
    
    [HttpPost]
    [Authorize]
    public async Task<Ekzemplo> Post(Ekzemplo e)
    {
        _context.Add(e);
        await _context.SaveChangesAsync();
        return e;
    }    
    
    [HttpDelete("{id:Guid}")]
    [Authorize]
    public async Task Delete(Guid id)
    {
        var uid = HttpContext.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")!.Value;
        var e = (await _context.Ekzemploj.FindAsync(id))!;
        _context.Entry(e).Reference(e=>e.Fonto).Load();
        if(e.Fonto.KreintoId != uid) throw new UnauthorizedAccessException();
        _context.Remove(e);
        _context.Remove(e.Fonto);
        await _context.SaveChangesAsync();
    }
}