# This script will download and build Rasterbar-libtorrent in both debug and
# release configurations.

$PACKAGES_DIRECTORY = Join-Path $PSScriptRoot "packages"
$OUTPUT_DIRECTORY   = Join-Path $PSScriptRoot "bin"
$VERSION            = "0.0.0"

$OPENSSL_PACKAGE_DIRECTORY = Join-Path $PACKAGES_DIRECTORY "hadouken.openssl.0.1.3"

if (Test-Path Env:\APPVEYOR_BUILD_VERSION) {
    $VERSION = $env:APPVEYOR_BUILD_VERSION
}

# 7zip configuration section
$7ZIP_VERSION      = "9.20"
$7ZIP_DIRECTORY    = Join-Path $PACKAGES_DIRECTORY "7zip-$7ZIP_VERSION"
$7ZIP_TOOL         = Join-Path $7ZIP_DIRECTORY "7za.exe"
$7ZIP_PACKAGE_FILE = "7za$($7ZIP_VERSION.replace('.', '')).zip"
$7ZIP_DOWNLOAD_URL = "http://downloads.sourceforge.net/project/sevenzip/7-Zip/$7ZIP_VERSION/$7ZIP_PACKAGE_FILE"

# Boost configuration section
$BOOST_VERSION      = "1.57.0"
$BOOST_DIRECTORY    = Join-Path $PACKAGES_DIRECTORY "boost_$($BOOST_VERSION.replace('.', '_'))"
$BOOST_PACKAGE_FILE = "boost_$($BOOST_VERSION.replace('.', '_')).7z"
$BOOST_DOWNLOAD_URL = "http://downloads.sourceforge.net/project/boost/boost/$BOOST_VERSION/$BOOST_PACKAGE_FILE"

# Libtorrent configuration section
$LIBTORRENT_VERSION      = "1.0.4"
$LIBTORRENT_DIRECTORY    = Join-Path $PACKAGES_DIRECTORY "libtorrent-rasterbar-$LIBTORRENT_VERSION"
$LIBTORRENT_PACKAGE_FILE = "libtorrent-rasterbar-$LIBTORRENT_VERSION.tar.gz"
$LIBTORRENT_DOWNLOAD_URL = "http://downloads.sourceforge.net/project/libtorrent/libtorrent/$LIBTORRENT_PACKAGE_FILE"

# Nuget configuration section
$NUGET_FILE         = "nuget.exe"
$NUGET_TOOL         = Join-Path $PACKAGES_DIRECTORY $NUGET_FILE
$NUGET_DOWNLOAD_URL = "https://nuget.org/$NUGET_FILE"

function Download-File {
    param (
        [string]$url,
        [string]$target
    )

    $webClient = new-object System.Net.WebClient
    $webClient.DownloadFile($url, $target)
}

function Extract-File {
    param (
        [string]$file,
        [string]$target
    )

    [System.Reflection.Assembly]::LoadWithPartialName('System.IO.Compression.FileSystem') | Out-Null
    [System.IO.Compression.ZipFile]::ExtractToDirectory($file, $target)
}

# Create packages directory if it does not exist
if (!(Test-Path $PACKAGES_DIRECTORY)) {
    New-Item -ItemType Directory -Path $PACKAGES_DIRECTORY | Out-Null
}

# Download 7zip
if (!(Test-Path (Join-Path $PACKAGES_DIRECTORY $7ZIP_PACKAGE_FILE))) {
    Write-Host "Downloading $7ZIP_PACKAGE_FILE"
    Download-File $7ZIP_DOWNLOAD_URL (Join-Path $PACKAGES_DIRECTORY $7ZIP_PACKAGE_FILE)
}

# Download Boost
if (!(Test-Path (Join-Path $PACKAGES_DIRECTORY $BOOST_PACKAGE_FILE))) {
    Write-Host "Downloading $BOOST_PACKAGE_FILE"
    Download-File $BOOST_DOWNLOAD_URL (Join-Path $PACKAGES_DIRECTORY $BOOST_PACKAGE_FILE)
}

# Download Libtorrent
if (!(Test-Path (Join-Path $PACKAGES_DIRECTORY $LIBTORRENT_PACKAGE_FILE))) {
    Write-Host "Downloading $LIBTORRENT_PACKAGE_FILE"
    Download-File $LIBTORRENT_DOWNLOAD_URL (Join-Path $PACKAGES_DIRECTORY $LIBTORRENT_PACKAGE_FILE)
}

# Download Nuget
if (!(Test-Path $NUGET_TOOL)) {
    Write-Host "Downloading $NUGET_FILE"
    Download-File $NUGET_DOWNLOAD_URL $NUGET_TOOL
}

# Unpack 7zip
if (!(Test-Path $7ZIP_DIRECTORY)) {
    Write-Host "Unpacking $7ZIP_PACKAGE_FILE"
    Extract-File (Join-Path $PACKAGES_DIRECTORY $7ZIP_PACKAGE_FILE) $7ZIP_DIRECTORY
}

# Unpack Boost (may take a while)
if (!(Test-Path $BOOST_DIRECTORY)) {
    Write-Host "Unpacking $BOOST_PACKAGE_FILE (this may take a while)"
    $pkg = Join-Path $PACKAGES_DIRECTORY $BOOST_PACKAGE_FILE
    & "$7ZIP_TOOL" x $pkg -o"$PACKAGES_DIRECTORY"
}

# Unpack Libtorrent
if (!(Test-Path $LIBTORRENT_DIRECTORY)) {
    Write-Host "Unpacking $LIBTORRENT_PACKAGE_FILE"
    $tmp = Join-Path $PACKAGES_DIRECTORY $LIBTORRENT_PACKAGE_FILE

    & "$7ZIP_TOOL" x $tmp -o"$PACKAGES_DIRECTORY"
    & "$7ZIP_TOOL" x $tmp.replace('.gz', '') -o"$PACKAGES_DIRECTORY"
}

# Install support package OpenSSL
& "$NUGET_TOOL" install hadouken.openssl -Version 0.1.3 -OutputDirectory "$PACKAGES_DIRECTORY"

function Build-Boost {
    Push-Location $BOOST_DIRECTORY

    # Bootstrap Boost only if we do not have b2 already
    if (!(Test-Path b2.exe)) {
        cmd /c bootstrap.bat
    }

    Start-Process ".\b2.exe" -ArgumentList "toolset=msvc-12.0 link=shared runtime-link=shared --with-chrono --with-thread" -Wait -NoNewWindow

    Pop-Location
}

Build-Boost

function Compile-Libtorrent {
    param (
        [string]$platform,
        [string]$configuration
    )

    Push-Location $LIBTORRENT_DIRECTORY

    $b2 = Join-Path $BOOST_DIRECTORY "b2.exe"

    $openssl_include = Join-Path $OPENSSL_PACKAGE_DIRECTORY "$platform/$configuration/include"
    $openssl_lib = Join-Path $OPENSSL_PACKAGE_DIRECTORY "$platform/$configuration/lib"

    Start-Process "$b2" -ArgumentList "-sBOOST_ROOT=""$BOOST_DIRECTORY"" toolset=msvc-12.0 include=""$openssl_include"" library-path=""$openssl_lib"" variant=$configuration boost=source boost-link=shared dht=on i2p=on encryption=openssl link=shared runtime-link=shared deprecated-functions=off debug-symbols=on" -Wait -NoNewWindow

    Pop-Location
}

function Output-Libtorrent {
    param (
        [string]$platform,
        [string]$configuration
    )

    pushd $LIBTORRENT_DIRECTORY

    $t = Join-Path $OUTPUT_DIRECTORY "$platform\$configuration"
    $out = "bin\msvc-12.0\$configuration\boost-link-shared\boost-source\deprecated-functions-off\encryption-openssl\threading-multi"

    if ($configuration -eq "release")
    {
        $out = "bin\msvc-12.0\$configuration\boost-link-shared\boost-source\debug-symbols-on\deprecated-functions-off\encryption-openssl\threading-multi"
    }

    # Copy output files
    xcopy /y "$out\*.lib" "$OUTPUT_DIRECTORY\$platform\lib\$configuration\*"
    xcopy /y "$out\*.dll" "$OUTPUT_DIRECTORY\$platform\bin\$configuration\*"
    xcopy /y "$out\*.pdb" "$OUTPUT_DIRECTORY\$platform\bin\$configuration\*"

    popd
}

function Output-Boost {
    param (
        [string]$platform,
        [string]$configuration
    )

    Push-Location $BOOST_DIRECTORY

    # Copy chrono
    xcopy /y "bin.v2\libs\chrono\build\msvc-12.0\$configuration\threading-multi\*.dll" "$OUTPUT_DIRECTORY\$platform\bin\$configuration\*"
    xcopy /y "bin.v2\libs\chrono\build\msvc-12.0\$configuration\threading-multi\*.pdb" "$OUTPUT_DIRECTORY\$platform\bin\$configuration\*"
    xcopy /y "bin.v2\libs\chrono\build\msvc-12.0\$configuration\threading-multi\*.lib" "$OUTPUT_DIRECTORY\$platform\lib\$configuration\*"

    # Copy thread
    xcopy /y "bin.v2\libs\thread\build\msvc-12.0\$configuration\threading-multi\*.dll" "$OUTPUT_DIRECTORY\$platform\bin\$configuration\*"
    xcopy /y "bin.v2\libs\thread\build\msvc-12.0\$configuration\threading-multi\*.pdb" "$OUTPUT_DIRECTORY\$platform\bin\$configuration\*"
    xcopy /y "bin.v2\libs\thread\build\msvc-12.0\$configuration\threading-multi\*.lib" "$OUTPUT_DIRECTORY\$platform\lib\$configuration\*"

    $extra = ""

    if ($configuration -eq "release")
    {
        $extra = "debug-symbols-on\"
    }

    # Copy date_time
    xcopy /y "bin.v2\libs\date_time\build\msvc-12.0\$configuration\boost-link-shared\boost-source\$($extra)deprecated-functions-off\encryption-openssl\threading-multi\*.dll" "$OUTPUT_DIRECTORY\$platform\bin\$configuration\*"
    xcopy /y "bin.v2\libs\date_time\build\msvc-12.0\$configuration\boost-link-shared\boost-source\$($extra)deprecated-functions-off\encryption-openssl\threading-multi\*.pdb" "$OUTPUT_DIRECTORY\$platform\bin\$configuration\*"
    xcopy /y "bin.v2\libs\date_time\build\msvc-12.0\$configuration\boost-link-shared\boost-source\$($extra)deprecated-functions-off\encryption-openssl\threading-multi\*.lib" "$OUTPUT_DIRECTORY\$platform\lib\$configuration\*"

    # Copy system
    xcopy /y "bin.v2\libs\system\build\msvc-12.0\$configuration\boost-link-shared\boost-source\$($extra)deprecated-functions-off\encryption-openssl\threading-multi\*.dll" "$OUTPUT_DIRECTORY\$platform\bin\$configuration\*"
    xcopy /y "bin.v2\libs\system\build\msvc-12.0\$configuration\boost-link-shared\boost-source\$($extra)deprecated-functions-off\encryption-openssl\threading-multi\*.pdb" "$OUTPUT_DIRECTORY\$platform\bin\$configuration\*"
    xcopy /y "bin.v2\libs\system\build\msvc-12.0\$configuration\boost-link-shared\boost-source\$($extra)deprecated-functions-off\encryption-openssl\threading-multi\*.lib" "$OUTPUT_DIRECTORY\$platform\lib\$configuration\*"

    Pop-Location
}

Compile-Libtorrent "win32" "debug"
Output-Libtorrent  "win32" "debug"

Compile-Libtorrent "win32" "release"
Output-Libtorrent  "win32" "release"

Output-Boost "win32" "debug"
Output-Boost "win32" "release"

# Output headers
xcopy /y "$(Join-Path $LIBTORRENT_DIRECTORY include)\*" "$(Join-Path $OUTPUT_DIRECTORY win32\include)\*" /E
xcopy /y "$(Join-Path $BOOST_DIRECTORY boost)\*" "$(Join-Path $OUTPUT_DIRECTORY win32\include)\boost\*" /E

# Package with NuGet

copy hadouken.libtorrent.nuspec $OUTPUT_DIRECTORY

pushd $OUTPUT_DIRECTORY
Start-Process "$NUGET_TOOL" -ArgumentList "pack hadouken.libtorrent.nuspec -Properties version=$VERSION" -Wait -NoNewWindow
popd
