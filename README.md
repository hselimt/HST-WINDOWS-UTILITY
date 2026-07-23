<div align="center">
  
# HST WINDOWS UTILITY
<p>
  <img src="https://img.shields.io/github/downloads/hselimt/HST-WINDOWS-UTILITY/total?style=for-the-badge&logo=github&label=Downloads"/>
</p>

**Windows optimization tools designed to maximize system performance through registry tweaks, service management, and system cleanup. Perfect for gamers and power users seeking maximum hardware efficiency.**
</div>

> [!WARNING]
> This tool makes aggressive system changes. Read the documentation before running any features  **GUI (? icon in top-right) and the CLI ([H] Help)**.

https://github.com/user-attachments/assets/063aa012-9401-460f-9741-def6cb5f6398

<table>
<tr>
<td align="center"><b>🖥️ GUI</b></td>
<td align="center"><b>⌨️ CLI</b></td>
</tr>
<tr>
<td><img src="./GUI/HST.png" width="500"/></td>
<td><img src="./CLI/HST.png" width="500"/></td>
</tr>
<tr>
<td valign="top">

Installer (`.exe`). Graphical interface.

- ~350 MB disk space
- .NET 8.0 Runtime (bundled)
- Windows 10/11 64-bit
- Administrator required
- Docs: **?** icon, top-right

</td>
<td valign="top">

Portable script. No installer, no runtime, no leftovers.

- A few hundred KB
- Same feature set as the GUI
- Windows 10/11 64-bit
- Administrator required
- Docs: **[H] Help**

</td>
</tr>
</table>

<div align="center">

[![Download](https://img.shields.io/badge/Download-Latest%20Release-success?style=for-the-badge&logo=github)](https://github.com/hselimt/HST-WINDOWS-UTILITY/releases)

</div>

---

## ✨ Features

### 🛡️ System Management
- **Restore Point Creation** — Automatic system restore points for safe rollback
- **Registry Optimization** — 120+ performance-focused tweaks (gaming, latency, privacy, telemetry)
- **Task Scheduler** — Disable 60+ unnecessary scheduled tasks
- **Windows Updates** — Disable automatic updates and delivery optimization
- **Visual Effects** — Reduce animations for better performance
- **Dark Mode** — System-wide dark theme toggle
- **Power Plan** — Custom high-performance power plan
- **Startup Apps** — Remove common apps from startup (Discord, Steam, Spotify, etc.)
- **Logging** — All operations logged to `%TEMP%\HST-WINDOWS-UTILITY.log`

### ⚙️ Services Management
Disable services by category:
| Category | Count | Examples |
|----------|-------|----------|
| Recommended | 107 | Telemetry, diagnostics, rarely-used services |
| Bluetooth | 5 | Audio gateway, support services |
| Hyper-V | 11 | All virtualization services |
| Xbox | 4 | Game save, networking, auth |

### 🧹 Cleanup & Debloat
- **Temp files** — Windows temp, user temp, prefetch
- **Cache** — Chrome cache, Windows Update cache
- **Event logs** — Clear all Windows logs
- **Power plans** — Remove default Windows plans
- **Debloat** — Remove 45+ Microsoft apps, Edge, OneDrive, Xbox apps, Store

## 🧪 Testing & Validation
Every button, toggle, and script in this utility has been tested across multiple clean installation environments using virtual machines:
- Windows 10 22H2
- Windows 11 21H2/22H2/23H2/24H2/25H2

---

## ⛔ Before You Install

| Warning | Why it matters |
|---------|----------------|
| **Create a restore point first** | Built-in button available — use it before any changes |
| **OneDrive removal deletes files** | Synced files are removed. Disable sync first |
| **App removals are permanent** | Microsoft Store apps need Windows reset to restore |
| **Revert ≠ Undo** | Restores Windows defaults, not your custom settings |

<details>
<summary><b>🚫 Skip these features if...</b></summary>

| Your situation | What to skip | Why |
|----------------|--------------|-----|
| Laptop user | Power plan, registry tweaks | Kills battery life, breaks sleep/hibernate |
| VM user (VirtualBox, Hyper-V, etc.) | Registry tweaks, Hyper-V services | Breaks guest integration, snapshots |
| Use a printer | Recommended services | Disables Print Spooler |
| Bluetooth headphones/mouse | Bluetooth services | Devices won't connect |
| Remote Desktop user | Recommended services | Disables RDP services |
| Unstable hardware | Power plan | Driver may not initialize after cold boot, you would have to restart |
| Web Applications | HST Essential Services | Some web apps requires some services that this software disables |

</details>

---

<details>
<summary><b>🛠️ App won't start?</b></summary>

| Issue | Fix |
|-------|-----|
| Nothing happens when launching | Unblock the .exe — right-click → Properties → check **Unblock** → Apply |
| Operations fail silently | Must run as Administrator |
| Config file error | Re-run as Administrator |
| Need to see what happened | Check `%TEMP%\HST-WINDOWS-UTILITY.log` |
| Need to undo everything | Revert panel or Windows System Restore |

</details>

---

## 🛠️ Tech Stack

| Component | Technologies |
|-----------|--------------|
| **Backend** | C# / ASP.NET Core 8, PowerShell, Windows Registry APIs |
| **Frontend** | React 18, Electron 27, Lucide Icons |
| **CLI** | Batch scripting, native Windows commands |

---

## 🤝 Contributing

See [CONTRIBUTING](CONTRIBUTING.md) for guidelines.
