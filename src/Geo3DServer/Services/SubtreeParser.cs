using System.Text;
using Geo3DServer.Models;

namespace Geo3DServer.Services;

public sealed class SubtreeParser
{
    private static readonly byte[] Magic = Encoding.ASCII.GetBytes("subt");

    public SubtreeFile Parse(byte[] data)
    {
        var stream = new MemoryStream(data);
        var reader = new BinaryReader(stream);

        var magic = reader.ReadBytes(4);
        if (!magic.SequenceEqual(Magic))
            throw new InvalidDataException("Invalid subtree magic number");

        var version = reader.ReadUInt32();
        var header = new SubtreeHeader { Magic = "subt", Version = (int)version };

        var jsonByteLength = ReadVarUint(reader);
        var jsonData = reader.ReadBytes((int)jsonByteLength);

        var binaryByteLength = ReadVarUint(reader);
        var binaryData = binaryByteLength > 0
            ? reader.ReadBytes((int)binaryByteLength)
            : Array.Empty<byte>();

        var subtree = System.Text.Json.JsonSerializer.Deserialize<SubtreeJson>(jsonData)
                      ?? new SubtreeJson();

        return new SubtreeFile
        {
            Header = header,
            Buffers = new SubtreeBuffers
            {
                Data = binaryData.Length > 0
                    ? new List<byte[]> { binaryData }
                    : new List<byte[]>()
            },
            BufferViews = new SubtreeBufferViews
            {
                Views = subtree.BufferViews ?? new List<SubtreeBufferView>()
            },
            Availability = new SubtreeAvailability
            {
                TileAvailability = subtree.TileAvailability != null
                    ? new SubtreeAvailabilityBitstream
                    {
                        Bitstream = subtree.TileAvailability.Bitstream,
                        AvailableCount = subtree.TileAvailability.AvailableCount,
                        Constant = subtree.TileAvailability.Constant
                    }
                    : null,
                ContentAvailability = subtree.ContentAvailability != null
                    ? new SubtreeAvailabilityBitstream
                    {
                        Bitstream = subtree.ContentAvailability.Bitstream,
                        AvailableCount = subtree.ContentAvailability.AvailableCount,
                        Constant = subtree.ContentAvailability.Constant
                    }
                    : null,
                ChildSubtreeAvailability = subtree.ChildSubtreeAvailability != null
                    ? new SubtreeAvailabilityBitstream
                    {
                        Bitstream = subtree.ChildSubtreeAvailability.Bitstream,
                        AvailableCount = subtree.ChildSubtreeAvailability.AvailableCount,
                        Constant = subtree.ChildSubtreeAvailability.Constant
                    }
                    : null
            },
            Metadata = new SubtreeMetadata
            {
                TileMetadata = subtree.TileMetadata ?? new List<object>(),
                ContentMetadata = subtree.ContentMetadata ?? new List<object>(),
                SubtreeMetadataItems = subtree.SubtreeMetadata ?? new List<object>()
            }
        };
    }

    private static uint ReadVarUint(BinaryReader reader)
    {
        uint value = 0;
        int shift = 0;
        byte b;
        do
        {
            b = reader.ReadByte();
            value |= (uint)(b & 0x7F) << shift;
            shift += 7;
        } while ((b & 0x80) != 0);
        return value;
    }

    private sealed class SubtreeJson
    {
        public List<SubtreeBufferView>? BufferViews { get; set; }
        public Avail? TileAvailability { get; set; }
        public Avail? ContentAvailability { get; set; }
        public Avail? ChildSubtreeAvailability { get; set; }
        public List<object>? TileMetadata { get; set; }
        public List<object>? ContentMetadata { get; set; }
        public List<object>? SubtreeMetadata { get; set; }
    }

    private sealed class Avail
    {
        public int Bitstream { get; set; }
        public int? AvailableCount { get; set; }
        public bool? Constant { get; set; }
    }
}