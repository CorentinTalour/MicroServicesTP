# Image de base runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Image de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copier le fichier csproj
COPY ["DetailFilm.csproj", "."]
RUN dotnet restore "DetailFilm.csproj"

# Copier le reste du projet
COPY . .

# Build de l'application
RUN dotnet build "DetailFilm.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publication
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "DetailFilm.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Image finale
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DetailFilm.dll"]