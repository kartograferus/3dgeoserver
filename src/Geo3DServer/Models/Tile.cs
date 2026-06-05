using System.Text.Json.Serialization;

namespace Geo3DServer.Models;

public sealed class Tile
{
    [JsonPropertyOrder(0)]
    public BoundingVolume BoundingVolume { get; set; } = new BoundingVolumeRegion();

    [JsonPropertyOrder(1)]
    public double GeometricError { get; set; }

    [JsonPropertyOrder(2)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Refine { get; set; } // "ADD" or "REPLACE"

    [JsonPropertyOrder(3)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TileContent? Content { get; set; }

    [JsonPropertyOrder(5)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<Tile>? Children { get; set; }

    [JsonPropertyOrder(6)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ImplicitTiling? ImplicitTiling { get; set; }

    [JsonPropertyOrder(99)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? Metadata { get; set; }

    [JsonPropertyOrder(100)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, object>? Extensions { get; set; }

    [JsonPropertyOrder(101)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? Extras { get; set; }
}