# Add custom panel (UIToolkit format)

A demo panel code:
```
public class DemoPanelModule : IConsolePanelModule
{
 public string Name => "DemoPanel"; // Name to display on the side panel

 public float SideBarOrder => 12; // The order of the button to appear on the side panel

 public VisualElement CreateElement(ConsoleContext context)
 {
     // Create the VisualElement for the panel - this is for that specific ConsoleContext.
     // Remember you can have multiple editor console windows and at runtime too.
     // The context will be unique per window.
     return new BasicDemoPanel();
 }
}

class BasicDemoPanel : VisualElement
{
    public BasicDemoPanel()
    {
        AddToClassList("panel"); // just adds a background
        Add(new Label("Hello from demo panel"));
    }
}
```

2 ways to register to console.

a. Manually add the panel at runtime by code:  
```
if(NjConsole.Modules.GetModule(typeof(DemoPanelModule)) == null){
    NjConsole.Modules.AddModule(new DemoPanelModule()); 
}
```

b. Add via IConsoleExtension:
 1. Add IConsoleExtension interface to your panel module class.
 2. Add [Serializable] attribute to the class.  
 3. Go to `Project Settings > NjConsole > Extension Modules > Add Extension Module` > add your new class 
 4. Press `Apply changes`

# Add custom panel (OnGUI / IMGUI format)

Alternatively, you can also add your panel using OnGUI rendering.  
```
public class DemoOnGUIPanelModule : IConsoleIMGUIPanelModule
{
   public string Name => "OnGUI";

   public float SideBarOrder => 12;

   public IConsoleIMGUI CreateIMGUIPanel(ConsoleContext context)
   {
      // Each ConsoleContext is a different ui window, so we need different instance per window.
      return new BasicOnGUIDemoPanel();
   }
}

class BasicOnGUIDemoPanel : IConsoleIMGUI
{
   public void OnGUI()
   {
      GUILayout.Label("Hello from OnGUI");
   }
}
```