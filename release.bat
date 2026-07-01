@echo off
dotnet build src/Limbo.Umbraco.Tables --configuration Release /t:rebuild /t:pack -p:PackageOutputPath=../../releases/nuget