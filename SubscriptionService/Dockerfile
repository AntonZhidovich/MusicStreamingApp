FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_ENVIRONMENT=Release

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src/SubscriptionService/
COPY ["SubscriptionService/SubscriptionService.API/SubscriptionService.API.csproj", "SubscriptionService.API/"]
COPY ["SubscriptionService/SubscriptionService.BusinessLogic/SubscriptionService.BusinessLogic.csproj", "SubscriptionService.BusinessLogic/"]
COPY ["SubscriptionService/SubscriptionService.DataAccess/SubscriptionService.DataAccess.csproj", "SubscriptionService.DataAccess/"]
COPY ["Identity/Identity.BusinessLogic/Contracts/userservice.proto", "/src/Identity/Identity.BusinessLogic/Contracts/"]
RUN dotnet restore "SubscriptionService.API/SubscriptionService.API.csproj"
COPY ["SubscriptionService/", "."]
WORKDIR "/src/SubscriptionService/SubscriptionService.API"
RUN dotnet build "SubscriptionService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SubscriptionService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SubscriptionService.API.dll"]