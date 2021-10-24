FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["NuGet.Config", "."]
COPY ["RecomendationForStartups/RecomendationForStartups.csproj", "RecomendationForStartups/"]
COPY ["RecomendationForStartups.ServiceInterface/RecomendationForStartups.ServiceInterface.csproj", "RecomendationForStartups.ServiceInterface/"]
COPY ["RecomendationForStartups.ServiceModel/RecomendationForStartups.ServiceModel.csproj", "RecomendationForStartups.ServiceModel/"]
RUN dotnet restore "RecomendationForStartups/RecomendationForStartups.csproj"
COPY . .
WORKDIR "/src/RecomendationForStartups"
RUN dotnet build "RecomendationForStartups.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RecomendationForStartups.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RecomendationForStartups.dll"]
