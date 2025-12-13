# Design: Docker and CI/CD

## Containerization Strategy
We will use a multi-stage Docker build to ensure a small final image size.

1.  **Build Stage:**
    *   Base Image: `mcr.microsoft.com/dotnet/sdk:10.0` (or appropriate preview tag given the .NET 10 target).
    *   Actions: Copy source, restore dependencies, build, and publish the application in Release mode.

2.  **Runtime Stage:**
    *   Base Image: `mcr.microsoft.com/dotnet/runtime:10.0` (or deps/runtime-deps if AOT is used, but standard runtime is safer for now).
    *   Actions: Copy published artifacts from the build stage.
    *   Entrypoint: Configured to run the `MealieMcp` executable.
