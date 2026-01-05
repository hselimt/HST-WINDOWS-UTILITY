<div align="center">
  
# HST WINDOWS UTILITY

<p>
<img src="https://img.shields.io/badge/Windows-10%20%7C%2011-0078D6?style=for-the-badge&logo=windows&logoColor=white"/>
<img src="https://img.shields.io/badge/C%23-ASP.NET%20Core-239120?style=for-the-badge&logo=csharp&logoColor=white"/>
<img src="https://img.shields.io/badge/React-18-61DAFB?style=for-the-badge&logo=react&logoColor=black"/>
<img src="https://img.shields.io/badge/Electron-27-47848F?style=for-the-badge&logo=electron&logoColor=white"/>
</p>

**Windows optimization tools designed to maximize system performance through registry tweaks, service management, and system cleanup. Perfect for gamers and power users seeking maximum hardware efficiency.**



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
</table>

</div>

---

## ⚠️ Before You Use

**This tool makes aggressive system changes.** Ideal for: gaming desktops, fresh Windows installs, power users who know what they're doing.

### ⛔ Critical

| Warning | Why it matters |
|---------|----------------|
| **Create a restore point first** | Built-in button available — use it before any changes |
| **OneDrive removal deletes files** | Synced files are removed. Disable sync first |
| **App removals are permanent** | Microsoft Store apps need Windows reset to restore |
| **Revert ≠ Undo** | Restores Windows defaults, not your custom settings |

<details>
<summary><b>🛠️ Troubleshooting</b></summary>

| Issue | Fix |
|-------|-----|
| Operations fail silently | Must run as Administrator |
| App won't start | Unblock the .exe (Properties → Unblock) |
| Config file error | Re-run as Administrator |
| Need to see what happened | Check `%TEMP%\HST-WINDOWS-UTILITY.log` |
| Need to undo everything | Revert panel or Windows System Restore |

</details>

<details>
<summary><b>🚫 Skip these features if...</b></summary>

| Your situation | What to skip | Why |
|----------------|--------------|-----|
| Laptop user | Power plan, registry tweaks | Kills battery life, breaks sleep/hibernate |
| VM user (VirtualBox, Hyper-V, etc.) | Registry tweaks, Hyper-V services | Breaks guest integration, snapshots |
| Work/School PC | Windows Update toggle | IT policies will conflict or flag you |
| Use a printer | Recommended services | Disables Print Spooler |
| Bluetooth headphones/mouse | Bluetooth services | Devices won't connect |
| Remote Desktop user | Recommended services | Disables RDP services |

</details>

<details>
<summary><b>🔧 What registry optimization actually does</b></summary>

| Disabled | What breaks |
|----------|-------------|
| Game Bar & DVR | No Xbox overlay, no clip recording, no Game Bar streaming |
| Location services | Weather widgets, Maps, "Find My Device" won't work |
| Webcam system access | Video calls fail until re-enabled in Privacy settings |
| Background apps | Apps won't update or sync when minimized |
| Notifications | No toast notifications from any app |
| Search indexing | File searches become slower |
| Windows Spotlight | No automatic lock screen wallpapers |
| Mouse acceleration | Raw 1:1 input — feels "slow" at first if you're used to acceleration |

**Still enabled:**
- Defender real-time protection (only scheduled scans disabled)
- Firewall
- Core security features

**Manually reversible:**
- Webcam: Settings → Privacy → Camera
- Location: Settings → Privacy → Location
- Notifications: Settings → System → Notifications

</details>

---

### 📥 Download

[![Download](https://img.shields.io/badge/Download-Latest%20Release-success?style=for-the-badge&logo=github)](https://github.com/hselimt/HST-WINDOWS-UTILITY/releases)

**After downloading Right-click .exe → Properties → Check "Unblock" → Apply → Run as Administrator**

![BLOCK](./BLOCK.png)

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
| Recommended | 108 | Telemetry, diagnostics, rarely-used services |
| Bluetooth | 4 | Audio gateway, support services |
| Hyper-V | 11 | All virtualization services |
| Xbox | 4 | Game save, networking, auth |

### 🧹 Cleanup & Debloat
- **Temp files** — Windows temp, user temp, prefetch
- **Cache** — Chrome cache, Windows Update cache
- **Event logs** — Clear all Windows logs
- **Power plans** — Remove default Windows plans
- **Debloat** — Remove 45+ Microsoft apps, Edge, OneDrive, Xbox apps, Store

---

## 🧪 Testing & Validation

To ensure system stability and safety, every function in this utility has been tested across multiple clean installation environments using virtual machines

| Operating System | Version | Status |
|------------------|---------|--------|
| Windows 10 | 22H2 | ✅ Verified |
| Windows 11 | 21H2 | ✅ Verified |
| Windows 11 | 22H2 | ✅ Verified |
| Windows 11 | 23H2 | ✅ Verified |
| Windows 11 | 24H2 | ✅ Verified |
| Windows 11 | 25H2 | ✅ Verified |

### Quality Assurance Process

- **Environment:** Tests were conducted on VirtualBox and Hyper-V simulating fresh Windows installations to isolate variables
- **Coverage:** Each button, toggle, and script was executed multiple times individually to verify functionality
- **Reversibility:** Revert functions were tested to ensure systems could be returned to default states without errors

---

## 🛠️ Tech Stack

| Component | Technologies |
|-----------|--------------|
| **Backend** | C# / ASP.NET Core 8, PowerShell, Windows Registry APIs |
| **Frontend** | React 18, Electron 27, Lucide Icons |
| **CLI** | Batch scripting, native Windows commands |

---

## 🔧 Requirements For GUI

- Windows 10/11 (64-bit)
- Administrator privileges
- 100 MB disk space
- .NET 8.0 Runtime (included in GUI executable)

---

## 👨‍💻 Development

**What I Built:**
- Years of Windows tweaking knowledge
- Complete C# backend with Windows system APIs
- Batch and PowerShell scripts/commands
- All optimization logic and safety checks
- CLI version with 20+ optimization modules
- Full integration and testing

**AI-Assisted:**
- React frontend UI
- Electron packaging setup
- Build configuration

---

## 🤝 Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines.

---

## 📞 Contact

[![Email](https://img.shields.io/badge/Email-hselimt%40gmail.com-D14836?style=for-the-badge&logo=gmail&logoColor=white)](mailto:hselimt@gmail.com)
