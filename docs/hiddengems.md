---
title: Hidden Gems
nav_section: "Advanced"
nav_order: 9
nav_icon: "💎"
---

# 💎 Hidden Gems

Public API that NjConsole uses internally and you're welcome to use too, but that doesn't get its own page
elsewhere. Handy for building your own debug tools on top of the console.

These are `public` and won't be removed casually, but expect less hand-holding than the guided flow.

## 📦 What survives stripping

[Stripping the console](buildcustomization.md#️-disable--strip-njconsole-for-production) compiles most of it out
of the build. A few entry points keep a do-nothing stub so your code still compiles; everything else disappears
and must be wrapped:

```csharp
#if !NJCONSOLE_DISABLE
    ConsoleTextPrompt.Show(data);
#endif
```

| ✅ Safe to call unguarded | ❗ Must be wrapped in `#if !NJCONSOLE_DISABLE` |
|---|---|
| `NjConsole.*` — Options, KeyBindings, Overlay, Settings, Modules | `ConsoleTextPrompt` |
| `ConsoleToasts` | `ConsoleInspector` |
| Everything in `Ninjadini.Logger` — `NjLogger`, `LogChannel`, `LogsHistory`, `StrValue` | `ConsoleGraphingElement`, `FpsMonitorElement`, `MemoryMonitorElement` |
| `ColoredLogChannel` | `ConsoleUtilitiesElement` — the log export helpers |
| `ConsoleModules`, `ConsoleSettings` | `ConsoleWindow`, `ConsoleLogsPanel`, `ConsoleScreenShortcuts` |
| `ConsoleContext.SetData()` / `GetData()` / `Storage` | `ConsoleContext.TryGetFocusedContext()` / `RuntimeOverlay` |
| `ConsoleKeyBindings`, including `IsUserTyping()` | `ConsoleScreenShortcuts`, including its `IRestorable` |
| Every `IConsole*` interface you implement | `StringParser`, `ConsoleUIUtils` |

> 💡 Rule of thumb: reached through the `NjConsole` static, it's stubbed; named as a UI class directly, it isn't.
> This bites on **naming** the type at all — implementing a stripped interface or using it as a generic argument
> breaks the build just like calling into it, so the guard goes around the whole declaration.

## 🍞 Toasts

A transient message at the top of whichever console window has focus — good for confirming a debug action ran.
Stubbed, so no guard needed.

```csharp
using Ninjadini.Console;

ConsoleToasts.TryShow("Save wiped");

// ... with a call-to-action button
ConsoleToasts.TryShow("Save wiped", () => NjConsole.Overlay.Hide(), "Close");
```

`TryShow()` falls back to the log if no console window exists; `Show(context, ...)` targets a specific window.
Both return the toast element — `RemoveFromHierarchy()` to dismiss early.

## ⌨️ Text prompts

The modal input dialog behind the Options panel's text and number items ([Options Menu](optionsmenu.md)).
Call it directly when you need input somewhere else.

> ❗ Needs `#if !NJCONSOLE_DISABLE` — this class has no stub.

```csharp
#if !NJCONSOLE_DISABLE
ConsoleTextPrompt.Show(new ConsoleTextPrompt.Data()
{
    Title = "Server URL",
    InitialText = MyGame.ServerUrl,
    ResultCallback = (txt) =>
    {
        if (txt == null) return true;                   // close button - accept and dismiss.
        if (!txt.StartsWith("https://")) return false;  // reject, prompt stays open and flashes red.
        MyGame.ServerUrl = txt;
        return true;
    }
});
#endif
```

| Method | Where it draws |
|---|---|
| `Show(Data)` | Own UIDocument above the game. **Play mode only.** Returns it, destroyed for you on success. |
| `ShowInConsoleRoot(visualSrc, Data)` | Inside the console window `visualSrc` belongs to. **Works in editor windows.** |
| `Show(parent, Data)` | A child of `parent`, to place it yourself. |

`Data` also carries auto-complete (`AutoCompleteResultsCallback`), password masking, multiline and mobile
keyboard type. `Data.CreateForNumberInput(setter)` preconfigures numeric input for `float` / `double` /
`int` / `uint` / `long` / `ulong`.

## 🔍 Object inspector from your own code

[Inspector](inspector.md) covers the panel. The programmatic hooks:

```csharp
// Draw your own type however you like - applies everywhere the inspector shows it.
ConsoleInspector.RegisterCustomDrawer(typeof(MySaveData), (obj, foldOut) =>
{
    var data = (MySaveData)obj;
    foldOut.Add(ConsoleInspector.CreateInfo("coins", typeof(int),
        () => data.Coins, v => data.Coins = (int)v));
});

// Open the inspector on any object, from inside a console panel.
ConsoleInspector.Show(someContainerElement, myObject);
```

`ConsoleInspector.AutoReadProperties` and `CustomFieldCreator` are the other two static hooks.

## 📌 Pin your own widget to the screen shortcuts

The corner shortcuts — where the FPS and memory graphs live — accept any `VisualElement`. Implement
`ConsoleScreenShortcuts.IRestorable` so the console can rebuild your widget after a restart, then add it.

> ❗ `IRestorable` is nested inside the stripped `ConsoleScreenShortcuts`, so the **whole class** needs the
> guard — a guard around just the method body won't compile, because the base list still names the interface.

```csharp
#if !NJCONSOLE_DISABLE
class MyWidget : IConsoleModule, ConsoleScreenShortcuts.IRestorable
{
    string ConsoleScreenShortcuts.IRestorable.FeatureName => "myWidget";

    VisualElement ConsoleScreenShortcuts.IRestorable.TryRestore(ConsoleContext context, string path)
        => path == "hp" ? new Label("HP") : null;

    public void PinIt(ConsoleContext context)
        => context.RuntimeOverlay?.Shortcuts?.AddNewItem(this, "hp");
}
#endif
```

> ⚠️ `FeatureName` and the path are persisted — renaming them orphans the user's pinned shortcuts.

## 📈 Your own graphs

The element behind the built-in FPS and memory monitors. Pair it with the shortcuts above to pin a live graph
on screen.

```csharp
var graph = new ConsoleGraphingElement(width: 120, pixesPerValue: 2f, intervalMs: 100);
graph.Add(new IConsoleGraphDataProvider.Simple("enemies", Color.red, () => MyGame.EnemyCount));
graph.ValueSuffix = " units";
```

Subclass to override `Update()`, `AddValueLabel()` or `UpdateLabelValue()`. `ForcedMinBound` / `ForcedMaxBound`
pin the scale when auto-ranging isn't what you want.

## 📜 Reading the log history yourself

`NjLogger.LogsHistory` is the ring buffer of everything NjLogger captured. Always available — nothing in
`Ninjadini.Logger` is stripped by `NJCONSOLE_DISABLE`, which is what makes it usable for crash reports.
See [Logging](logging.md).

```csharp
using Ninjadini.Logger;
// The LogsHistory type itself lives in Ninjadini.Logger.Internal - only needed if you name it.

NjLogger.LogsHistory.ForEachLogNewestToOldest(line => Report(line.GetLineString()), maxLogs: 200);

var sb = new StringBuilder();
NjLogger.LogsHistory.GenerateHistoryNewestToOldest(sb, maxLogs: 500);
```

- `GetLevelCount(level)` — a cheap running count, good for an on-screen error badge.
- `StartBackLoggingLock()` / `StopBackLoggingLock()` — stop old lines recycling while the user scrolls back.

## 🐞 Your own bug-report button

The Utilities panel's export buttons are public statics, so you can wire the same behaviour to your own UI.
For the format, implement `IConsoleLogExportFormatter`
([Logging](logging.md#-customize-the-exported--emailed-logs)).

```csharp
#if !NJCONSOLE_DISABLE
var context = ConsoleContext.TryGetFocusedContext();
if (context != null)
{
    var text = ConsoleUtilitiesElement.GenerateTextLog(context); // header + logs + footer
    ConsoleUtilitiesElement.EmailTextLog(context);               // opens a mail draft
    ConsoleUtilitiesElement.ExportHtmlLog(context);              // shareable HTML file
}
#endif
```

## 🧰 Adding a page to the Utilities panel

Your own screens next to Tools / App Info / Device Info. Safe unguarded.

```csharp
NjConsole.Settings.CustomUtilitiesMenus += (context, addMenu) =>
{
    addMenu("My Tools", () => new Label("Hello from my tools"));
};
```

Use `+=`, not `=`, so you don't wipe out other systems' menus. It runs once per console window, so the editor
window and runtime overlay each get their own. For a full panel instead of a sub-page, see
[Custom Panels](custompanels.md).

## 🎛️ Driving the overlay

These live on the stubbed `NjConsole` static, so they're safe unguarded. See [Console Window](consolewindow.md).

```csharp
NjConsole.Overlay.Toggle();                       // show if hidden, hide if showing
NjConsole.Overlay.SetActivePanel("Utilities");    // by side-bar name - always safe
NjConsole.Overlay.Instance.Scale = 1.5f;          // user-facing UI scale, persisted
NjConsole.Overlay.Instance.HideErrorDisplayedAtTop();
NjConsole.Overlay.Destroy();                      // tear the runtime console down

#if !NJCONSOLE_DISABLE
// The generic overload names a panel type, and the built-in panel classes are stripped.
NjConsole.Overlay.SetActivePanel<ConsoleLogsPanel.Module>();
#endif
```

## 🧩 Odds and ends

- **`ConsoleContext.SetData(obj)` / `GetData<T>()`** — per-window state keyed by type, for as long as the window
  lives. `context.Storage` is the persisted version: PlayerPrefs at runtime, serialised into the editor window.
- **`ConsoleKeyBindings.IsUserTyping()`** — true while a text field has focus. Check it before acting on raw key
  input, so your shortcuts don't fire while someone types in the command line.
- **`NjConsole.Settings.CustomStackTraceFrameSkip`** — hide your logging wrapper's frames so double-clicking a
  log lands on the real call site.
- **`NjConsole.Settings.PlayerPrefKeysProvider`** — feed your known keys into Utilities > Player Prefs.
- **`ConsoleModules.ModuleAdd` / `ModuleRemoved`** — react to modules coming and going. Unsubscribe:
  `ConsoleModules` is shared across windows and play sessions, so it leaks fast.
- **`StringParser`** — parsing helpers behind the [Command Line](commandline.md).
  `GetRemainingPartialMatch()` is the one for `IConsoleCommandlineModule.FillAutoCompletableHints()`. Needs a guard.
- **`ConsoleObjReferenceStorage`** — the command line's named object store (`_`, `@`, `$name`). Write to it from
  your own command line module so users can pass objects between commands. Safe unguarded.

[NjConsole doc home](index.md)
