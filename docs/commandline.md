# 💻️ Command Line

NjConsole’s Command Line lets you run user-defined and built-in commands through text input.

Commands are registered the same way as the **Options Menu** (via `[ConsoleOption]` or programmatically).  
See [Options Menu](optionsmenu.md) for full details.  
By default, all Options Menu entries also appear as commands. To separate the two, see **Managing Command Catalogs**.

---

## ▶️ Basic Usage

- **Show Command Line**: Press any key while focused on the Logs panel.
- **Autocomplete**: Suggestions appear as you type.
    - `Tab` accepts the first suggestion.
    - `Shift+↑` / `Shift+↓` to navigate suggestions, `Tab` or `Enter` to accept.
- **History**: `↑` / `↓` cycles through previous commands.
- **Hide**: `Esc` closes the Command Line.

### 📱 On Mobile (no physical keyboard)

- Tap the **Logs** button again to show the Command Line.
- The input field uses a text-prompt style with autocomplete.
- Tap the `⌨` button to toggle between prompt and normal input.

---

## 🔤 Command Structure & Syntax

```
<command> <parameters seperated by space ` ` or comma `,`>
```

**Built-in commands** appear under `/`.   
Example: `/help` lists all commands.   
**String** params can be wrapped in quotes (`"`) to include spaces or commas. Escape `"` with `\"`. 

**Notes:**
- Command names are case-insensitive. Conflicts in casing will show in autocomplete suggestions but may not execute correctly.
- Names can contain spaces; `/` creates grouped folders, like in the Options Menu.
- Method overloads are **not** supported. Each command must be unique — add a suffix to avoid collisions.

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

If a parameter is an object that requires constructor arguments, group them in parentheses:  
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

## 📦 Storage commands

`/store <name>`: store the last returned object.   
`/retrieve <name>`: retrieve a stored object.   
`/list stored`: list stored variables.   

**Example:**
```
[ConsoleOption("profiles/loadprofile")]
void LoadProfile(PlayerProfile value) { ... }

Command: /store profile
Command: profiles/loadprofile $profile
Output: `> PlayerProfile; Scope $@ set.`
```

## 🔍 Scopes
When a command returns a **class object**, the Command Line automatically switches scope to that object.
This is shown in the output as: `Scope $@ set`.

**Scope Variables**   
- `$@` — current scope object.
- `$@prev` — previous scope object.
- `$_` — last returned object (not necessarily scoped).

### /call
Some built-in commands (like `/call`) act directly on the current scope object, letting you invoke any field, property, or method via reflection:
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
To register commands for Command Line only (not Options Menu):
```csharp
NjConsole.CommandLineOptions.CreateCatalog...(...)
```
This API is the same as the Options Menu catalog.

- Hide all commands from Options Menu: ```NjConsole.Options.CommandLinePath = null;```
- Put all Options Menu commands into a subfolder: ```NjConsole.Options.CommandLinePath = "<folder>"```

## 🗝️ Accessing stored variable from code
You can access stored variables programmatically the same way built-in commands do:
```csharp
var storage = NjConsole.Modules.GetOrCreateModule<ConsoleObjReferenceStorage>();
var lastResult = storage.GetLastResult();   // same as `storage.GetStored("_")`
var scope = storage.GetScope();     // same as `storage.GetStored("@")`
var customVar = storage.GetStored("profile");   // if you called `/store profile`
```

> Returned objects, scopes and other stored $ variables will be strong referenced and will not be garbage collected.
> Call `/clear stored` to clear everything.

## 🖇️ Custom Command Line Handling (Input Prompt Takeover)
If a command returns an IConsoleCommandlineModule, the Command Line switches into a locked mode, where all subsequent input is routed only to that module until it releases control.
This is useful for creating interactive prompts, multi-step wizards, or temporary modes.
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
See `GuessNumberCommandLineGame` in DemoNjConsole.cs or call `guessTheNumber` in demo scene for a full example.

## 🔌 Adding a Custom Executor

Similar to custom input prompt handling, you can also create an extension and add it in project settings.
1. Create a class that implements IConsoleCommandlineModule and fill in the required implementation.
2. (Optional) Implement `PopulateHelpSuggestions(List<string> helpLines)` to display help text.
3. Implement IConsoleExtension.
4. Add [Serializable] attribute.
5. Go to `Project Settings > NjConsole > Extension Modules > Add Extension Module` > add your new class
6. Press `Apply changes`

```
[Serializable]
public class DemoFoodPromptHandler : IConsoleCommandlineModule, IConsoleExtension {
    ...
    ...
    public void PopulateHelpSuggestions(List<string> helpLines) {
        helpLines.Add("<Type any food you like>");
    }

    public bool PersistInEditMode => true; // OPTIONAL: If you need your custom commands to be runnable outside play-mode.

    void OnAdded(ConsoleModules modules) {
        // OPTIONAL: How to disable Options Menu's command line.
        var optionsCmdModule = NjConsole.Modules.GetModule<OptionCommandsModule>(false);
        if (optionsCmdModule != null)
        {
            optionsCmdModule.Enabled = false;
        }
    }
}
```
Your custom executor is evaluated before the default Command Line, except that `/` commands always take priority.
If `TryRun()` returns `false`, NjConsole passes the input to the next enabled Command Line Module.

[NjConsole doc home](index.md)