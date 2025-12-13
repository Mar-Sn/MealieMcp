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
FROM mcr.microsoft.com/dotnet/runtime:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# The entrypoint is the executable name
ENTRYPOINT ["./MealieMcp"]
