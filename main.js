const { app, BrowserWindow } = require('electron');
const { spawn } = require('child_process');
const path = require('path');
const http = require('http');
const fs = require('fs');

let mainWindow;
let apiProcess;

app.requestSingleInstanceLock();

function waitForServer(url, maxAttempts = 30) {
    return new Promise((resolve, reject) => {
        let attempts = 0;
        const check = () => {
            http.get(url, (res) => {
                resolve();
            }).on('error', (err) => {
                attempts++;
                if (attempts >= maxAttempts) {
                    reject(new Error('Server failed to start'));
                } else {
                    setTimeout(check, 1000);
                }
            });
        };
        check();
    });
}

function getBackendPath() {
    if (app.isPackaged) {
        const unpackedPath = path.join(
            process.resourcesPath,
            'app.asar.unpacked',
            'bin',
            'HST WINDOWS UTILITY.exe'
        );

        if (fs.existsSync(unpackedPath)) {
            return unpackedPath;
        }
    }

    return path.join(__dirname, 'bin', 'HST WINDOWS UTILITY.exe');
}

function getIconPath() {
    if (app.isPackaged) {
        const locations = [
            path.join(process.resourcesPath, 'build', 'hst-high-resolution-logo-transparent.ico'),
            path.join(process.resourcesPath, 'hst-high-resolution-logo-transparent.ico'),
            path.join(__dirname, 'build', 'hst-high-resolution-logo-transparent.ico')
        ];

        for (const location of locations) {
            if (fs.existsSync(location)) {
                return location;
            }
        }
    }

    return path.join(__dirname, 'build', 'hst-high-resolution-logo-transparent.ico');
}

async function createWindow() {
    const exePath = getBackendPath();
    const iconPath = getIconPath();

    console.log("Backend path:", exePath);
    console.log("Icon path:", iconPath);

    if (!fs.existsSync(exePath)) {
        console.error(`Backend not found at: ${exePath}`);
        app.quit();
        return;
    }

    const exeDir = path.dirname(exePath);
    console.log("Working directory:", exeDir);

    apiProcess = spawn(exePath, [], {
        cwd: exeDir,
        windowsHide: true
    });

    apiProcess.on('error', (err) => {
        console.error('Failed to start backend:', err);
    });

    apiProcess.stdout?.on('data', (data) => {
        console.log(`Backend: ${data}`);
    });

    apiProcess.stderr?.on('data', (data) => {
        console.error(`Backend Error: ${data}`);
    });

    try {
        await waitForServer('http://localhost:5000');
    } catch (err) {
        console.error('Backend failed to start:', err);
        app.quit();
        return;
    }

    mainWindow = new BrowserWindow({
        width: 1000,
        height: 1000,
        resizable: false,
        maximizable: false,
        fullscreenable: false,
        autoHideMenuBar: true,
        frame: false,
        icon: iconPath,
        webPreferences: {
            nodeIntegration: false,
            contextIsolation: true
        }
    });

    mainWindow.setMenu(null);
    mainWindow.loadURL('http://localhost:5000');
}

app.whenReady().then(createWindow);

app.on('before-quit', () => {
    if (apiProcess) {
        apiProcess.kill();
    }
});

app.on('window-all-closed', () => {
    if (apiProcess) {
        apiProcess.kill();
    }
    app.quit();
});