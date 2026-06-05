using System.Text.Json.Serialization;

namespace Geo3DServer.Models;

public sealed class Tileset
{
    [JsonPropertyName("asset")]
    [JsonPropertyOrder(0)]
    public TilesetAsset Asset { get; set; } = new();

    [JsonPropertyOrder(1)]
    public double GeometricError { get; set; }

    [JsonPropertyOrder(2)]
    public Tile Root { get; set; } = new();

    [JsonPropertyOrder(10)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TilesetSchema? Schema { get; set; }

    [JsonPropertyOrder(11)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TilesetStatistics? Statistics { get; set; }

    [JsonPropertyOrder(50)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? Groups { get; set; }

    [JsonPropertyOrder(51)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? Metadata { get; set; }

    [JsonPropertyOrder(99)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, object>? Extensions { get; set; }

    [JsonPropertyOrder(100)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? Extras { get; set; }
}

public sealed class TilesetAsset
{
    [JsonPropertyOrder(0)]
    public string Version { get; set; } = "1.1";
}

public sealed class TilesetSchema
{
    [JsonPropertyOrder(0)]
    public string? Id { get; set; }

    [JsonPropertyOrder(1)]
    public string? Name { get; set; }

    [JsonPropertyOrder(2)]
    public Dictionary<string, object> Classes { get; set; } = new();

    [JsonPropertyOrder(3)]
    public Dictionary<string, object> Enums { get; set; } = new();
}

public sealed class TilesetStatistics
{
    [JsonPropertyOrder(0)]
    public Dictionary<string, object> Classes { get; set; } = new();
}