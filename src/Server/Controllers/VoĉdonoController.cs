using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;

namespace vortaro.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VoĉdonoController : ControllerBase
{
    private readonly ILogger<VortoController> _logger;

    private readonly VortaroContext _context;

    public VoĉdonoController(ILogger<VortoController> logger, VortaroContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    [HttpPost("{enhavoId:guid}")]
    [Authorize]
    public async Task<Voĉdono> Post(Guid enhavoId, [FromBody] bool supre)
    {
        var uid = HttpContext.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")!.Value;
        var vd = _context.Voĉdonoj
            .FirstOrDefault(v =>
                v.EnhavoId == enhavoId 
                && v.UzantoId == uid);
        if(vd is null)
        {
            vd = _context.Add(new Voĉdono{EnhavoId = enhavoId, UzantoId = uid, ĈuSupraPoento = supre}).Entity;
        }
        else
        {
            // neniu ŝanĝo
            if(vd.ĈuSupraPoento == supre) return vd;

            vd.ĈuSupraPoento = supre;
        }        
        await _context.SaveChangesAsync();
        var e = await _context.FindAsync<Enhavo>(enhavoId);
        await _context
            .Entry(e!)
            .Reference(e=>e.Fonto)
            .LoadAsync();

        var voĉdonoj = _context
            .Voĉdonoj
            .Where(v => v.EnhavoId == enhavoId)
            .ToArray()
            .GroupBy(v=>v.ĈuSupraPoento)
            .ToDictionary(g => g.Key, g=>g.Count());

        if(!voĉdonoj.ContainsKey(true)) voĉdonoj[true] = 0;
        if(!voĉdonoj.ContainsKey(false)) voĉdonoj[false] = 0;
        e!.Fonto.Favoreco = voĉdonoj[true] - voĉdonoj[false];
        await _context.SaveChangesAsync();
        return vd;
    }
}