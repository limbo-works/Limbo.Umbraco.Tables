@echo off

REM Install dependencies and build client
pushd "%~dp0src\Limbo.Umbraco.Tables\Client"

call npm install
if errorlevel 1 exit /b 1

call npm run build
if errorlevel 1 exit /b 1

popd

REM Build and pack .NET project
dotnet build src/Limbo.Umbraco.Tables --configuration Debug /t:rebuild /t:pack -p:PackageOutputPath=C:\nuget\Umbraco17