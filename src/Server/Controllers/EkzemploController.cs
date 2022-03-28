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
            return _context.Ekzemploj
                .Where(e => 
                    ((off && !e.Fonto.ĈuUzantkreita) || (uzf && e.Fonto.ĈuUzantkreita))
                        && e.Teksto.Contains(serĉfrazo))
                .Select(e => e.Vorto)
                .ToArray();
        }
        return _context.Ekzemploj
            .Where(e => 
                    ((off && !e.Fonto.ĈuUzantkreita) || (uzf && e.Fonto.ĈuUzantkreita))
                        && Regex.IsMatch(e.Teksto, s))
            .Select(e => e.Vorto)
            .ToArray();    
    }
    
    [HttpPost]
    [Authorize]
    public async Task<Ekzemplo> Post(Ekzemplo e)
    {
        _context.Add(e);
        await _context.SaveChangesAsync();
        return e;
    }
}