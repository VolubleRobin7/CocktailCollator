#!/bin/bash
set -e

echo "Starting SQL Server..."
/opt/mssql/bin/sqlservr &

echo "Starting Blazor app..."
exec dotnet CocktailCollator.Web.dll