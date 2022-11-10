@echo off
dotnet build src/Limbo.Umbraco.StructuredData --configuration Debug /t:rebuild /t:pack -p:PackageOutputPath=c:\nuget