# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy everything and restore as distinct layers
COPY ["SmartCard.sln", "./"]
COPY ["SmartCard.Api/SmartCard.Api.csproj", "SmartCard.Api/"]
COPY ["SmartCard.Application/SmartCard.Application.csproj", "SmartCard.Application/"]
COPY ["SmartCard.Domain/SmartCard.Domain.csproj", "SmartCard.Domain/"]
COPY ["SmartCard.Infrastructure/SmartCard.Infrastructure.csproj", "SmartCard.Infrastructure/"]
RUN dotnet restore

# Copy the remaining files and build the app
COPY . .
WORKDIR /app/SmartCard.Api
RUN dotnet publish -c Release -o /out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /out ./

# Set environment variables (adjust as needed)
ENV DOTNET_RUNNING_IN_CONTAINER=true \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    ASPNETCORE_ENVIRONMENT=Production

COPY ./https /https

EXPOSE 8080 443
# Define the entry point for the container
ENTRYPOINT ["dotnet", "SmartCard.Api.dll"]

