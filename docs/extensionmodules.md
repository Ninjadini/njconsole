# 🧱 Extension Modules / Addons

Extensions provide a simple way to build plug-and-play modules for NjConsole, allowing your features to be easily integrated into different projects or shared within the community.

Thanks to the modular design, you can pick and choose only the features or extensions that are relevant for your project, keeping your console lightweight and focused.

## 📦 External Extensions Repo

You can find some extensions here:  
[https://github.com/Ninjadini/njconsole/tree/main/extension-modules](https://github.com/Ninjadini/njconsole/tree/main/extension-modules)  
Each extension will usually include a README or comments explaining how to include it in your project.
> Note: This is still early days, so there may not be much there yet.


## ✨ Create Your Own Extension

To add your own extension:
1. Implement the IConsoleExtension interface.
2. Add the [Serializable] attribute to your class.
3. Go to  `Project Settings` > `NjConsole` > `Extension Modules`, add your new class.

Here’s a simple example that registers an option menu item:

```
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