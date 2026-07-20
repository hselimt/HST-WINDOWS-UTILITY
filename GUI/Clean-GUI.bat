@echo off
setlocal enabledelayedexpansion
echo ========================================
echo       HST-WINDOWS-UTILITY CLEAN
echo ========================================
echo.
set "BASE_DIR=%~dp0"
cd /d "%BASE_DIR%"

echo [1/1] Cleaning build artifacts...
echo   ^> Running dotnet clean...
call dotnet clean --nologo >nul 2>&1
echo   ^> Removing build directories...
if exist "bin" rd /s /q "bin" 2>nul
if exist "obj" rd /s /q "obj" 2>nul
if exist "dist" rd /s /q "dist" 2>nul
if exist "node_modules" rd /s /q "node_modules" 2>nul
if exist "View\node_modules" rd /s /q "View\node_modules" 2>nul
if exist "View\build" rd /s /q "View\build" 2>nul

echo   ^> Cleaning wwwroot (preserving Powerplan)...
if exist "wwwroot" (
    for /d %%D in ("wwwroot\*") do (
        if /i not "%%~nxD"=="Powerplan" rd /s /q "%%D"
    )
    for %%F in ("wwwroot\*") do del /q "%%F"
)
if not exist "wwwroot\Powerplan" mkdir "wwwroot\Powerplan"

echo   ^> Clean complete!
echo.
pause