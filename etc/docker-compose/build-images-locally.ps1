param ($version='latest')

$currentFolder = $PSScriptRoot
$slnFolder = Join-Path $currentFolder "../../"

Write-Host "********* BUILDING DbMigrator *********" -ForegroundColor Green
$dbMigratorFolder = Join-Path $slnFolder "src/NewBlazorWebApp.DbMigrator"
Set-Location $dbMigratorFolder
dotnet publish -c Release
docker build -f Dockerfile.local -t mycompanyname/newblazorwebapp-db-migrator:$version .





Write-Host "********* BUILDING Blazor Web Application *********" -ForegroundColor Green
$blazorWebAppFolder = Join-Path $slnFolder "src/NewBlazorWebApp.Blazor"
Set-Location $blazorWebAppFolder
dotnet publish -c Release
docker build -f Dockerfile.local -t mycompanyname/newblazorwebapp-blazor:$version .



### ALL COMPLETED
Write-Host "COMPLETED" -ForegroundColor Green
Set-Location $currentFolder