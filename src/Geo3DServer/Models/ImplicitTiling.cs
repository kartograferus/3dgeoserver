using System.Text.Json.Serialization;

namespace Geo3DServer.Models;

public sealed class ImplicitTiling
{
    [JsonPropertyOrder(0)]
    public string SubdivisionScheme { get; set; } = "QUADTREE";

    [JsonPropertyOrder(1)]
    public int SubtreeLevels { get; set; } = 7;

    [JsonPropertyOrder(2)]
    public int AvailableLevels { get; init; }

    [JsonPropertyOrder(3)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ImplicitSubtrees? Subtrees { get; set; }
}

public sealed class ImplicitSubtrees
{
    [JsonPropertyOrder(0)]
    public string Uri { get; set; } = string.Empty;
}