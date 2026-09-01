---
title: Changelog
nav_section: "Advanced"
nav_order: 11
nav_icon: "📋"
---

## 1.3.1
✨ Updated GettingStarted.pdf to reflect the latest features and improvements.
✨ `NjConsole.Overlay.Toggle()` to show or hide the overlay in one call.   
🐛 Fixed some of the new UI symbols not rendering on certain platforms.   
🐛 Fixed on-screen keyboard staying up after you leave a text field - search, command line and text prompts.   

## 1.3.0
🆕 Ready for Unity 6.4 through 6.7. ⚠️ Unity 6000.5.0 - 6000.5.8 render scroll views blank on Vulkan (Unity bug UUM-142280, fixed in 6000.5.9f1)   
🆕 Menu Shortcuts: press and hold any option or group to pin it into a shortcuts list inside the Options panel - now in the Editor window too, not just the runtime overlay.    
🆕 Logging paths are now three independent settings - Unity into NjConsole, NjConsole out to Unity, and whether `Debug.Log()` still reaches Unity's own console. Carefully reworked so no route logs twice or captures the same stack trace twice, and `Debug.Log(..., context)` now carries through.   
🆕 `ColoredLogChannel`: like `LogChannel` but with an optional color for its debug and info logs, applied to the whole line - and it carries through to Unity's own console too.   
✨ Log level filtering moved into a `Levels` panel, with the warning / error counts shown next to the button.   
✨ `Options.Repeating`: logging again replaces the previous line instead of spamming the log, and counts up a `123x` prefix on it. Useful for e.g. progress bars.    
✨ Select log rows and press Ctrl/Cmd+Shift+C to copy them including their stack traces (Ctrl/Cmd+C alone copies without).   
✨ Stack trace improvements: separate min-level settings for Release vs Development Builds.  
✨ FPS Monitor: click to flip between `fps` and `ms`; right-click to collapse / expand.   
✨ Viewing a group or panel that gets removed now shows it's gone instead of kicking you out, and restores when it's back.   
✨ Reworded labels, tooltips, help text and API docs throughout - fixed typos, and made the terminology consistent.   
⚠️ API Change: `NjLogger` now only holds the logging methods - handlers and stack trace settings moved to `NjLogger.Settings` (e.g. `NjLogger.Settings.AddHandler()`)   
⚠️ API Change: `logRow.GetString()` no longer takes a StringBuilder - custom `NjLogger.IHandler` implementations just drop the argument.  
⚠️ API Change: `LoggerUtils.TempStringBuilder` removed, use `LoggerUtils.BorrowStringBuilder()` / `LoggerUtils.ReturnStringBuilder()` if you need a scratch builder of your own.   
🐛 Various logs history and number formatting fixes.

## 1.2.2
✨ Lots of UX improvements:   
&nbsp; ▸ Clicking an error log at the top now opens in full details view   
&nbsp; ▸ Select multiple log rows and Ctrl+C to copy   
&nbsp; ▸ Options: Search and History results now also link to the belonging directory   
&nbsp; ▸ Button to perform Resources.UnloadUnusedAssets() in Utilities menu   
&nbsp; ▸ Memory Monitor: Click to GC, double click to UnloadUnusedAssets, triple click to minimize / expand   
&nbsp; ▸ FPS Monitor: Click to minimize / expand   
&nbsp; ▸ Improved visibility of FPS and Memory Monitor texts   
&nbsp; ▸ and much more!   
✨ Custom UIToolkit PanelSettings and StyleSheet support via project settings   
✨ Tested to support Editor Play Mode `Do not reload Domain or Scene`   
✨ Tested to support High `Managed Stripping Level`.   
⚠️ API Change: `ConsoleEditorBridge.CustomStackTraceFrameSkip` moved to `ConsoleContext.CustomStackTraceFrameSkip` for availability in both Editor and Runtime.   
🐛 Improved scaling issues in Editor play mode on high-DPI screens.   
🐛 Fix not being able to select dropdown menu items in Shortcut Options.   
🐛 Lots of minor bug fixes.

## 1.2.1
✨ Custom log colors: Pass a color as the first parameter of a log to easily color the log text.   
✨ Right-click to pin: Right-click any log to pin it. Pinned logs remain visible regardless of filters (search, channel, or priority).   
✨ Custom stack trace skipping: Add your own frame-skipping logic via `ConsoleEditorBridge.CustomStackTraceFrameSkip`.   
🐛 Improved to stick to bottom of log scroll better when resizing etc.   
🐛 Fixed compile errors not always showing in NjConsole.   
🐛 Fixed inspector input issue where number inputs were blocked by the command line UI.

## 1.2.0
🆕 Command Line Support: Using the same paths as Option Menus. With commands history, autocompletion, multi-params support, variables & scopes. Please see online doc for details.   
✨ Log details text view now detects file paths and adds buttons to locate the file.   
✨ Static members can now be added to Option Menus by passing the type object, e.g. `CreateCatalogFrom(typeof(MyConsoleMenus))`.   
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