---
title: Command Line
nav_section: "Core"
nav_order: 4
nav_icon: "⌨️"
---

# 💻️ Command Line

Run your own and built-in commands as text. Commands register exactly like the **Options Menu** —
via `[ConsoleOption]` or programmatically, see [Options Menu](optionsmenu.md).
Every Options Menu entry doubles as a command by default; **Separating Command Catalogs** below splits them.

---

## ▶️ Basic Usage

- **Show**: press any key while focused on the Logs panel, or `Shift` + <code>`</code> in the runtime overlay.
- **Autocomplete**: `Tab` accepts the first suggestion; `Shift+↑` / `Shift+↓` navigates, `Tab` or `Enter` accepts.
- **History**: `↑` / `↓`.
- **Hide**: `Esc`.

### 📱 On Mobile (no physical keyboard)
Tap the **Logs** button again to show it. The field uses a text-prompt style with autocomplete — tap `⌨` to
switch between prompt and normal input.

---

## 🔤 Command Structure & Syntax

```
<command> <parameters separated by space ` ` or comma `,`>
```

**Built-in commands** live under `/` — `/help` lists everything.
**String** params take quotes (`"`) for spaces or commas; escape with `\"`.

### 🧾 Built-in `/` commands

| Command | What it does |
|---|---|
| `/help` | List every available command, with argument hints and tooltips |
| `/clear logs` | Clear the log history |
| `/filter cmds` | Filter the Logs panel down to Command Line output only |
| `/filter reset` | Clear all log filters |
| `/store <name>` | Store the last result `$_` under a name |
| `/retrieve <name>` | Retrieve a stored object (becomes the new `$_`) |
| `/list stored` | List all stored objects |
| `/clear stored` | Clear all stored objects |
| `/scope [name]` | Set scope `$@` to `$_`, or to a stored object if you pass a name |
| `/rescope` | Go back to the previous scope `$@prev` |
| `/call <member> [args]` | Call a field, property or method on the current scope |
| `/inspect [name]` | Open `$_` (or a stored object) in the Object Inspector |
| `/destroy [name]` | Destroy `$_` (or a stored object) |
| `/find type` | Search loaded assemblies with the type search prompt |
| `/close` | Close the console overlay (runtime only) |

`/` commands always take priority over your own commands.

- Names are case-insensitive. Casing conflicts show in autocomplete but may not execute correctly.
- Names can contain spaces; `/` creates folders, as in the Options Menu.
- Overloads are **not** supported — each command must be unique, so add a suffix to avoid collisions.

---

## 💡 Examples

```csharp
[ConsoleOption] 
void SayHello() {
    Debug.Log("Hello");
}
```
Command: `sayhello`

```csharp
[ConsoleOption("profile/name")]
[Tooltip("Get or set user profile name")] // shows in autocomplete and /help
public string Name;
```
Get Command: `profile/name`   
Set Command: `profile/name "My name here"`

```csharp
[ConsoleOption("demo/introduce")]
void IntroducePerson(string name, int age) { ... }
```
Command: `demo/introduce Ninjadini 30`   
or `demo/introduce "Ninjadini", 30`

---

## 🏗️ Advanced Parameters - Constructor Arguments

Group constructor arguments in parentheses:  
```csharp
[ConsoleOption("math / vector multiply")]
static Vector3 MultiplyV(Vector3 a, float b) => a * b;
```
Command: `math/vector multiply (1 2 3) 1`   
or `math/vector multiply (1,2,3),1`   

**Nested constructors:**
```csharp
[ConsoleOption]
void NestedConstructor(PositionAndSize a, string message) => Debug.Log($"Position: {a.Pos}, Size: {a.Size}, Message: {message}");
        
struct PositionAndSize {
    public Vector3 Pos;
    public float Size;
    public PositionAndSize(Vector3 pos, float size) {
        Pos = pos;
        Size = size;
    }
}
```
Command: `nestedConstructor ((1, 2, 3), 3), "hello there"`

`/help` ends with a general reminder of this syntax, and any argument-parsing error repeats it. For a
per-command example, add a `[Tooltip]` — the autocomplete hint is generated from the signature
(`<Vector3 a> <Single b>`), which says nothing about the parentheses:

```csharp
[ConsoleOption("math / vector multiply")]
[Tooltip("Example param: (1 2 3) 1  OR  (1,2,3),1")]
static Vector3 MultiplyV(Vector3 a, float b) => a * b;
```
The tooltip shows next to the command in **autocomplete** and in **`/help`**.

---

## 📤 Return Values & Storage

```csharp
[ConsoleOption("profiles/GetUserId")]
string GetUserId() { ... }
```
Command: `profiles/getuserid`   
Output: `> ‘UserId1234’` < the returned string from GetUserId method.   

Returned objects are stored in `$_` and can be reused:
```csharp
[ConsoleOption("profiles/GetProfile")]
PlayerProfile GetProfile(string id) { ... }
```
Command: `profiles/getprofile $_`   
Output: `> PlayerProfile; Scope $@ set.`

> ℹ️ If a command returns a null object, the returned object variable `$_` will not change.

## 📦 Storing objects for later

Name the last result so you can feed it back into another command later:
```csharp
[ConsoleOption("profiles/getprofile")]
PlayerProfile GetProfile(string id) { ... }

[ConsoleOption("profiles/loadprofile")]
void LoadProfile(PlayerProfile value) { ... }
```
```
> profiles/getprofile user1234     // returns a PlayerProfile, now in $_
> /store profile                   // keep it as $profile
> profiles/loadprofile $profile    // pass it to another command
```
`/list stored` shows everything you've named, `/clear stored` empties it.

## 🔍 Scopes
Returning a **class object** switches scope to it automatically — the output says `Scope $@ set`.

**Scope Variables**   
- `$@` — current scope object.
- `$@prev` — previous scope object.
- `$_` — last returned object (not necessarily scoped).

### /call
`/call` acts on the current scope object, invoking any field, property or method by reflection:
- Read field profile.Name: `/call Name`   
- Set field profile.Name: `/call Name "New name here"`   
- Call method profile.SetAge(int age): `/call SetAge 30`   
> `/call` uses reflection. Member names are case-sensitive.

`/rescope`: return to the previous scope (`$@prev`).
`/scope <stored name>`: switch to a stored variable’s scope.

## 🔗 Accessing Logged Objects
If you log an object like this:
```csharp
NjLogger.Info("Player Profile link: ", playerProfile.AsLogRef());
```
- The log details view will show a button to inspect it.
- Click `⌨` to send that object to the Command Line (e.g., `/store profile`).
- You can also run `/inspect` to open the last returned object in the Inspector.

## 📂 Accessing Hierarchy Objects
From NjConsole’s hierarchy panel:
- Click on any object to show the Console Inspector.
- Click `⌨` to pass it to the Command Line.

## 🗂 Separating Command Catalogs
To register Command Line-only commands:
```csharp
NjConsole.CommandLineOptions.CreateCatalog...(...)
```
This API is the same as the Options Menu catalog.

- Hide all commands from Options Menu: ```NjConsole.Options.CommandLinePath = null;```
- Put all Options Menu commands into a subfolder: ```NjConsole.Options.CommandLinePath = "<folder>"```

## 🗝️ Accessing stored variable from code
Reach stored variables the same way the built-in commands do:
```csharp
var storage = NjConsole.Modules.GetOrCreateModule<ConsoleObjReferenceStorage>();
var lastResult = storage.GetLastResult();   // same as `storage.GetStored("_")`
var scope = storage.GetScope();     // same as `storage.GetStored("@")`
var customVar = storage.GetStored("profile");   // if you called `/store profile`
```

> Stored `$` variables, returned objects and scopes are strong references and won't be collected.
> `/clear stored` releases them.

## 🖇️ Custom Command Line Handling (Input Prompt Takeover)
Return an `IConsoleCommandlineModule` and the Command Line locks, routing the next input to that module —
useful for interactive prompts, multi-step wizards, or temporary modes.
```
[ConsoleOption("food prompt")]
IConsoleCommandlineModule FoodPrompt() => new DemoFoodPromptHandler();

class DemoFoodPromptHandler : IConsoleCommandlineModule {
    public bool TryRun(IConsoleCommandlineModule.Context ctx){
        if (ctx.Input == "exit") {
            ctx.Output.Info("Good bye!");
        } else {
            ctx.Output.Info($"Your 'name of food': \"{ctx.Input}\"");
            ctx.Result = this;  // Keep input locked to this module.
        }
        return true;  // Indicates the input was handled.
    }
    public void FillAutoCompletableHints(IConsoleCommandlineModule.HintContext ctx) {
        // Suggest 'exit'
        var remaining = StringParser.GetRemainingPartialMatch(ctx.Input, "exit");
        if (remaining != null) {
            ctx.Add(remaining, "<alpha=#44> Exit the demo prompt");
            return;
        } else { 
            // Suggest generic text without tracking whats already typed (-ctx.Input.Length)
            ctx.Add("", "A name of food", -ctx.Input.Length);
        }
    }
}
```
See `GuessNumberCommandLineGame` in DemoNjConsole.cs, or run `guessthenumber` in the demo scene for a full example.

## 🔌 Adding a Custom Executor

Same interface, registered as an extension instead:
1. Create a class that implements `IConsoleCommandlineModule`.
2. (Optional) Implement `PopulateHelpSuggestions(List<string> helpLines)` to display help text.
3. Implement IConsoleExtension.
4. Add [Serializable] attribute.
5. Go to `Project Settings > Ninjadini ⌨ Console > Extension Modules > Add Extension Module` > add your new class
6. Press `Apply extension changes`

```
[Serializable]
public class DemoFoodPromptHandler : IConsoleCommandlineModule, IConsoleExtension {
    ...
    ...
    public void PopulateHelpSuggestions(List<string> helpLines) {
        helpLines.Add("<Type any food you like>");
    }

    public bool PersistInEditMode => true; // OPTIONAL: If you need your custom commands to be runnable outside play-mode.

    void IConsoleModule.OnAdded(ConsoleModules modules) {
        // OPTIONAL: How to disable Options Menu's command line.
        var optionsCmdModule = NjConsole.Modules.GetModule<OptionCommandsModule>(false);
        if (optionsCmdModule != null)
        {
            optionsCmdModule.Enabled = false;
        }
    }
}
```
Custom executors run before the default Command Line, though `/` commands always win.
Return `false` from `TryRun()` and the input passes to the next enabled module.

[NjConsole doc home](index.md)