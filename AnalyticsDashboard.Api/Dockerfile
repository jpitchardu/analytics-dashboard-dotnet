FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY AnalyticsDashboard.sln .
COPY AnalyticsDashboard.Api/*.csproj ./AnalyticsDashboard.Api/
COPY AnalyticsDashboard.Core/*.csproj ./AnalyticsDashboard.Core/
COPY AnalyticsDashboard.Infrastructure/*.csproj ./AnalyticsDashboard.Infrastructure/

RUN dotnet restore

COPY . .

WORKDIR /src/AnalyticsDashboard.Api
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AnalyticsDashboard.Api.dll"]