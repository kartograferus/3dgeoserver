# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/Geo3DServer/Geo3DServer.csproj src/Geo3DServer/
RUN dotnet restore src/Geo3DServer/Geo3DServer.csproj

COPY src/Geo3DServer/ src/Geo3DServer/
COPY data/ data/

RUN dotnet publish src/Geo3DServer/Geo3DServer.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT=Production
ENV Tilesets__DataPath=/app/data/tilesets

COPY --from=build /app .
COPY --from=build /src/data/tilesets data/tilesets

EXPOSE 5090

ENTRYPOINT ["dotnet", "Geo3DServer.dll"]