using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace vortaro.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class LingvoController : ControllerBase
{
    private readonly ILogger<VortoController> _logger;

    private readonly VortaroContext _context;

    public LingvoController(ILogger<VortoController> logger, VortaroContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet()]
    public List<Lingvo> Get() =>  _context.Lingvoj.ToList();
}