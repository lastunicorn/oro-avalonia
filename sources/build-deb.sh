#!/bin/bash
set -e

# Configuration
APP_NAME="oro-avalonia"
APP_VERSION="1.0.0"
ARCH="amd64"
BUILD_DIR="build-deb"
PACKAGE_NAME="${APP_NAME}_${APP_VERSION}_${ARCH}"

echo "Building DEB package for ${APP_NAME} v${APP_VERSION}..."

# Clean previous build
if [ -d "$BUILD_DIR" ]; then
    rm -rf "$BUILD_DIR"
fi

# Create directory structure
mkdir -p "$BUILD_DIR/$PACKAGE_NAME/DEBIAN"
mkdir -p "$BUILD_DIR/$PACKAGE_NAME/opt/$APP_NAME"
mkdir -p "$BUILD_DIR/$PACKAGE_NAME/usr/share/applications"

# Publish the .NET application
echo "Publishing .NET application..."
dotnet publish sources/OroAvalonia/OroAvalonia.csproj \
    -c Release \
    -r linux-x64 \
    --self-contained false \
    -o "$BUILD_DIR/publish"

# Copy published files to package structure
echo "Copying application files..."
cp -r "$BUILD_DIR/publish/"* "$BUILD_DIR/$PACKAGE_NAME/opt/$APP_NAME/"

# Copy control file
cp debian/control "$BUILD_DIR/$PACKAGE_NAME/DEBIAN/"

# Copy and set permissions for maintainer scripts
if [ -f "debian/postinst" ]; then
    cp debian/postinst "$BUILD_DIR/$PACKAGE_NAME/DEBIAN/"
    chmod 755 "$BUILD_DIR/$PACKAGE_NAME/DEBIAN/postinst"
fi

if [ -f "debian/prerm" ]; then
    cp debian/prerm "$BUILD_DIR/$PACKAGE_NAME/DEBIAN/"
    chmod 755 "$BUILD_DIR/$PACKAGE_NAME/DEBIAN/prerm"
fi

# Copy desktop entry
if [ -f "debian/$APP_NAME.desktop" ]; then
    cp "debian/$APP_NAME.desktop" "$BUILD_DIR/$PACKAGE_NAME/usr/share/applications/"
fi

# Set permissions
chmod 755 "$BUILD_DIR/$PACKAGE_NAME/opt/$APP_NAME/Oro"

# Build the DEB package
echo "Building DEB package..."
dpkg-deb --build "$BUILD_DIR/$PACKAGE_NAME"

# Move the package to the current directory
mv "$BUILD_DIR/${PACKAGE_NAME}.deb" .

echo "DEB package created successfully: ${PACKAGE_NAME}.deb"
echo ""
echo "To install the package, run:"
echo "  sudo dpkg -i ${PACKAGE_NAME}.deb"
echo ""
echo "To install dependencies (if needed):"
echo "  sudo apt-get install -f"
