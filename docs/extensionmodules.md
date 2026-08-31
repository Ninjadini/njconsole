---
title: Extension Modules
nav_section: "Advanced"
nav_order: 8
nav_icon: "🧱"
---

# 🧱 Extension Modules / Addons

Extensions are plug-and-play modules — an easy way to move a feature between projects or share one with the
community. Pick only what your project needs and the console stays lightweight.

## 🧩 The extension points

Everything below is an `IConsoleModule`. Add `IConsoleExtension` too and NjConsole creates it for you from
Project Settings instead of you registering it in code.

| Interface | What it lets you add | Docs |
|---|---|---|
| `IConsolePanelModule` | A whole new panel (UI Toolkit) | [Custom Panels](custompanels.md) |
| `IConsoleIMGUIPanelModule` | A new panel drawn with OnGUI / IMGUI | [Custom Panels](custompanels.md) |
| `IConsoleCommandlineModule` | A custom command executor or an interactive prompt | [Command Line](commandline.md#-adding-a-custom-executor) |
| `IConsoleOverlayTrigger` | Your own way to open the runtime overlay | [Console Window](consolewindow.md#️-setting-up-your-own-activation-trigger) |
| `IConsoleAccessChallenge` | A gate before the console opens (e.g. your login) | [Console Window](consolewindow.md#-setting-up-your-own-custom-access-challenge-type) |
| `IConsoleTimestampFormatter` | How timestamps are drawn in the Logs panel | [Logging](logging.md#-customize-timestamp-format-in-logs-panel) |
| `IConsoleLogExportFormatter` | Header / footer / line format for exported and emailed logs | [Logging](logging.md#-customize-the-exported--emailed-logs) |
| `IConsoleExtension` alone | Anything else — run setup code when the console starts | below |

Two `IConsoleModule` members worth knowing:
- `PersistInEditMode` — `true` to survive leaving play mode, needed for anything that works in the editor
  window outside play mode. Defaults to `false`.
- `OnAdded(ConsoleModules modules)` / `Dispose()` — setup and teardown.

> ⚠️ Implement `OnAdded` **explicitly** (`void IConsoleModule.OnAdded(...)`) or make it `public`.
> A private `void OnAdded(...)` will silently not be called.

## 📦 External Extensions Repo

You can find some extensions here:  
[https://github.com/Ninjadini/njconsole/tree/main/extension-modules](https://github.com/Ninjadini/njconsole/tree/main/extension-modules)  
Each usually has a README or comments on how to include it.
> Early days — there may not be much there yet.


## ✨ Create Your Own Extension

1. Implement `IConsoleExtension`.
2. Add `[Serializable]` — that's what makes its fields editable in Project Settings.
3. Keep it a plain data class, **not** a `MonoBehaviour` or `ScriptableObject`.
4. Go to `Project Settings` > `Ninjadini ⌨ Console` > `Extension Modules` and add it.
5. Press `Apply extension changes`.

A simple one that registers an option menu item:

```csharp
[Serializable]
public class ExampleExtension : IConsoleExtension
{
    void IConsoleModule.OnAdded(ConsoleModules modules)
    {
        var catalog = modules.GetOrCreateModule<ConsoleOptions>().CreateCatalog();

        catalog.AddButton("Extensions/Say Hello", () =>
        {
            Debug.Log("Hello!");
        });
    }
}
```

Public serializable fields appear as editable settings next to it, so an extension can ship its own config.

## 🔧 Registering in code instead

For a module created at runtime, or one that shouldn't be in every build, add it directly:

```csharp
if (NjConsole.Modules.GetModule(typeof(MyModule)) == null)
{
    NjConsole.Modules.AddModule(new MyModule());
}
```
Other `ConsoleModules` APIs: `GetOrCreateModule<T>()`, `GetModule<T>(includeSubClasses)`,
`RemoveModule(type)`, `HasModule(module)`, and the `ModuleAdd` / `ModuleRemoved` events.

[NjConsole doc home](index.md)
