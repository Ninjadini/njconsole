
# 📝 NjLogger & Logs panel

## 📘 NjLogger basics
NjLogger is a high-performance alternative to Debug.Log, designed to minimize overhead and integrate tightly with NjConsole.
Compared to Debug.Log, which allocates memory and generates costly stack traces, NjLogger offers:
- Avoids GC pressure with zero-allocation argument formatting
- Seamless integration with NjConsole: supports filtering, channels, and object inspection
- Automatic capture of logs from Debug.Log() — they’ll still appear in NjConsole without extra setup

```
// These logs will appear in NjConsole with appropriate severity styling
NjLogger.Debug("This is a debug level text - they get auto excluded in release builds");
NjLogger.Info("This is an info level text");
NjLogger.Warn("This is a warning level text");
NjLogger.Error("This is an error level text - an alert shows when an error is logged.");
        
// Mixing types? No problem. This won't allocate.
NjLogger.Info("Mix argument types without allocation... integer:",123," float:", 123.45f," bool:", true);

// Link to objects for runtime inspection
var playerObj = GetTestPlayerObj();
NjLogger.Info("Here is a log with a link to ", playerObj.AsLogRef(), " - you can inspect it");

// Log object as plain string (no link)
NjLogger.Info("If you don't want a link, this is how... ", playerObj.AsString());

// Use named channels to group logs (recommended to keep as static readonly)
static readonly LogChannel channel = new LogChannel("myChannel");

channel.Info("A log in `myChannel`");
channel.Warn("A warning in `myChannel`");

// Unity's native logs still show up in NjConsole
Debug.Log("Logs from Unity’s Debug.Log() automatically appear in NjConsole");
```

<img src="images/logs-window.png" alt="Screenshot of logs panel" width="450" >

## 🔍 Log filtering

<img src="images/log-filters.png" alt="Screenshot of log filtering" width="450" >

NjConsole provides powerful filtering options to help you focus on the logs that matter:

- 🔤 **Text Search** Use multiple conditions to refine results.
  - `And` All conditions must match
  - `Or` At least one Or condition must match
  - `Not` Must not match to pass the filter
- 🧵 **Channels**
  - [ * ] Show all logs (no channel filtering)
  - [ - ] Show logs that have no channel assigned
- 🚦 **Log Levels**
  - Filter logs by severity: Info, Warn, Error


## 🔗 Logs object linking
You can include object references directly in your logs.  
When clicked, they open in the Object Inspector for quick inspection and editing.
```
var playerObj = GetTestPlayerObj();
NjLogger.Info("Here is a log with a link to ", playerObj);
NjLogger.Info("Here is a link to ", playerObj.AsLogRef(), " - mixed in multiple arguments");
```

When you click the log entry in the console, a button will appear for any linked object:  
<img src="images/logs-object-link.png" alt="Screenshot of object link" width="450" >   
Clicking the button opens the object in the inspector:  
<img src="images/logs-inspector.png" alt="Screenshot of object link" width="450" >  

> **♻️ Memory-Safe by Design**  
> Object links are held via weak references, so they won't cause memory leaks. However, if the object is garbage collected, the link may expire.  
> To retain the object for the log's lifetime (as long as it stays in the ring buffer), use a strong reference:  
> `NjLogger.Info("A strong object link:", aTestObj.AsStrongLogRef());`
  
> **⚠️ Limitations**  
> While you can view and modify many fields and properties, not all data types are fully editable (yet).


# 🔧 Advanced topics

## 🔁 Sending Logs _To_ NjLogger from Your Own Logger

- If your logger already forwards logs to Debug.Log(), **no extra setup is needed.**
- Otherwise, the most efficient way is to call NjLogger.Add() directly from your logger:
```
NjLogger.Add(<message>, options: NjLogger.Options.Info /* or map your log level here */);
```

## 🔀 Sending Logs _From_ NjLogger to Your Own Logger

To forward logs from `NjLogger` into your own logging system, implement and register a custom `NjLogger.IHandler`.
```
// Register once during initialization
NjLogger.AddHandler(new MyLoggerBridge());

public class MyLoggerBridge : NjLogger.IHandler {
  public void HandleLog(ref NjLogger.LogRow logRow){
    var message = logRow.GetString(LoggerUtils.TempStringBuilder);
    var level = logRow.Level;
    // Forward to your logger
  }
  public void HandleException(Exception exception, ref LogRow logRow){
    var message = logRow.GetString(LoggerUtils.TempStringBuilder);
    var level = logRow.Level;
    // Forward exception to your logger
  }
}
```
> ⚠️ Only register your handler once. Multiple registrations will result in duplicate logs.


## 🧵 Extracting Log Strings from NjLogger

`NjLogger` stores logs in a rotating ring buffer, with the size controlled by `MaxHistoryCount` (set in Project Settings for Editor/Player).  

To export logs (from newest to oldest) as a single string:
```
var stringBuilder = new StringBuilder();
NjLogger.LogsHistory.GenerateHistoryNewestToOldest(stringBuilder);
var logMessages = stringBuilder.ToString();
```

For finer control, you can iterate manually:
```
NjLogger.LogsHistory.ForEachLogNewestToOldest((log) =>
{
    var message = log.GetLineString();
    var level = log.Level;
    var time = log.Time;
    var channelName = log.GetChannelName();
    var channelTag = !string.IsNullOrEmpty(channelName) ? $" [{channelName}]" : "";
    var formattedLogString = $"[{time:HH:mm:ss}] [{level}]{channelTag} {message}";

    //Debug.Log(formattedLogString);
});
```
If you need the other direction, use `ForEachLogOldestToNewest`



## ⏱ Customize timestamp format in logs panel
1. Create a class that implements both `IConsoleTimestampFormatter` and `IConsoleExtension`.
2. Mark the class with the `[Serializable]` attribute.
```
[Serializable]
public class MyCustomTimestampFormatter : IConsoleTimestampFormatter, IConsoleExtension
{
    public void AppendFormatted(LogLine log, StringBuilder stringBuilder)
    {
        var time = log.Time;
        LoggerUtils.AppendNumWithZeroPadding(stringBuilder, time.Hour, 2);
        stringBuilder.Append(":");
        LoggerUtils.AppendNumWithZeroPadding(stringBuilder, time.Minute, 2);
        stringBuilder.Append(":");
        LoggerUtils.AppendNumWithZeroPadding(stringBuilder, time.Second, 2);
    }
}
```
3. In `Project Settings > NjConsole > Extension Modules`, add your new class to the list.
4. Click `Apply Extension Changes` to reload.
5. In the **Logs Panel**, click the **Time** dropdown (top-right), and select **Custom Module**.  
Note. if you have multiple `IConsoleTimestampFormatter` modules added, it might not pick the right one.

[NjConsole doc home](index.md)