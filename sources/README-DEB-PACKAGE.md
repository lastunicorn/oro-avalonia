# Creating a DEB Package for OroAvalonia

This guide explains how to create a Debian package (.deb) for the OroAvalonia application to deploy it on Ubuntu/Debian-based Linux distributions.

## Prerequisites

### On Linux (Ubuntu/Debian):
```bash
# Install required tools
sudo apt-get update
sudo apt-get install -y dpkg-dev dotnet-sdk-8.0
```

### On Windows:
You have two options:

**Option 1: Using WSL (Windows Subsystem for Linux)**
```powershell
# Install WSL if not already installed
wsl --install

# Inside WSL, install required tools
wsl sudo apt-get update
wsl sudo apt-get install -y dpkg-dev
```

**Option 2: Using Docker**
```powershell
# Ensure Docker Desktop is installed and running
```

## Project Structure

The following files are needed for DEB package creation:

```
oro-avalonia/
??? debian/
?   ??? control                    # Package metadata
?   ??? postinst                   # Post-installation script
?   ??? prerm                      # Pre-removal script
?   ??? oro-avalonia.desktop       # Desktop entry file
??? build-deb.sh                   # Linux build script
??? build-deb.ps1                  # Windows PowerShell build script
```

## Building the DEB Package

### Method 1: On Linux Directly

```bash
# Make the script executable
chmod +x build-deb.sh

# Run the build script
./build-deb.sh
```

### Method 2: On Windows Using WSL

```powershell
# Run the PowerShell script (uses WSL by default)
.\build-deb.ps1

# Or explicitly specify WSL
.\build-deb.ps1 -Method wsl -Version "1.0.0"
```

### Method 3: On Windows Using Docker

```powershell
# Run the PowerShell script with Docker
.\build-deb.ps1 -Method docker -Version "1.0.0"
```

## What the Scripts Do

1. **Publish the application**: Uses `dotnet publish` to create a Linux x64 build
2. **Create DEB structure**: Sets up the proper directory hierarchy
3. **Copy files**: Places the application in `/opt/oro-avalonia/`
4. **Create metadata**: Adds control files and maintainer scripts
5. **Build package**: Uses `dpkg-deb` to create the final `.deb` file

## Package Installation

After building, you'll have a file like `oro-avalonia_1.0.0_amd64.deb`.

### Install the package:
```bash
sudo dpkg -i oro-avalonia_1.0.0_amd64.deb
```

### Install missing dependencies (if any):
```bash
sudo apt-get install -f
```

### Verify installation:
```bash
dpkg -l | grep oro-avalonia
```

### Run the application:
```bash
/opt/oro-avalonia/Oro
```

Or find it in your application menu as "Oro Avalonia".

## Package Removal

```bash
sudo dpkg -r oro-avalonia
```

Or with configuration files:
```bash
sudo dpkg --purge oro-avalonia
```

## Customization

### Update Package Version
Edit `debian/control` and the version in the build scripts.

### Change Installation Location
Modify the build scripts to change from `/opt/oro-avalonia/` to your preferred location.

### Add Dependencies
Edit `debian/control` and add a `Depends:` line:
```
Depends: libicu72, libssl3, zlib1g
```

### Self-Contained Build
For a self-contained build (includes .NET runtime), modify the publish command in `build-deb.sh`:
```bash
dotnet publish sources/OroAvalonia/OroAvalonia.csproj \
    -c Release \
    -r linux-x64 \
    --self-contained true \
    -o "$BUILD_DIR/publish"
```

Note: This will significantly increase the package size but won't require .NET runtime to be installed on target systems.

## Troubleshooting

### Package doesn't install
- Check dependencies: `sudo apt-get install -f`
- Verify architecture matches: `dpkg --print-architecture`

### Application doesn't run
- Check permissions: `ls -l /opt/oro-avalonia/Oro`
- Install .NET runtime: `sudo apt-get install dotnet-runtime-8.0`
- Check dependencies: `ldd /opt/oro-avalonia/Oro`

### Missing desktop icon
- Create an icon file and update `debian/oro-avalonia.desktop`
- Place icon in `/opt/oro-avalonia/` or `/usr/share/pixmaps/`

## Advanced: Using dotnet-deb Tool

Alternatively, you can use the `dotnet-deb` tool for easier package creation:

```bash
# Install the tool
dotnet tool install --global dotnet-deb

# Create package
dotnet deb install \
    -c Release \
    -f net8.0 \
    -r linux-x64 \
    sources/OroAvalonia/OroAvalonia.csproj
```

## Distribution

Once you have the DEB package, you can:
- Distribute it directly to users
- Host it on your website
- Create a custom APT repository
- Upload to a PPA (Personal Package Archive)
