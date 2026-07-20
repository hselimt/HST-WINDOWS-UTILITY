<div align="center">

# 🤝 Contributing to HST WINDOWS UTILITY

**Thanks for your attention! Contributions are welcome.**

</div>

---

## 📋 What to Contribute

- **New optimization features**
- **Bug fixes**
- **Performance enhancements**
- **Code refactoring**
- **Documentation improvements**

---

## 🏗️ Building & Running

### GUI

**Full production build** (from `GUI/`) guaranteed to produce a working package:
```
Build-GUI.bat
```
It cleans previous build output, builds the React frontend, publishes the self-contained .NET backend, copies both into place, then packages everything with electron-builder into an NSIS installer under `dist/`.

- **Backend only** — from `GUI/`, run `dotnet run`. Serves the API (and any existing `wwwroot/` static files) on `http://localhost:5200`.
- **Frontend only** — from `GUI/View/`, run `npm start`. This starts the CRA dev server; note `App.js` calls the hardcoded `http://localhost:5200` directly (not a relative path), so a real backend must already be running via `dotnet run` for API calls to work, regardless of CRA's dev proxy.

### CLI

No build step — edit and run the `.cmd` file directly.

---

## 📝 Guidelines

### General
- ✅ Test changes thoroughly before submitting
- ✅ Follow existing code style and patterns
- ✅ Update README/CHANGELOG if needed
- ✅ Run as Administrator when testing

### Backend Code Standards
- ✅ Use `ConfigLoader` for JSON configuration loading
- ✅ Return proper HTTP status codes (500 for errors, 400 for validation)
- ✅ Use `Logger.Log()` and `Logger.Error()` for all operations
- ✅ Use system-aware paths (`Environment.SpecialFolder`, `Paths` class) - no hardcoded paths
- ✅ Use `async/await` with `Async` suffix on method names
- ✅ Continue on failures for removal operations (partial success acceptable)

### Configuration Files
- ✅ Keep JSON files properly formatted and validated

---

## 📞 Contact

[![Email](https://img.shields.io/badge/Email-hselimt%40gmail.com-D14836?style=for-the-badge&logo=gmail&logoColor=white)](mailto:hselimt@gmail.com)

</div>