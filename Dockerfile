# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and project files first to leverage layer caching
COPY ["MealieMcp.slnx", "./"]
COPY ["MealieMcp/MealieMcp.csproj", "MealieMcp/"]
RUN dotnet restore "MealieMcp/MealieMcp.csproj"

# Copy the rest of the source code
COPY . .
WORKDIR "/src/MealieMcp"
# Publish the application
# We rely on project settings for SelfContained/SingleFile, but specifying -r linux-x64 is good practice for Docker
RUN dotnet publish "MealieMcp.csproj" -c Release -r linux-x64 -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# OpenTelemetry Configuration
ENV OTEL_SERVICE_NAME="MealieMcp"
ENV OTEL_EXPORTER_OTLP_ENDPOINT="http://localhost:4317"
ENV OTEL_EXPORTER_OTLP_PROTOCOL="grpc"
# Listen on all interfaces
ENV ASPNETCORE_URLS="http://+:8080"

# The entrypoint is the executable name
ENTRYPOINT ["./MealieMcp"]
