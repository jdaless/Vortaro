using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace vortaro.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class UzantoController : ControllerBase
{
    private readonly ILogger<VortoController> _logger;

    private readonly VortaroContext _context;

    public UzantoController(ILogger<VortoController> logger, VortaroContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet("{id}")]
    public Uzanto? Get(string id)
    {
        return _context.Find<Uzanto>(System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(id)));
    }
        
    // dZqGU.avsq!6aH8
}