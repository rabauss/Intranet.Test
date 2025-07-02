# Base image
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080 8081

RUN apt-get update && apt-get install -y curl

# Set environment variables
ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED=true \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    LC_ALL=de_DE.UTF-8 \
    LANG=de_DE.UTF-8

# Build image
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["Intranet.Test.sln", "./"]
COPY ["Directory.Packages.props", "./"]
COPY ["Intranet.Test.WebUI.Server/Intranet.Test.WebUI.Server.csproj", "Intranet.Test.WebUI.Server/"]

# Restore dependencies
RUN dotnet restore "./Intranet.Test.sln"

# Copy source code
COPY . .

# Build the project
RUN dotnet build "./Intranet.Test.sln" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "./Intranet.Test.sln" -c Release -o /app/publish /p:UseAppHost=false

# Final image
FROM base AS final
WORKDIR /app

# Create a new user and use it
RUN useradd -ms /bin/bash appuser
RUN chown -R appuser:appuser /app
USER appuser

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Intranet.Test.WebUI.Server.dll"]
