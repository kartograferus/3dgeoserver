using Microsoft.AspNetCore.Mvc;
using Geo3DServer.Services;

namespace Geo3DServer.Controllers;

[ApiController]
[Route("tilesets/{tilesetId}/tiles")]
public class TilesController : ControllerBase
{
    private readonly ITilesetService _service;

    public TilesController(ITilesetService service)
    {
        _service = service;
    }

    [HttpGet("{**tilePath}")]
    public IActionResult GetTileContent(string tilesetId, string? tilePath)
    {
        if (!_service.TilesetExists(tilesetId))
            return NotFound(new { error = $"Tileset '{tilesetId}' not found" });

        var contentPath = tilePath ?? string.Empty;

        if (contentPath.EndsWith(".json"))
        {
            var subtree = _service.GetSubtree(tilesetId, contentPath);
            if (subtree != null)
                return Ok(subtree);
        }

        var content = _service.GetContent(tilesetId, contentPath);
        if (content == null)
            return NotFound(new { error = $"Tile content '{contentPath}' not found" });

        var contentType = GetContentType(contentPath);
        return File(content, contentType);
    }

    private static string GetContentType(string path)
    {
        return Path.GetExtension(path).ToLowerInvariant() switch
        {
            ".b3dm" => "application/octet-stream",
            ".i3dm" => "application/octet-stream",
            ".pnts" => "application/octet-stream",
            ".cmpt" => "application/octet-stream",
            ".glb" => "model/gltf-binary",
            ".gltf" => "model/gltf+json",
            ".json" => "application/json",
            ".png" => "image/png",
            ".jpg" or ".jpeg" => "image/jpeg",
            ".bin" => "application/octet-stream",
            _ => "application/octet-stream"
        };
    }
}