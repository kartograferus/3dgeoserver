# 3D GeoServer

Мини-пет-проект для проверки возможностей AI-кодинга по созданию геосерверов на C#. Сервер реализует REST API по спецификации **OGC 3D Tiles 1.1 (22-025r4)** для хостинга и раздачи трёхмерных геопространственных данных.

## Стек

- **C# 12** / **.NET 8.0**
- **ASP.NET Core** — Web API, Minimal Hosting
- **System.Text.Json** — сериализация/десериализация 3D Tiles JSON
- **Swagger** (Swashbuckle) — документация API
- **Docker** / **Docker Compose** — контейнеризация и деплой

## Что сделано

- **Модели OGC 3D Tiles 1.1:** `Tileset`, `Tile`, `BoundingVolume` (box/region/sphere — кастомный `JsonConverter` без type-дискриминатора), `TileContent`, `ImplicitTiling`, `Style` (включая expression-объекты), `MetadataEntity`, `SubtreeFile`
- **Сервисный слой:** файловое хранилище тайлсетов, парсер бинарных subtree-файлов (`SubtreeParser`), path-traversal защита (`GetSafePath`)
- **REST API:** эндпоинты для списка тайлсетов, `tileset.json`, стилей, метаданных, бинарного контента тайлов (b3dm/glb/pnts)
- **Демо-данные:** тайлсет `sample-city` с region boundingVolume, 3 стилями (Default, Height-based, LOD-debug), метаданными зданий
- **Docker:** мультистейдж-сборка, docker-compose с volume-mount данных
- **Безопасность:** path-traversal защита, sandboxed filesystem access через `GetSafePath`

## Структура проекта

```
3dgeoserver/
├── Dockerfile
├── docker-compose.yml
├── .dockerignore
├── README.md
├── data/
│   └── tilesets/
│       └── sample-city/
│           ├── tileset.json
│           ├── styles.json
│           └── metadata.json
└── src/
    └── Geo3DServer/
        ├── Geo3DServer.csproj
        ├── Program.cs
        ├── appsettings.json
        ├── Models/
        │   ├── BoundingVolume.cs      — box/region/sphere + JsonConverter
        │   ├── ImplicitTiling.cs      — subdivisionScheme, subtreeLevels
        │   ├── Metadata.cs            — MetadataEntity, SubtreeFile
        │   ├── Style.cs               — 3D Tiles Styles (Color — object?)
        │   ├── Tile.cs                — boundingVolume, geometricError, children
        │   ├── TileContent.cs         — uri + boundingVolume
        │   └── Tileset.cs             — asset v1.1, root, schema, statistics
        ├── Services/
        │   ├── ITilesetService.cs     — интерфейс + TilesetInfo/MetadataResponse
        │   ├── TilesetService.cs      — файловое хранилище + GetSafePath
        │   └── SubtreeParser.cs       — бинарный парсер .subtree
        └── Controllers/
            ├── HealthController.cs    — GET /, GET /health
            ├── TilesController.cs     — GET /tilesets/{id}/tiles/{**path}
            └── TilesetsController.cs  — GET /tilesets, /styles, /metadata
```

## REST API

| Метод | Путь | Описание |
|-------|------|----------|
| GET | `/` | Информация о сервисе |
| GET | `/health` | Health-check |
| GET | `/tilesets` | Список тайлсетов |
| GET | `/tilesets/{id}` | `tileset.json` |
| GET | `/tilesets/{id}/styles` | `styles.json` |
| GET | `/tilesets/{id}/metadata` | Метаданные |
| GET | `/tilesets/{id}/tiles/{**path}` | Бинарный контент тайла |

Swagger UI: `/api-docs`

## Запуск

### Docker (рекомендуемый)

```powershell
docker compose up --build -d
# Сервер на http://localhost:5090
```

### Локально (требуется .NET 8.0 SDK)

```powershell
cd src/Geo3DServer
dotnet run
```

---
*Сгенерировано с помощью Kilo Code, 2026*