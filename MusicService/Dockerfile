FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_ENVIRONMENT=Release

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src/MusicService/
COPY ["MusicService/MusicService.API/MusicService.API.csproj", "MusicService.API/"]
COPY ["MusicService/MusicService.Infrastructure/MusicService.Infrastructure.csproj", "MusicService.Infrastructure/"]
COPY ["MusicService/MusicService.Application/MusicService.Application.csproj", "MusicService.Application/"]
COPY ["MusicService/MusicService.Domain/MusicService.Domain.csproj", "MusicService.Domain/"]
COPY ["Identity/Identity.BusinessLogic/Contracts/userservice.proto", "/src/Identity/Identity.BusinessLogic/Contracts/"]
RUN dotnet restore "./MusicService.API/MusicService.API.csproj"
COPY ["MusicService/", "."]
WORKDIR "/src/MusicService/MusicService.API"
RUN dotnet build "MusicService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MusicService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MusicService.API.dll"]