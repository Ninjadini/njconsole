
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
> An upcoming update will introduce support for `#ifdef` flags, allowing you to fully disable the console at compile time, ensuring they are completely stripped from the build.

[NjConsole doc home](index.md)