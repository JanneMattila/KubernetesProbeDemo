# This Dockerfile contains Build and Release steps:
# 1. Build image
FROM microsoft/aspnetcore-build:2.0.7-2.1.105-stretch AS build
WORKDIR /source

# Cache nuget restore
COPY /src/KubernetesProbeDemo/*.csproj .
RUN dotnet restore KubernetesProbeDemo.csproj

# Copy sources and compile
COPY /src/KubernetesProbeDemo .
RUN dotnet publish KubernetesProbeDemo.csproj --output /app/ --configuration Release

# 2. Release image
FROM microsoft/aspnetcore:2.0.7-stretch
WORKDIR /app
EXPOSE 80

# Copy content from Build image
COPY --from=build /app .

ENTRYPOINT ["dotnet", "KubernetesProbeDemo.dll"]
