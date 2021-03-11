# This Dockerfile contains Build and Release steps:
# 1. Build image
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /source

# Cache nuget restore
COPY /src/KubernetesProbeDemo/*.csproj .
RUN dotnet restore KubernetesProbeDemo.csproj

# Copy sources and compile
COPY /src/KubernetesProbeDemo .
RUN dotnet publish KubernetesProbeDemo.csproj --output /app/ --configuration Release

# 2. Release image
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

# Copy content from Build image
COPY --from=build /app .

ENTRYPOINT ["dotnet", "KubernetesProbeDemo.dll"]
