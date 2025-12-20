@echo off
setlocal enabledelayedexpansion

echo ========================================
echo    HST-WINDOWS-UTILITY CLEAN BUILD
echo ========================================
echo.

set "BASE_DIR=%~dp0"
cd /d "%BASE_DIR%"

:: Step 1: Clean previous builds
echo [1/8] Cleaning previous build artifacts...
echo   ^> Running dotnet clean...
call dotnet clean --nologo >nul 2>&1

echo   ^> Removing build directories...
if exist "bin" rd /s /q "bin" 2>nul
if exist "obj" rd /s /q "obj" 2>nul
if exist "dist" rd /s /q "dist" 2>nul
if exist "node_modules" rd /s /q "node_modules" 2>nul
if exist "View/node_modules" rd /s /q "View/node_modules" 2>nul
if exist "wwwroot" rd /s /q "wwwroot" 2>nul

echo   ^> Recreating wwwroot structure...
mkdir "wwwroot\Powerplan" 2>nul

echo   ^> Copying .pow file to Powerplan folder...
if exist "wwwroot\Powerplan\HST.pow" (
    echo   ^> HST.pow found in Powerplan folder
) else if exist "HST.pow" (
    copy /Y "HST.pow" "wwwroot\Powerplan\HST.pow" >nul
    echo   ^> Copied HST.pow from GUI root
) else (
    echo   ^> WARNING: HST.pow not found, continuing without it
)

echo   ^> Clean complete!
echo.

:: Step 2: Build React frontend
echo [2/8] Building React frontend...
cd /d "%BASE_DIR%View"

echo   ^> Installing npm dependencies...
call npm install --loglevel=error
if errorlevel 1 (
    echo   ^> ERROR: npm install failed!
    pause
    exit /b 1
)

echo   ^> Building React app...
call npm run build --loglevel=error
if errorlevel 1 (
    echo   ^> ERROR: React build failed!
    pause
    exit /b 1
)

echo   ^> React build complete!
echo.

:: Step 3: Copy React build to wwwroot
echo [3/8] Copying React build to wwwroot...
cd /d "%BASE_DIR%"

if not exist "View\build" (
    echo   ^> ERROR: React build output not found!
    pause
    exit /b 1
)

echo   ^> Copying files...
xcopy /E /I /Y /Q "View\build\*" "wwwroot\" >nul
if errorlevel 1 (
    echo   ^> ERROR: Failed to copy React build to wwwroot!
    pause
    exit /b 1
)

if not exist "wwwroot\index.html" (
    echo   ^> ERROR: index.html not found in wwwroot!
    pause
    exit /b 1
)

echo   ^> Copy complete!
echo.

:: Step 4: Build .NET backend
echo [4/8] Building .NET backend...
cd /d "%BASE_DIR%"

echo   ^> Publishing backend (this may take a few minutes)...
call dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true --nologo
if errorlevel 1 (
    echo   ^> ERROR: .NET publish failed!
    pause
    exit /b 1
)

echo   ^> .NET build complete!
echo.

:: Step 5: Copy publish output to bin root
echo [5/8] Preparing backend for Electron...

if not exist "bin\Release\net8.0\win-x64\publish" (
    echo   ^> ERROR: Publish output not found!
    pause
    exit /b 1
)

echo   ^> Copying publish output to bin...
xcopy /E /I /Y /Q "bin\Release\net8.0\win-x64\publish\*" "bin\" >nul
if errorlevel 1 (
    echo   ^> ERROR: Failed to copy publish output!
    pause
    exit /b 1
)

if not exist "bin\HST-WINDOWS-UTILITY.exe" (
    echo   ^> ERROR: Backend executable not found!
    pause
    exit /b 1
)

echo   ^> Backend ready!
echo.

:: Step 6: Verify wwwroot in bin
echo [6/8] Verifying wwwroot in backend output...

if not exist "bin\wwwroot\index.html" (
    echo   ^> WARNING: wwwroot not found in bin, copying...
    xcopy /E /I /Y /Q "wwwroot\*" "bin\wwwroot\" >nul

    if not exist "bin\wwwroot\index.html" (
        echo   ^> ERROR: Failed to copy wwwroot to bin!
        pause
        exit /b 1
    )
)

echo   ^> wwwroot verified!
echo.

:: Step 7: Build Electron app
echo [7/8] Building Electron application...
cd /d "%BASE_DIR%"

echo   ^> Installing Electron dependencies...
call npm install --loglevel=error
if errorlevel 1 (
    echo   ^> ERROR: npm install failed!
    pause
    exit /b 1
)

echo   ^> Building Electron package...
call npm run build --loglevel=error
if errorlevel 1 (
    echo   ^> ERROR: Electron build failed!
    pause
    exit /b 1
)

echo   ^> Electron build complete!
echo.

:: Step 8: Verify final output
echo [8/8] Verifying build output...

set "EXE_PATH=%BASE_DIR%dist\HST-WINDOWS-UTILITY-1.6.0.exe"

if not exist "%EXE_PATH%" (
    echo   ^> ERROR: Final executable not found!
    echo   ^> Expected: %EXE_PATH%
    echo.
    echo   ^> Contents of dist folder:
    dir /b "%BASE_DIR%dist" 2>nul
    pause
    exit /b 1
)

for %%A in ("%EXE_PATH%") do set "FILE_SIZE=%%~zA"
set /a "SIZE_MB=!FILE_SIZE! / 1048576"

echo   ^> Executable found: !SIZE_MB! MB
echo.

echo ========================================
echo      BUILD COMPLETED SUCCESSFULLY!
echo ========================================
echo.
echo Output: %EXE_PATH%
echo.
echo Launching application in 3 seconds...
echo (Press Ctrl+C to cancel)
echo.

timeout /t 3 /nobreak >nul

echo Starting HST-WINDOWS-UTILITY...
start "" "%EXE_PATH%"

echo.
echo Application launched! Check the taskbar.
echo If you see issues, check the console output above.
echo.
echo Build script completed!
echo.
pause
