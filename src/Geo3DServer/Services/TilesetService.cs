using System.Text.Json;
using Geo3DServer.Models;

namespace Geo3DServer.Services;

public sealed class TilesetService : ITilesetService
{
    private readonly string _basePath;
    private readonly ILogger<TilesetService> _logger;
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = false
    };

    public TilesetService(IConfiguration configuration, ILogger<TilesetService> logger)
    {
        var configuredPath = configuration.GetValue<string>("Tilesets:DataPath");
        if (!string.IsNullOrEmpty(configuredPath) && Path.IsPathFullyQualified(configuredPath))
        {
            _basePath = configuredPath;
        }
        else if (!string.IsNullOrEmpty(configuredPath))
        {
            _basePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, configuredPath));
        }
        else
        {
            _basePath = Path.Combine(AppContext.BaseDirectory, "data", "tilesets");
        }

        _logger = logger;
        _logger.LogInformation("Tileset data path: {Path}", _basePath);
    }

    public IEnumerable<TilesetInfo> ListTilesets()
    {
        if (!Directory.Exists(_basePath))
            return Enumerable.Empty<TilesetInfo>();

        var result = new List<TilesetInfo>();
        foreach (var dir in Directory.EnumerateDirectories(_basePath))
        {
            var tilesetJson = Path.Combine(dir, "tileset.json");
            if (!File.Exists(tilesetJson)) continue;

            var info = new TilesetInfo
            {
                Id = Path.GetFileName(dir),
                Name = Path.GetFileName(dir),
                UpdatedAt = new FileInfo(tilesetJson).LastWriteTimeUtc
            };

            try
            {
                var json = File.ReadAllText(tilesetJson);
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (root.TryGetProperty("asset", out var asset) &&
                    asset.TryGetProperty("version", out var version))
                {
                    info.Description = $"3D Tiles v{version.GetString()}";
                }
            }
            catch { }

            result.Add(info);
        }

        return result;
    }

    public Tileset? GetTileset(string tilesetId)
    {
        var tilesetPath = Path.Combine(_basePath, tilesetId, "tileset.json");
        if (!File.Exists(tilesetPath)) return null;

        var json = File.ReadAllText(tilesetPath);
        return JsonSerializer.Deserialize<Tileset>(json, _jsonOptions);
    }

    public bool TilesetExists(string tilesetId)
    {
        var tilesetPath = Path.Combine(_basePath, tilesetId, "tileset.json");
        return File.Exists(tilesetPath);
    }

    public byte[]? GetContent(string tilesetId, string contentPath)
    {
        var fullPath = GetSafePath(tilesetId, contentPath);
        if (fullPath == null || !File.Exists(fullPath)) return null;

        _logger.LogInformation("Serving tile content: {Path}", fullPath);
        return File.ReadAllBytes(fullPath);
    }

    public SubtreeFile? GetSubtree(string tilesetId, string subtreePath)
    {
        var fullPath = GetSafePath(tilesetId, subtreePath);
        if (fullPath == null || !File.Exists(fullPath))
        {
            _logger.LogWarning("Subtree not found: {Path}", subtreePath);
            return null;
        }

        var bytes = File.ReadAllBytes(fullPath);
        return ParseSubtree(bytes);
    }

    public StyleCollection? GetStyles(string tilesetId)
    {
        var stylesPath = Path.Combine(_basePath, tilesetId, "styles.json");
        if (!File.Exists(stylesPath))
        {
            _logger.LogInformation("No styles.json found for tileset {Id}", tilesetId);
            return null;
        }

        var json = File.ReadAllText(stylesPath);
        return JsonSerializer.Deserialize<StyleCollection>(json, _jsonOptions);
    }

    public TilesetMetadataResponse? GetMetadata(string tilesetId, string? className, string? entityId)
    {
        var metadataPath = Path.Combine(_basePath, tilesetId, "metadata.json");
        if (!File.Exists(metadataPath))
            return null;

        var json = File.ReadAllText(metadataPath);
        var allMetadata = JsonSerializer.Deserialize<Dictionary<string, List<Dictionary<string, object>>>>(json, _jsonOptions);
        if (allMetadata == null) return null;

        var response = new TilesetMetadataResponse { TilesetId = tilesetId, Class = className };

        if (className != null && allMetadata.TryGetValue(className, out var entities))
        {
            if (entityId != null)
            {
                var entity = entities.FirstOrDefault(e =>
                    e.TryGetValue("id", out var id) && id?.ToString() == entityId);
                if (entity != null) response.Entities.Add(entity);
            }
            else
            {
                response.Entities = entities;
            }
        }
        else
        {
            response.Entities = allMetadata.Values.SelectMany(v => v).ToList();
        }

        return response;
    }

    private string? GetSafePath(string tilesetId, string relativePath)
    {
        var tilesetDir = Path.GetFullPath(Path.Combine(_basePath, tilesetId));
        var fullPath = Path.GetFullPath(Path.Combine(tilesetDir, relativePath));
        return fullPath.StartsWith(tilesetDir + Path.DirectorySeparatorChar) ||
               fullPath == tilesetDir
            ? fullPath
            : null;
    }

    private static SubtreeFile ParseSubtree(byte[] data)
    {
        return new SubtreeParser().Parse(data);
    }
}