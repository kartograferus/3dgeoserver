using Microsoft.AspNetCore.Mvc;

namespace Geo3DServer.Controllers;

[ApiController]
public class HealthController : ControllerBase
{
    [HttpGet("/health")]
    [Produces("application/json")]
    public IActionResult Health()
    {
        return Ok(new { status = "healthy", timestamp = DateTimeOffset.UtcNow });
    }

    [HttpGet("/")]
    [Produces("application/json")]
    public IActionResult Root()
    {
        return Ok(new
        {
            service = "3D GeoServer",
            version = "0.1.0",
            specification = "OGC 3D Tiles 1.1 (22-025r4)",
            capabilities = new[]
            {
                "GET /tilesets",
                "GET /tilesets/{id}",
                "GET /tilesets/{id}/tiles/{**tilePath}",
                "GET /tilesets/{id}/styles",
                "GET /tilesets/{id}/metadata",
                "GET /health"
            }
        });
    }
}