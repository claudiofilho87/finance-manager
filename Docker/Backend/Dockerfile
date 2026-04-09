# Stage 1: Base runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Stage 2: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY Backend/src/FinanceManager.Domain/FinanceManager.Domain.csproj src/FinanceManager.Domain/
COPY Backend/src/FinanceManager.Application/FinanceManager.Application.csproj src/FinanceManager.Application/
COPY Backend/src/FinanceManager.Infrastructure/FinanceManager.Infrastructure.csproj src/FinanceManager.Infrastructure/
COPY Backend/src/FinanceManager.API/FinanceManager.API.csproj src/FinanceManager.API/
RUN dotnet restore src/FinanceManager.API/FinanceManager.API.csproj
COPY Backend/src/ src/
RUN dotnet build src/FinanceManager.API/FinanceManager.API.csproj -c $BUILD_CONFIGURATION -o /app/build

# Stage 3: Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish src/FinanceManager.API/FinanceManager.API.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 4: Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FinanceManager.API.dll"]