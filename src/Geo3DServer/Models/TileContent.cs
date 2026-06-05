using System.Text.Json.Serialization;

namespace Geo3DServer.Models;

public sealed class TileContent
{
    [JsonPropertyOrder(0)]
    public string Uri { get; set; } = string.Empty;

    [JsonPropertyOrder(1)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BoundingVolume? BoundingVolume { get; set; }
}