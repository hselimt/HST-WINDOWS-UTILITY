# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this is

HST WINDOWS UTILITY is a Windows optimization tool (registry tweaks, service management, task scheduler debloat, system cleanup, app/bloatware removal) shipped as two independent, feature-equivalent products:

- **`GUI/`** — Electron desktop app wrapping a local ASP.NET Core 8 backend + React 18 frontend.
- **`CLI/`** — A single self-contained Windows batch script (`HST WINDOWS UTILITY - SCRIPT VERSION v1.8.0.cmd`) implementing the same optimizations without any GUI dependency.

Both require Administrator privileges — nearly every operation is a registry write, service change, or scheduled task edit that silently fails without elevation.

## Architecture (GUI)

Three-process model at runtime:
1. **Electron shell** (`GUI/main.js`) — spawns the published .NET backend as a child process, polls `http://localhost:5200` until it's up, then loads that URL into a frameless `BrowserWindow`. On quit it kills the backend child process. Also handles: single-instance enforcement (`requestSingleInstanceLock`, quitting if another instance already holds it, focusing the existing window on a `'second-instance'` event); a native error dialog (`dialog.showErrorBox`) if the backend fails to start within `waitForServer`'s retry window, instead of quitting silently; external links (`setWindowOpenHandler` + a `will-navigate` guard) routed to the OS default browser via `shell.openExternal` rather than navigating inside the app window; and `app.disableHardwareAcceleration()`, called before `app.whenReady()` to avoid a GPU-compositor paint failure that otherwise renders a blank window.
2. **ASP.NET Core backend** (`GUI/Program.cs`, `GUI/Controllers/`) — serves the React build as static files AND exposes the REST API on port 5200. There is only one controller, `SystemController` (`GUI/Controllers/Controller.cs`), which fronts every feature as a POST/GET endpoint under `api/system/*`.
3. **React frontend** (`GUI/View/src/App.js`) — a single large component file (App.js, no router) that calls the backend via plain `fetch("http://localhost:5200/api/system/...")`. There is no build-time API client/SDK — endpoints are called by string literal.

Backend feature modules live under `GUI/Controllers/<Area>/` and are registered as scoped services in `Program.cs`:
- `Registry/RegistryOptimizer.cs` — registry tweaks (grouped into private methods like `OptimizePriorityAndPower`, `DisableGameFeatures`, `OptimizeLatency`, etc., all invoked from `OptimizeRegistryAsync`)
- `Services/SetServices.cs`, `Services/DisableUpdates.cs` — Windows service enable/disable
- `System/TaskSchedulerOptimizer.cs`, `System/SetPowerPlan.cs`, `System/CleanUp.cs`, `System/RestorePointCreator.cs`, `System/SystemInfoDto.cs`
- `Debloat/Debloater.cs` — app/package removal (MS apps, Xbox, Store, Edge, OneDrive, startup apps)
- `Helpers/` — shared infrastructure (see below)

Shared helpers (`GUI/Controllers/Helpers/`), used consistently across every feature module:
- `ConfigLoader.cs` — loads JSON config from the app base directory into typed dictionaries; used instead of hardcoding lists of services/apps/tasks to modify.
- `ProcessRunner.cs` — runs external commands elevated (`Verb = "runas"`) with a timeout, wrapping failures as exceptions.
- `Logger.cs` — writes timestamped entries to `%TEMP%\HST-WINDOWS-UTILITY.log`; log is reset (`InitializeLog`) on every app start. Every controller/service catches exceptions and calls `Logger.Error(operation, ex)`.
- `Paths.cs` — centralizes `Environment.SpecialFolder`-based paths (AppData, ProgramData, Desktop, etc.) so nothing hardcodes user-specific paths.
- `FileManager.cs` — file/dir cleanup helpers.

Data-driven config (JSON files at `GUI/` root, copied to the output dir on build via `.csproj` `<None Update>` entries):
- `ServicesConfig.json` — services grouped by category (`recommended`, `bluetooth`, `hyperv`, `xbox`), each with a `service` (short name), `name`, and `defaultStartup` (used for revert).
- `AppsConfig.json` — app/package lists (`startupApps`, `msApps`, `xboxApps`, `storeApps`).
- `ScheduledTasksConfig.json` — scheduled tasks to disable.
- `GUIDConfig.json` — power plan GUIDs used when reverting to Windows defaults.
- `wwwroot/Powerplan/HST.pow` — the custom power plan imported/activated by `SetPowerPlan.cs`; also kept as `GUI/HST.pow` and synced into `wwwroot/Powerplan/` at build/publish time (see `Build-GUI.bat` step 1, and the `CopyPowerPlanFile` target in the `.csproj`).

Controller endpoint pattern (`SystemController`): every action-taking endpoint follows the same shape — try/catch around the operation, `Logger.Error` on failure, and a JSON body of `{ status, message, success }` (200 on success, 400 for missing/invalid options, 500 on operation failure). Endpoints that accept a feature subset take an `Options` DTO (e.g. `ServiceOptions`, `DebloatOptions`, `CleanUpOptions`, `RevertOptions`) with boolean flags per category — follow this pattern for any new endpoint rather than inventing a new response shape.

Revert is symmetric: `RevertConfigurations` accepts a `RevertOptions` DTO and independently reverts services/tasks/updates/registry, continuing past individual failures ("continue on failure" — partial revert success is acceptable and expected, per CONTRIBUTING.md).

## Architecture (CLI)

`CLI/HST WINDOWS UTILITY - SCRIPT VERSION v1.8.0.cmd` is a monolithic batch script (~1900 lines): an admin-privilege check, a `:MAIN_MENU` ASCII-art menu keyed by numeric choice, and labeled sections implementing each optimization inline using native `reg`, `sc`, `schtasks`, `powercfg`, `dism`/`winget`, etc. It intentionally has no external dependencies (no PowerShell modules, no .NET) so it starts instantly — this is its main differentiator from the GUI, which needs its backend to spin up first.

## Build & run

**GUI — full production build** (from `GUI/`):
```
Build-GUI.bat
```
This is the canonical build path — it does, in order: `dotnet clean` + wipe `bin/obj/dist/node_modules/wwwroot`, sync `HST.pow` into `wwwroot/Powerplan/`, `npm install && npm run build` in `View/` (React), copy the React build into `wwwroot/`, `dotnet publish -c Release -r win-x64 --self-contained true`, copy publish output into `bin/`, delete the leftover `bin\Release\` intermediate build tree (a duplicate of the publish output that `dotnet publish` leaves behind — left uncleaned it roughly doubles the packaged app's size, since electron-builder's `bin/**/*` glob picks it up too), `npm install && npm run build` at the `GUI/` root (Electron/electron-builder → NSIS installer in `dist/`), then launches the built exe. Treat this script as the source of truth for the build order — do not assume `dotnet build`/`npm run build` alone reproduces a working package, since the Electron shell expects the published self-contained backend under `bin/` and the React build under `wwwroot/`.

Two other packaging-size settings to be aware of when touching the build: `package.json`'s `build.electronLanguages: ["en-US"]` prunes Electron's bundled Chromium locale files down to just English (the app has no localized UI, so the other ~54 locales are dead weight); and `HST-WINDOWS-UTILITY.csproj`'s `<CopyRefAssembliesToPublishDirectory>false</CopyRefAssembliesToPublishDirectory>` stops the publish output from including compile-time-only reference assemblies (`ref/`), which the app never needs at runtime (no Razor views requiring runtime compilation).

**GUI — iterate on backend only** (from `GUI/`):
```
dotnet run
```
Serves the API (and any existing `wwwroot/` static files) on `http://localhost:5200`.

**GUI — iterate on frontend only** (from `GUI/View/`):
```
npm start
```
CRA dev server; `package.json` sets `"proxy": "https://localhost:5200"` for API calls during development. Note `App.js` itself calls the hardcoded `http://localhost:5200` (not a relative path), so the backend must actually be running locally regardless of the CRA proxy.

**GUI — Electron shell alone** (from `GUI/`):
```
npm start   # electron .
```
Expects a built backend at `GUI/bin/HST-WINDOWS-UTILITY.exe` (see `getBackendPath()` in `main.js`) — run the full `Build-GUI.bat` or at least `dotnet publish` first.

**CLI**: no build step — run the `.cmd` file directly (as Administrator).

There is no automated test suite in this repo (see CONTRIBUTING.md — "Test changes thoroughly before submitting" is manual, run-as-Administrator verification against a real/VM Windows install).

## Conventions (from CONTRIBUTING.md)

- Load config via `ConfigLoader`, never hardcode service/app/task lists in code.
- Return `{status, message, success}` JSON with proper HTTP codes: 400 for missing/invalid input, 500 for operation failure.
- Use `Logger.Log()` / `Logger.Error()` for all operations (this is also the only diagnostic surface end users have — `%TEMP%\HST-WINDOWS-UTILITY.log`).
- Use `Environment.SpecialFolder` / the `Paths` class for filesystem paths — never hardcode user-specific paths.
- Async methods end in `Async` and use `async/await`.
- Removal/disable operations should continue past individual failures rather than aborting the whole batch (partial success is acceptable and expected).
