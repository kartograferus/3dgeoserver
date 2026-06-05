using Geo3DServer.Models;

namespace Geo3DServer.Services;

public interface ITilesetService
{
    IEnumerable<TilesetInfo> ListTilesets();
    Tileset? GetTileset(string tilesetId);
    byte[]? GetContent(string tilesetId, string contentPath);
    SubtreeFile? GetSubtree(string tilesetId, string subtreePath);
    StyleCollection? GetStyles(string tilesetId);
    TilesetMetadataResponse? GetMetadata(string tilesetId, string? className, string? entityId);
    bool TilesetExists(string tilesetId);
}

public sealed class TilesetInfo
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTimeOffset UpdatedAt { get; set; }
}

public sealed class TilesetMetadataResponse
{
    public string TilesetId { get; set; } = string.Empty;
    public string? Class { get; set; }
    public List<Dictionary<string, object>> Entities { get; set; } = new();
}