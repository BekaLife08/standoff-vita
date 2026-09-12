@echo off
echo ============================================
echo   Standoff Vita - Build VPK
echo ============================================
echo.
echo This script will:
echo   1. Kill any running Unity
echo   2. Run Unity IL2CPP build for PSP2
echo   3. Extract source.zip
echo   4. Fix sce_module, param.sfo, livearea
echo   5. Patch eboot.bin (auth_id fix)
echo   6. Pack into VPK
echo.
echo Starting at %TIME%
echo.

powershell -ExecutionPolicy Bypass -NoProfile -File "%~dp0build_vita.ps1"
set EXIT_CODE=%ERRORLEVEL%

echo.
if %EXIT_CODE% EQU 0 (
    echo ============================================
    echo   BUILD SUCCESSFUL!
    echo   Output: C:\StandoffProj\Build\Standoff2_Vita.vpk
    echo ============================================
) else (
    echo ============================================
    echo   BUILD FAILED (exit code %EXIT_CODE%)
    echo   Check the log above for errors
    echo ============================================
)
echo.
echo Finished at %TIME%
pause
