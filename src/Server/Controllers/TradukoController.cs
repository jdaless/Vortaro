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
            return _context.Tradukoj
                .Where(e => 
                    ((off && !e.Fonto.ĈuUzantkreita) || (uzf && e.Fonto.ĈuUzantkreita))
                        && e.LingvoId == lingvo
                        && e.Teksto.Contains(serĉfrazo))
                .Select(e => e.Vorto)
                .ToArray();
        }
        return _context.Tradukoj
            .Where(e => 
                    ((off && !e.Fonto.ĈuUzantkreita) || (uzf && e.Fonto.ĈuUzantkreita))
                        && e.LingvoId == lingvo
                        && Regex.IsMatch(e.Teksto, s))
            .Select(e => e.Vorto)
            .ToArray();   
    }

    
    [HttpPost]
    [Authorize]
    public async Task<Traduko> Post(Traduko t)
    {
        _context.Add(t);
        await _context.SaveChangesAsync();
        return t;
    }
}