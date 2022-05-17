using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;

namespace vortaro.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TradukoController : ControllerBase
{
    private readonly ILogger<VortoController> _logger;

    private readonly VortaroContext _context;

    public TradukoController(ILogger<VortoController> logger, VortaroContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet("{id:Guid}")]
    public async Task<Traduko> Get(Guid id)
    {
        var traduko = await _context.FindAsync<Traduko>(id);
        if(traduko is null) 
        {
            NotFound();
            return null!;
        }
        return traduko;
    }
    
    [HttpGet("{lingvo}/{serĉfrazo}")]
    public IEnumerable<Vorto> Get(string lingvo, string serĉfrazo, bool off = true, bool uzf = true)
    {
        var s = Komuna.TraktiSerĉfrazon(serĉfrazo);

        // se la uzanto ne uzas RegEsp, ni povas simple kontroli
        // .Contains en la db   
        if(Regex.Escape(s) == s)
        {
            var a = _context.Tradukoj
                .Where(e => 
                    ((off && !e.Fonto.ĈuUzantkreita) || (uzf && e.Fonto.ĈuUzantkreita))
                        && e.LingvoId == lingvo
                        && e.Teksto.Contains(serĉfrazo))
                .Select(e => e.Vorto)
                .ToArray();
            foreach(var v in a) _context.Entry(v).Reference(v=>v.Fonto).Load();
            return a;
        }
        else
        {
            var a = _context.Tradukoj
                .Where(e => 
                        ((off && !e.Fonto.ĈuUzantkreita) || (uzf && e.Fonto.ĈuUzantkreita))
                            && e.LingvoId == lingvo
                            && Regex.IsMatch(e.Teksto, s))
                .Select(e => e.Vorto)
                .ToArray();               
            foreach(var v in a) _context.Entry(v).Reference(v=>v.Fonto).Load();
            return a;
        }
    }

    
    [HttpPost]
    [Authorize]
    public async Task<Traduko> Post(Traduko t)
    {
        _context.Add(t);
        await _context.SaveChangesAsync();
        return t;
    }  
    
    [HttpDelete("{id:Guid}")]
    [Authorize]
    public async Task Delete(Guid id)
    {
        var uid = HttpContext.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")!.Value;
        var e = (await _context.Tradukoj.FindAsync(id))!;
        _context.Entry(e).Reference(e=>e.Fonto).Load();
        if(e.Fonto.KreintoId != uid) throw new UnauthorizedAccessException();
        _context.Remove(e);
        _context.Remove(e.Fonto);
        await _context.SaveChangesAsync();
    }
}