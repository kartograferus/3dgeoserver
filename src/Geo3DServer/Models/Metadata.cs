using System.Text.Json.Serialization;

namespace Geo3DServer.Models;

public sealed class MetadataEntity
{
    [JsonPropertyOrder(0)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Class { get; set; }

    [JsonPropertyOrder(1)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, object>? Properties { get; set; }
}

public sealed class SubtreeHeader
{
    [JsonPropertyName("magic")]
    [JsonPropertyOrder(0)]
    public string Magic { get; set; } = "subt";

    [JsonPropertyName("version")]
    [JsonPropertyOrder(1)]
    public int Version { get; set; } = 0;
}

public sealed class SubtreeFile
{
    [JsonPropertyOrder(0)]
    public SubtreeHeader Header { get; set; } = new();

    [JsonPropertyOrder(1)]
    public SubtreeBuffers Buffers { get; set; } = new();

    [JsonPropertyOrder(2)]
    public SubtreeBufferViews BufferViews { get; set; } = new();

    [JsonPropertyOrder(3)]
    public SubtreeAvailability Availability { get; set; } = new();

    [JsonPropertyOrder(4)]
    public SubtreeMetadata Metadata { get; set; } = new();
}

public sealed class SubtreeBuffers
{
    [JsonPropertyOrder(0)]
    public List<byte[]> Data { get; set; } = new();
}

public sealed class SubtreeBufferViews
{
    [JsonPropertyOrder(0)]
    public List<SubtreeBufferView> Views { get; set; } = new();
}

public sealed class SubtreeBufferView
{
    [JsonPropertyOrder(0)]
    public int Buffer { get; set; }

    [JsonPropertyOrder(1)]
    public int ByteOffset { get; set; }

    [JsonPropertyOrder(2)]
    public int ByteLength { get; set; }
}

public sealed class SubtreeAvailability
{
    [JsonPropertyOrder(0)]
    public SubtreeAvailabilityBitstream? TileAvailability { get; set; }

    [JsonPropertyOrder(1)]
    public SubtreeAvailabilityBitstream? ContentAvailability { get; set; }

    [JsonPropertyOrder(2)]
    public SubtreeAvailabilityBitstream? ChildSubtreeAvailability { get; set; }
}

public sealed class SubtreeAvailabilityBitstream
{
    [JsonPropertyOrder(0)]
    public int Bitstream { get; set; }

    [JsonPropertyOrder(1)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? AvailableCount { get; set; }

    [JsonPropertyOrder(2)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Constant { get; set; }
}

public sealed class SubtreeMetadata
{
    [JsonPropertyOrder(0)]
    public List<object> TileMetadata { get; set; } = new();

    [JsonPropertyOrder(1)]
    public List<object> ContentMetadata { get; set; } = new();

    [JsonPropertyOrder(2)]
    public List<object> SubtreeMetadataItems { get; set; } = new();
}