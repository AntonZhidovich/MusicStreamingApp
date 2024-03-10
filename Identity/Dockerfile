FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_ENVIRONMENT=Release

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src/Identity/
COPY ["Identity/Identity.API/Identity.API.csproj", "Identity.API/"]
COPY ["Identity/Identity.BusinessLogic/Identity.BusinessLogic.csproj", "Identity.BusinessLogic/"]
COPY ["Identity/Identity.DataAccess/Identity.DataAccess.csproj", "Identity.DataAccess/"]
COPY ["../MusicService/MusicService.Application/Contracts/musicservice.proto", "/src/MusicService/MusicService.Application/Contracts/"]
RUN dotnet restore "Identity.API/Identity.API.csproj"
COPY ["Identity/", "."]
WORKDIR "/src/Identity/Identity.API"
RUN dotnet build "Identity.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Identity.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Identity.API.dll"]