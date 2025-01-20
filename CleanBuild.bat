@echo off
dotnet clean FirstApi/FirstApi.csproj
dotnet clean FirstMvcWebApp/FirstMvcWebApp.csproj
dotnet build FirstApi/FirstApi.csproj
dotnet build FirstMvcWebApp/FirstMvcWebApp.csproj
