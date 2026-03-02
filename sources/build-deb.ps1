# PowerShell script to build DEB package on Windows using WSL or Docker

param(
    [string]$Method = "wsl",  # Options: "wsl" or "docker"
    [string]$Version = "1.0.0"
)

$AppName = "oro-avalonia"
$PackageName = "${AppName}_${Version}_amd64"

Write-Host "Building DEB package for $AppName v$Version using $Method..." -ForegroundColor Green

if ($Method -eq "wsl") {
    # Using WSL
    Write-Host "Using WSL to build the package..." -ForegroundColor Yellow
    
    # Convert Windows path to WSL path
    $currentPath = (Get-Location).Path
    $wslPath = wsl wslpath -a "'$currentPath'"
    
    # Execute the build script in WSL
    wsl bash -c "cd '$wslPath' && chmod +x build-deb.sh && ./build-deb.sh"
    
} elseif ($Method -eq "docker") {
    # Using Docker
    Write-Host "Using Docker to build the package..." -ForegroundColor Yellow
    
    docker run --rm -v "${PWD}:/workspace" -w /workspace mcr.microsoft.com/dotnet/sdk:8.0 bash -c "
        apt-get update && apt-get install -y dpkg-dev
        chmod +x build-deb.sh
        ./build-deb.sh
    "
} else {
    Write-Host "Invalid method. Use 'wsl' or 'docker'." -ForegroundColor Red
    exit 1
}

if ($LASTEXITCODE -eq 0) {
    Write-Host "`nDEB package created successfully: ${PackageName}.deb" -ForegroundColor Green
    Write-Host "`nTo test installation on WSL:" -ForegroundColor Cyan
    Write-Host "  wsl sudo dpkg -i ${PackageName}.deb" -ForegroundColor White
} else {
    Write-Host "`nBuild failed!" -ForegroundColor Red
    exit $LASTEXITCODE
}
