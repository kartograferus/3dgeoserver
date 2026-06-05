using System.Text.Json;
using System.Text.Json.Serialization;

namespace Geo3DServer.Models;

[JsonConverter(typeof(BoundingVolumeConverter))]
public abstract class BoundingVolume;

public sealed class BoundingVolumeBox : BoundingVolume
{
    [JsonPropertyOrder(0)]
    public double[] Box { get; set; } = new double[12];
}

public sealed class BoundingVolumeRegion : BoundingVolume
{
    [JsonPropertyOrder(0)]
    public double[] Region { get; set; } = new double[6];
}

public sealed class BoundingVolumeSphere : BoundingVolume
{
    [JsonPropertyOrder(0)]
    public double[] Sphere { get; set; } = new double[4];
}

public sealed class BoundingVolumeConverter : JsonConverter<BoundingVolume>
{
    public override BoundingVolume? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException("Expected StartObject for boundingVolume");

        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        if (root.TryGetProperty("box", out var box))
            return new BoundingVolumeBox
            {
                Box = JsonSerializer.Deserialize<double[]>(box.GetRawText(), options)!
            };

        if (root.TryGetProperty("region", out var region))
            return new BoundingVolumeRegion
            {
                Region = JsonSerializer.Deserialize<double[]>(region.GetRawText(), options)!
            };

        if (root.TryGetProperty("sphere", out var sphere))
            return new BoundingVolumeSphere
            {
                Sphere = JsonSerializer.Deserialize<double[]>(sphere.GetRawText(), options)!
            };

        throw new JsonException("boundingVolume must contain one of: box, region, sphere");
    }

    public override void Write(Utf8JsonWriter writer, BoundingVolume value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}