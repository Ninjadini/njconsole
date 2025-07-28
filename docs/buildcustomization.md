
# 💻 Build customization

## 🔧 Dynamically Configure Console Features During Build

The example below disables the Hierarchy and Object Inspector panels for WebGL builds.
```
public class NjConsoleSettingsBuildProcessor : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;
    public void OnPreprocessBuild(BuildReport report)
    {
        var settings = ConsoleSettings.Get();
        var platform = report.summary.platform;

        settings.inPlayerObjectInspector = platform != BuildTarget.WebGL;
        settings.inPlayerHierarchyPanel = platform != BuildTarget.WebGL;
    }
}
```
> ⚠️ Note: In production builds, a determined attacker could still enable console features via memory hacking.   
> For best security, use compile define `NJCONSOLE_DISABLE` to strip out the console completely. See next section for more info.


# ✂️ Disable / Strip NjConsole for Production

### ✅ Benefits of Stripping
- 🧠 Reduces memory usage and final build size.
- 🔐 Prevents malicious users from reverse engineering to access NjConsole UI, cheat options, etc.
- ✅ Highly recommended for production releases.

### 🧩 How It Works
- ✂️ NjConsole scripts are conditionally stripped using `#if !NJCONSOLE_DISABLE`.
- 🧱 Most interfaces and key classes are safely **stubbed**, so your project continues to **compile without errors**.
- ❗ Certain advanced APIs are completely stripped and must be wrapped manually using `#if !NJCONSOLE_DISABLE`.
- 🔒 You should also wrap your own cheat/debug logic with `#if !NJCONSOLE_DISABLE` to ensure it’s fully stripped from production builds.
- 📋 **Log history is still collected** in the background — useful for crash reports or customer service diagnostics.

### ✂️ How to Disable NjConsole
- 🎛️️ **UI Method:** Go to Project Settings > NjConsole > Disable NjConsole
- ✍️ **Manual Define:** Add `NJCONSOLE_DISABLE` in _Player Settings > Scripting Define Symbols_.
- 🧑‍💻 **Code/API Method:** Call `ConsoleEditorSettings.AddDefineSymbolToDisableConsole()`

Example below disables NjConsole in release builds, and attempts to reenable it after the build.
```
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
        // Unfortunately, if the build failed, this code will not execute and you'll need to manually turn it on from Project Settings > NjConsole > Disable.
        // This code is not needed if you are using a build box where you revert the changes after build.
        ConsoleEditorSettings.RemoveDefineSymbolAndEnableConsole();
    }
}
```


[NjConsole doc home](index.md)