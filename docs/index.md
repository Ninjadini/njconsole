---
title: Overview
nav_title: Overview
nav_order: 0
nav_icon: "🏠"
---

# Ninjadini Debug Console [NjConsole] for Unity

NjConsole is the ultimate debug console you never knew you needed — until now.

- 📜 Runtime log viewer with powerful filtering and search (AND, OR, NOT, etc.)
- ⚡ Ultra-fast logging with near-zero allocations for high-performance debugging
- 🔍 Object inspector — view/edit fields and properties, follow references, and call methods directly
- 🔗 Log entries can link to objects — tap to inspect and tweak them instantly
- 📂 Hierarchy viewer — browse live GameObjects and their components, then inspect them in detail using the object inspector
- 🎮 Custom menu options and keybindings — for cheats, dev tools, and prototyping
- ⌨️ Command Line — To call menu options. With commands history, autocompletion and multi-params support.
- 🎯 Quick-access shortcuts — assign to any screen corner for rapid access while testing
- 🖥️ Cross-platform support — works in Unity Editor, standalone, WebGL, and mobile (supports safe areas, touch input, keyboard, and all orientations)
- 🖼️ Dockable editor window and in-game floating overlay for large screens
- 🚀 Plug-and-play setup — NjConsole starts automatically. No configuration required.
- 🧰 Built-in utilities — FPS/memory monitors, PlayerPrefs editor, live screen & quality settings, and device info.
- 🧱 Modular design — plug in your own modules and panels
- ✂️ Removable — Easily disable NjConsole at compile-time (saves memory, build size, and prevents unintended access)
- 🧾 Full C# source with XML-documented public APIs.

⚠️ Unity 2022.3 or newer is required
NjConsole relies on Unity’s UI Toolkit, which became stable for runtime use in 2022.3 LTS.

🏷️ Latest version: {{ site.version }}

---

## 📚 Documentation Topics

**Core**

- [Console Window](consolewindow.md) — opening it, activation triggers, access challenges

- [Logging & Logs panel](logging.md) — NjLogger, filtering, log routing, custom handlers

- [Options Menu / Cheats](optionsmenu.md) — building your debug menu, keybindings, shortcuts

- [Command Line](commandline.md) — running options as commands, variables, scopes

- [Inspector & Utilities](inspector.md) — the object inspector, scene tree, monitors and tools

**Advanced**

- [Custom Panels](custompanels.md) — add your own panel, in play mode or the editor

- [Build customization](buildcustomization.md) — trimming features per platform, stripping with `NJCONSOLE_DISABLE`

- [Extension Modules](extensionmodules.md) — packaging features as plug-and-play modules

- [Hidden Gems](hiddengems.md) — toasts, text prompts, custom graphs and other public API without its own page

Please also try the provided Demo scene inside Demo folder.

---

[🌐 Web Demo](https://njconsole.ninjadini.com/demo/)

[🚀 Getting Started PDF](GettingStarted.pdf)  

[🛒 Unity Asset Store](https://assetstore.unity.com/packages/slug/319982)

[📽️ Video](https://www.youtube.com/watch?v=J_qv3g_NY3U)

[📝 Change Log](CHANGELOG.md)

[🧯 Troubleshooting & Known Issues](troubleshooting.md)

---

### 🔒 License
Licensed under the [Unity Asset Store EULA](https://unity.com/legal/as-terms) as an **Extension Asset**.

Extension Assets are licensed **per seat** — a seat being one person, rather than one project, studio or
machine. So a team of ten people working in a Unity project that includes NjConsole needs ten seats.

Seat sales are what fund NjConsole's ongoing development, so thank you for sizing your license to your team.
