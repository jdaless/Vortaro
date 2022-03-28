using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace vortaro.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class FontoController : ControllerBase
{
    private readonly ILogger<VortoController> _logger;

    private readonly VortaroContext _context;

    public FontoController(ILogger<VortoController> logger, VortaroContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet()]
    public List<Fonto> Get() =>  _context.Fontoj.ToList();

    [HttpPost]
    [Authorize]
    public async Task<Fonto> Post(Fonto f)
    {
        _context.Add(f);
        await _context.SaveChangesAsync();
        return f;
    }
        
}