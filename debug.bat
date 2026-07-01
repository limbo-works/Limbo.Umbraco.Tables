@echo off
dotnet build src/Limbo.Umbraco.Tables.StaticAssets --configuration Debug /t:rebuild /t:pack -p:PackageOutputPath=C:\nuget\local
dotnet build src/Limbo.Umbraco.Tables --configuration Debug /t:rebuild /t:pack -p:PackageOutputPath=C:\nuget\local