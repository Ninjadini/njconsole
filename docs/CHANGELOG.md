## 1.2.0
🆕 Command Line Support: Using the same paths as Option Menus. With commands history, autocompletion, multi-params support, variables & scopes. Please see online doc for details.   
✨ Log details text view now detects file paths and adds buttons to locate the file.   
✨ Static members can now be added to Option Menus by passing the type object, e.g. `CreateCatalogFrom(typeof(MyConsoleMenus)`.   
✨ New setting to pipe logs from Unity into the `unity` channel.   

## 1.1.1
🐛 Fixed compile error when NjConsole is disabled with warnings-as-errors enabled.
🐛 Fixed type search in Object Inspector not working in WebGL.

## 1.1.0
🆕 Option to disable and strip NjConsole using #ifdef NJCONSOLE_DISABLE. Can also be toggled via Project Settings or API.   
🆕 Stack traces are now clickable line by line — click any line in a stack trace (e.g. the third line) to jump to that method in your IDE.   
✨ Log details panel is now resizable by dragging its top edge.   
✨ Clicking a log referencing a Unity Object now pings it in the Hierarchy, similar to the default Unity Console.   
🐛 Fixed not going to the correct line number in IDE when double-clicking a log.   
🐛 Fixed initialization crash loop in some projects caused by `ConsoleSettings` asset creation.   
🐛 Fixed auto-scaling issues in Editor play mode on high-DPI screens.

## 1.0.1
🐛 Fixed shortcuts drag issues related to Unity 6.1

## 1.0.0
🚀 Our first version 🎉

---
[NjConsole doc home](index.md)