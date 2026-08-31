---
title: Build Customization
nav_section: "Advanced"
nav_order: 7
nav_icon: "🔧"
---

# 💻 Build customization

## 🔧 Dynamically Configure Console Features During Build

Same switches as `Project Settings > Ninjadini ⌨ Console > Features` — you only need a build processor if
they differ per platform or build flavour.

| Field on `ConsoleSettings` | Turns off in player builds |
|---|---|
| `inPlayerLogsPanel` | Logs panel |
| `inPlayerOptionsPanel` | Options panel |
| `inPlayerHierarchyPanel` | Hierarchy panel |
| `inPlayerUtilitiesPanel` | Utilities panel |
| `inPlayerObjectInspector` | Object Inspector — including log object links, hierarchy component detail and type search |
| `inPlayerCommandLine` | Command Line |
| `inPlayerKeyBindings` | Key bindings (off by default) |
| `autoStartOverlay` | Auto-starting the runtime overlay |

All of them are always on in the Editor.

The example below disables the Hierarchy and Object Inspector panels for WebGL builds.
```csharp
public class NjConsoleSettingsBuildProcessor : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;
    public void OnPreprocessBuild(BuildReport report)
    {
        var settings = ConsoleSettings.Get();
        var platform = report.summary.platform;

        settings.inPlayerObjectInspector = platform != BuildTarget.WebGL;
        settings.inPlayerHierarchyPanel = platform != BuildTarget.WebGL;

        EditorUtility.SetDirty(settings);
        AssetDatabase.SaveAssets();
    }
}
```
> The settings live in a `Resources` asset, so mark it dirty and save before the build reads it. Revert it
> afterwards (or in `OnPostprocessBuild`) so the change doesn't leak into the next build.

> ⚠️ Note: In production builds, a determined attacker could still enable console features via memory hacking.   
> For best security, use compile define `NJCONSOLE_DISABLE` to strip out the console completely. See next section for more info.


# ✂️ Disable / Strip NjConsole for Production

### ✅ Benefits of Stripping
- 🧠 Smaller build, less memory.
- 🔐 Nothing to reverse engineer — no console UI, no cheat options.
- ✅ Recommended for production releases.

### 🧩 How It Works
- ✂️ NjConsole scripts are stripped behind `#if !NJCONSOLE_DISABLE`.
- 🧱 Most interfaces and key classes are **stubbed**, so your project still **compiles**.
- ❗ Some advanced APIs are stripped entirely and must be wrapped yourself.
  See [Hidden Gems](hiddengems.md#-what-survives-stripping) for what is stubbed and what isn't.
- 🔒 Wrap your own cheat/debug logic in `#if !NJCONSOLE_DISABLE` too, so it's stripped as well.
- 📋 **Log history is still collected** in the background — useful for crash reports and support diagnostics.

### ✂️ How to Disable NjConsole
- 🎛️️ **UI Method:** Go to `Project Settings > Ninjadini ⌨ Console > Disable NjConsole`
- ✍️ **Manual Define:** Add `NJCONSOLE_DISABLE` in _Player Settings > Scripting Define Symbols_.
- 🧑‍💻 **Code/API Method:** Call `ConsoleEditorSettings.AddDefineSymbolToDisableConsole()`

This disables NjConsole for release builds and re-enables it afterwards:
```csharp
using Ninjadini.Console.Editor;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class NjConsoleBuildDisableProcessor : IPreprocessBuildWithReport, IPostprocessBuildWithReport
{
    public int callbackOrder => -1000;

    public void OnPreprocessBuild(BuildReport report)
    {
        var isDevelopmentBuild = (report.summary.options & BuildOptions.Development) != 0;
        if (isDevelopmentBuild) return; 
        // ^ Have your own conditional logic here. In this example, we only disable NjConsole in release (non-debug) builds.
        
        ConsoleEditorSettings.AddDefineSymbolToDisableConsole();
    }

    public void OnPostprocessBuild(BuildReport report)
    {
        // Reenable NjConsole.
        // Unfortunately, if the build failed, this code will not execute and you'll need to manually turn it on from Project Settings > Ninjadini ⌨ Console > Disable NjConsole.
        // This code is not needed if you are using a build box where you revert the changes after build.
        ConsoleEditorSettings.RemoveDefineSymbolAndEnableConsole();
    }
}
```


[NjConsole doc home](index.md)