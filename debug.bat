@echo off
dotnet build src/Limbo.Umbraco.Tables --configuration Debug /t:rebuild /t:pack -p:PackageOutputPath=c:\nuget\Umbraco10