param(
    [string]$UnityExe = "C:\Program Files\Unity\Editor\Unity.exe",
    [string]$ProjectPath = "C:\Everything_for_the_project\StandoffProj",
    [string]$BuildOutput = "C:\StandoffProj\Build\PSP2",
    [string]$SdkSceModule = "C:\PSVITA\sdk\target\sce_module",
    [string]$WorkingParamSfo = "C:\Users\Rakhy\AppData\Local\Temp\vpk_fix\sce_sys\param.sfo",
    [string]$LiveAreaAssets = "C:\Everything_for_the_project\StandoffProj\LiveAreaAssets",
    [string]$VpkOutput = "C:\StandoffProj\Build\Standoff2_Vita.vpk",
    [string]$StagingDir = "C:\Users\Rakhy\AppData\Local\Temp\vpk_staging"
)

Add-Type -Assembly System.IO.Compression.FileSystem

function Write-Step($msg) { Write-Host "`n>>> $msg" -ForegroundColor Cyan }
function Write-Ok($msg) { Write-Host "    OK: $msg" -ForegroundColor Green }
function Write-Fail($msg) { Write-Host "    FAIL: $msg" -ForegroundColor Red }

# ============================================================
# STEP 1: Kill Unity
# ============================================================
Write-Step "Step 1: Killing Unity"
$procs = Get-Process -Name "Unity" -ErrorAction SilentlyContinue
if ($procs) {
    $procs | Stop-Process -Force
    Start-Sleep -Seconds 3
    Write-Ok "Killed $($procs.Count) Unity process(es)"
} else {
    Write-Ok "No Unity running"
}

# ============================================================
# STEP 2: Run Unity build
# ============================================================
Write-Step "Step 2: Running Unity IL2CPP build for PSP2"

$logFile = "$env:TEMP\unity_vita_build.log"
Remove-Item $logFile -Force -ErrorAction SilentlyContinue

# Clean old build output
if (Test-Path $BuildOutput) {
    Remove-Item $BuildOutput -Recurse -Force -ErrorAction SilentlyContinue
    Write-Ok "Cleaned old build output"
}

# Set SDK env vars so Unity finds the PSP2 SDK
$env:SCE_PSP2_SDK_DIR = "C:\StandoffProj\PSVITA\sdk"
$env:SCE_ROOT_DIR = "C:\StandoffProj\PSVITA\SCE"
Write-Host "    SCE_PSP2_SDK_DIR = $env:SCE_PSP2_SDK_DIR"
Write-Host "    SCE_ROOT_DIR = $env:SCE_ROOT_DIR"

Write-Host "    Unity: $UnityExe"
Write-Host "    Project: $ProjectPath"
Write-Host "    Log: $logFile"
Write-Host "    This will take several minutes..."

$buildStart = Get-Date
& "$UnityExe" -batchmode -nographics -quit `
    -projectPath "$ProjectPath" `
    -buildTarget PSP2 `
    -executeMethod BuildVita.Build `
    -logFile "$logFile" 2>&1 | Out-Null

$buildExit = $LASTEXITCODE
$buildTime = ((Get-Date) - $buildStart).TotalSeconds

if ($buildExit -ne 0) {
    Write-Fail "Unity exited with code $buildExit (took $([math]::Round($buildTime, 1))s)"
    Write-Host "`n    Last 20 lines of log:"
    if (Test-Path $logFile) {
        Get-Content $logFile -Tail 20 | ForEach-Object { Write-Host "    $_" }
    }
    exit 1
}
Write-Ok "Unity build completed in $([math]::Round($buildTime, 1))s"

# Find the source.zip
$sourceZip = Get-ChildItem "$BuildOutput\*.source.zip" -ErrorAction SilentlyContinue | Select-Object -First 1
if (-not $sourceZip) {
    Write-Fail "No .source.zip found in $BuildOutput"
    exit 1
}
Write-Ok "Found source.zip: $($sourceZip.Name) ($([math]::Round($sourceZip.Length / 1MB, 1)) MB)"

# ============================================================
# STEP 3: Extract source.zip and strip Files/ prefix
# ============================================================
Write-Step "Step 3: Extracting source.zip"

if (Test-Path $StagingDir) {
    Remove-Item $StagingDir -Recurse -Force
}

$zip = [IO.Compression.ZipFile]::OpenRead($sourceZip.FullName)
$entries = $zip.Entries
Write-Host "    $($entries.Count) entries in zip"

$extracted = 0
foreach ($entry in $entries) {
    if ($entry.FullName -eq "Files/" -or $entry.FullName -eq "savedata/" -or $entry.FullName -eq "savedata/persistentdata/") {
        continue
    }

    # Strip "Files/" prefix
    $relPath = $entry.FullName
    if ($relPath.StartsWith("Files/")) {
        $relPath = $relPath.Substring(6)
    }
    if ([string]::IsNullOrEmpty($relPath)) { continue }

    $destPath = Join-Path $StagingDir $relPath

    if ($entry.Length -eq 0 -and $entry.FullName.EndsWith("/")) {
        # Directory entry
        $dir = [IO.Path]::GetDirectoryName($destPath)
        if (-not (Test-Path $dir)) {
            New-Item -ItemType Directory -Path $dir -Force | Out-Null
        }
    } else {
        # File entry
        $dir = [IO.Path]::GetDirectoryName($destPath)
        if (-not (Test-Path $dir)) {
            New-Item -ItemType Directory -Path $dir -Force | Out-Null
        }
        [IO.Compression.ZipFileExtensions]::ExtractToFile($entry, $destPath, $true)
    }
    $extracted++
}
$zip.Dispose()
Write-Ok "Extracted $extracted files to $StagingDir"

# ============================================================
# STEP 4: Add missing sce_module files
# ============================================================
Write-Step "Step 4: Adding sce_module files"

$sceModuleDest = Join-Path $StagingDir "sce_module"
if (-not (Test-Path $sceModuleDest)) {
    New-Item -ItemType Directory -Path $sceModuleDest -Force | Out-Null
}

# Only copy files that are missing (don't overwrite libsmart which is already correct)
$sdkFiles = Get-ChildItem $SdkSceModule -File -ErrorAction SilentlyContinue
$added = 0
foreach ($f in $sdkFiles) {
    $dest = Join-Path $sceModuleDest $f.Name
    if (-not (Test-Path $dest)) {
        Copy-Item $f.FullName $dest -Force
        $added++
        Write-Host "    + $($f.Name) ($([math]::Round($f.Length / 1KB)) KB)"
    }
}
Write-Ok "Added $added missing sce_module files (total: $(Get-ChildItem $sceModuleDest | Measure-Object | Select-Object -ExpandProperty Count) files)"

# ============================================================
# STEP 5: Fix param.sfo
# ============================================================
Write-Step "Step 5: Fixing param.sfo"

$paramDest = Join-Path $StagingDir "sce_sys\param.sfo"
$oldSize = (Get-Item $paramDest -ErrorAction SilentlyContinue).Length
Copy-Item $WorkingParamSfo $paramDest -Force
$newSize = (Get-Item $paramDest).Length
Write-Ok "Replaced param.sfo: $oldSize -> $newSize bytes"

# ============================================================
# STEP 6: Fix livearea images
# ============================================================
Write-Step "Step 6: Fixing livearea images"

$liveareaDir = Join-Path $StagingDir "sce_sys\livearea\contents"
if (-not (Test-Path $liveareaDir)) {
    New-Item -ItemType Directory -Path $liveareaDir -Force | Out-Null
}

# bg01.png -> bg0.png (overwrite the small one from Unity)
$bgSrc = Join-Path $LiveAreaAssets "bg01.png"
$bgDest = Join-Path $liveareaDir "bg0.png"
Copy-Item $bgSrc $bgDest -Force
Write-Host "    + bg0.png ($([math]::Round((Get-Item $bgDest).Length / 1KB)) KB)"

# splash.png -> default_gate.png (overwrite the small one)
$splashSrc = Join-Path $LiveAreaAssets "splash.png"
$splashDest = Join-Path $liveareaDir "default_gate.png"
Copy-Item $splashSrc $splashDest -Force
Write-Host "    + default_gate.png ($([math]::Round((Get-Item $splashDest).Length / 1KB)) KB)"

# startup.png -> keep as-is (already there from Unity)
$startupSrc = Join-Path $LiveAreaAssets "startup.png"
if (Test-Path $startupSrc) {
    # Not strictly needed but ensures consistency
}

Write-Ok "Livearea images updated"

# ============================================================
# STEP 7: Patch eboot.bin (auth_id fix)
# ============================================================
Write-Step "Step 7: Patching eboot.bin"

$ebootPath = Join-Path $StagingDir "eboot.bin"
if (-not (Test-Path $ebootPath)) {
    Write-Fail "eboot.bin not found at $ebootPath"
    exit 1
}

$ebootSize = (Get-Item $ebootPath).Length
Write-Host "    eboot.bin size: $([math]::Round($ebootSize / 1MB, 1)) MB"

try {
    $fs = [IO.File]::Open($ebootPath, [IO.FileMode]::Open, [IO.FileAccess]::ReadWrite)
    $fs.Seek(0x80, [IO.SeekOrigin]::Begin) | Out-Null
    $fs.WriteByte(0x00)
    $fs.Close()
    Write-Ok "Patched byte at offset 0x80 to 0x00 (auth_id fix)"
} catch {
    Write-Fail "Failed to patch eboot.bin: $_"
    exit 1
}

# ============================================================
# STEP 8: Pack VPK
# ============================================================
Write-Step "Step 8: Packing VPK"

# Remove old VPK
if (Test-Path $VpkOutput) {
    Remove-Item $VpkOutput -Force
    Write-Host "    Removed old VPK"
}

Write-Host "    Creating ZIP archive..."
$vpkStart = Get-Date

# Create VPK (ZIP) from staging directory (stream-based for .NET 4.0 compat)
$allFiles = Get-ChildItem $StagingDir -Recurse -File
Write-Host "    $($allFiles.Count) files to pack"

$zipArchive = [IO.Compression.ZipFile]::Open($VpkOutput, [IO.Compression.ZipArchiveMode]::Create)
foreach ($file in $allFiles) {
    $entryName = $file.FullName.Substring($StagingDir.Length + 1).Replace("\", "/")
    $entry = $zipArchive.CreateEntry($entryName)
    $entryStream = $entry.Open()
    $fileStream = [IO.File]::OpenRead($file.FullName)
    $buffer = New-Object byte[] 65536
    $bytesRead = $fileStream.Read($buffer, 0, $buffer.Length)
    while ($bytesRead -gt 0) {
        $entryStream.Write($buffer, 0, $bytesRead)
        $bytesRead = $fileStream.Read($buffer, 0, $buffer.Length)
    }
    $fileStream.Dispose()
    $entryStream.Dispose()
}
$zipArchive.Dispose()

$vpkTime = ((Get-Date) - $vpkStart).TotalSeconds
$vpkSize = (Get-Item $VpkOutput).Length
Write-Ok "VPK created: $([math]::Round($vpkSize / 1MB, 1)) MB in $([math]::Round($vpkTime, 1))s"

# ============================================================
# STEP 9: Verify VPK structure
# ============================================================
Write-Step "Step 9: Verifying VPK"

$vpk = [IO.Compression.ZipFile]::OpenRead($VpkOutput)
$hasEboot = $vpk.Entries | Where-Object { $_.FullName -eq "eboot.bin" }
$hasParamSfo = $vpk.Entries | Where-Object { $_.FullName -eq "sce_sys/param.sfo" }
$hasIl2cpp = $vpk.Entries | Where-Object { $_.FullName -eq "Media/Modules/il2CppAssemblies.suprx" }
$hasMetadata = $vpk.Entries | Where-Object { $_.FullName -eq "Media/Metadata/global-metadata.dat" }

$checks = @(
    @("eboot.bin", $hasEboot),
    @("sce_sys/param.sfo", $hasParamSfo),
    @("Media/Modules/il2CppAssemblies.suprx", $hasIl2cpp),
    @("Media/Metadata/global-metadata.dat", $hasMetadata)
)

$allGood = $true
foreach ($check in $checks) {
    if ($check[1]) {
        Write-Host "    [OK] $($check[0])" -ForegroundColor Green
    } else {
        Write-Host "    [MISSING] $($check[0])" -ForegroundColor Red
        $allGood = $false
    }
}

# Check for forward slashes (no backslashes in VPK)
$badEntries = $vpk.Entries | Where-Object { $_.FullName.Contains("\") }
if ($badEntries) {
    Write-Host "    [WARN] $($badEntries.Count) entries have backslashes" -ForegroundColor Yellow
    $allGood = $false
} else {
    Write-Host "    [OK] All entries use forward slashes" -ForegroundColor Green
}

# Check eboot.bin size
$ebootEntry = $vpk.Entries | Where-Object { $_.FullName -eq "eboot.bin" }
if ($ebootEntry) {
    $ebootMB = [math]::Round($ebootEntry.Length / 1MB, 1)
    Write-Host "    [OK] eboot.bin: $ebootMB MB" -ForegroundColor Green
}

$totalEntries = ($vpk.Entries | Measure-Object).Count
Write-Host "    Total entries: $totalEntries"
$vpk.Dispose()

# ============================================================
# STEP 10: Cleanup
# ============================================================
Write-Step "Step 10: Cleaning up staging directory"
Remove-Item $StagingDir -Recurse -Force -ErrorAction SilentlyContinue
Write-Ok "Staging directory removed"

# ============================================================
# DONE
# ============================================================
Write-Host ""
Write-Host "============================================" -ForegroundColor Green
Write-Host "  BUILD COMPLETE!" -ForegroundColor Green
Write-Host "  Output: $VpkOutput" -ForegroundColor Green
Write-Host "  Size: $([math]::Round($vpkSize / 1MB, 1)) MB" -ForegroundColor Green
Write-Host "============================================" -ForegroundColor Green

exit 0
