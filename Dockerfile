# This Dockerfile contains Build and Release steps:
# 1. Build image
FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /source

# Cache nuget restore
COPY /src/KubernetesProbeDemo/*.csproj .
RUN dotnet restore KubernetesProbeDemo.csproj

# Copy sources and compile
COPY /src/KubernetesProbeDemo .
RUN dotnet publish KubernetesProbeDemo.csproj --output /app/ --configuration Release

# 2. Release image
FROM microsoft/dotnet:2.1-aspnetcore-runtime
WORKDIR /app
EXPOSE 80

# Copy content from Build image
COPY --from=build /app .

ENTRYPOINT ["dotnet", "KubernetesProbeDemo.dll"]
