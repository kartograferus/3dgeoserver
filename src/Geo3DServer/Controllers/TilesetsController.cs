using Microsoft.AspNetCore.Mvc;
using Geo3DServer.Services;

namespace Geo3DServer.Controllers;

[ApiController]
[Route("tilesets")]
public class TilesetsController : ControllerBase
{
    private readonly ITilesetService _service;

    public TilesetsController(ITilesetService service)
    {
        _service = service;
    }

    [HttpGet]
    [Produces("application/json")]
    public IActionResult ListTilesets()
    {
        var tilesets = _service.ListTilesets();
        return Ok(tilesets);
    }

    [HttpGet("{tilesetId}")]
    [Produces("application/json")]
    public IActionResult GetTileset(string tilesetId)
    {
        var tileset = _service.GetTileset(tilesetId);
        if (tileset == null)
            return NotFound(new { error = $"Tileset '{tilesetId}' not found" });

        return Ok(tileset);
    }

    [HttpGet("{tilesetId}/styles")]
    [Produces("application/json")]
    public IActionResult GetStyles(string tilesetId)
    {
        if (!_service.TilesetExists(tilesetId))
            return NotFound(new { error = $"Tileset '{tilesetId}' not found" });

        var styles = _service.GetStyles(tilesetId);
        if (styles == null)
            return NotFound(new { error = $"No styles defined for tileset '{tilesetId}'" });

        return Ok(styles);
    }

    [HttpGet("{tilesetId}/metadata")]
    [Produces("application/json")]
    public IActionResult GetMetadata(string tilesetId)
    {
        if (!_service.TilesetExists(tilesetId))
            return NotFound(new { error = $"Tileset '{tilesetId}' not found" });

        var metadata = _service.GetMetadata(tilesetId, null, null);
        if (metadata == null)
            return NotFound(new { error = $"No metadata for tileset '{tilesetId}'" });

        return Ok(metadata);
    }
}