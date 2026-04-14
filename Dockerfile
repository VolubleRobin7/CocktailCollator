FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy everything and build
COPY . .
RUN dotnet restore
RUN dotnet publish CocktailCollator.Web/CocktailCollator.Web.csproj -c Release -o /app/publish

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0

LABEL org.opencontainers.image.authors="VolubleRobin7"
LABEL org.opencontainers.image.description="Container package for the Cocktail Collator Blazor Server application, including SQL Server for data storage."
LABEL org.opencontainers.image.source="https://github.com/VolubleRobin7/CocktailCollator"

# Install SQL Server
RUN apt-get update && apt-get install -y curl apt-transport-https gnupg && \
    curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add - && \
    curl https://packages.microsoft.com/config/ubuntu/22.04/mssql-server-2022.list > /etc/apt/sources.list.d/mssql-server.list && \
    apt-get update && \
    ACCEPT_EULA=Y apt-get install -y mssql-server && \
    apt-get clean && rm -rf /var/lib/apt/lists/*

# Copy published app into container
WORKDIR /app
COPY --from=build /app/publish .

# Copy entrypoint
COPY entrypoint.sh /entrypoint.sh
RUN chmod +x /entrypoint.sh

# SQL Server environment variables
ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=YourStrong!Passw0rd

# Blazor Server environment variables
ENV ASPNETCORE_URLS=http://+:7012
ENV ASPNETCORE_ENVIRONMENT=Container

EXPOSE 7012
#EXPOSE 1433

ENTRYPOINT ["/entrypoint.sh"]