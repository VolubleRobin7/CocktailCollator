FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

LABEL org.opencontainers.image.authors="VolubleRobin7"

COPY . .

RUN dotnet restore
RUN dotnet publish CocktailCollator.Web/CocktailCollator.Web.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:7012
EXPOSE 7012

ENTRYPOINT ["dotnet", "CocktailCollator.Web.dll"]