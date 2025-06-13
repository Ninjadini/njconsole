#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
#define INPUT_SYSTEM_ONLY
#endif

using System;
using Ninjadini.Console;
using UnityEngine;

/**
 * Console extension to control TimeScale via Keybinding and options menu
 * 
 * How to install:
 * 1. Windows > Package Manager
 * 2. Add > install from git URL > https://github.com/Ninjadini/njconsole.git?path=extension-modules/TimeScaleControl
 * 3. Project Settings > Ninjadini Console > Extension Modules > Add Extension Module > select TimeScaleNjConsoleExtension
 *
 * Default settings:
 * Options menu: Extensions > TimeScale > Ultra Slow / Slow / Normal / Fast / Ultra Fast
 * Key bindings:
 * * Ultra Slow = shift+S
 * * Slow = S
 * * Normal = D
 * * Fast = F
 * * Ultra Fast = shift+F
 * ^ Please change these values if your game requires these keys such as an FPS game.
 */
[Serializable]
public class TimeScaleNjConsoleExtension : IConsoleExtension
{
    [Tooltip("The folder the options will show up in - they need to go somewhere, even if you only ever use the keybindings and not the UI buttons.")]
    [SerializeField] string parentFolder = "Extensions/TimeScale/";
    
    [SerializeField] float slowScale = 0.15f;
    [SerializeField] float ultraSlowScale = 0f;
    [SerializeField] float fastScale = 3f;
    [SerializeField] float ultraFastScale = 20f;

    [SerializeField] KeyAndModifer slowKey = new KeyAndModifer()
    {
#if INPUT_SYSTEM_ONLY
        key = UnityEngine.InputSystem.Key.S
#else
        keyCode = KeyCode.S
#endif
    };
    
    [SerializeField] KeyAndModifer ultraSlowKey = new KeyAndModifer()
    {
#if INPUT_SYSTEM_ONLY
        key = UnityEngine.InputSystem.Key.S,
#else
        keyCode = KeyCode.S,
#endif
        modifier = ConsoleKeyBindings.Modifier.Shift
    };
    
    [SerializeField] KeyAndModifer normalKey = new KeyAndModifer()
    {
#if INPUT_SYSTEM_ONLY
        key = UnityEngine.InputSystem.Key.D
#else
        keyCode = KeyCode.D
#endif
    };
    
    [SerializeField] KeyAndModifer fastKey = new KeyAndModifer()
    {
#if INPUT_SYSTEM_ONLY
        key = UnityEngine.InputSystem.Key.F
#else
        keyCode = KeyCode.F
#endif
    };
    
    [SerializeField] KeyAndModifer ultraFastKey = new KeyAndModifer()
    {
#if INPUT_SYSTEM_ONLY
        key = UnityEngine.InputSystem.Key.F,
#else
        keyCode = KeyCode.F,
#endif
        modifier = ConsoleKeyBindings.Modifier.Shift
    };
    
    void IConsoleModule.OnAdded(ConsoleModules modules)
    {
        var options = modules.GetOrCreateModule<ConsoleOptions>();
        var catalog = options.CreateCatalog();

        var folder = parentFolder?.Trim() ?? "";
        if (!folder.EndsWith("/"))
        {
            folder += "/";
        }
        
        AddBtn(catalog, folder + "Ultra Slow", ultraSlowScale, ultraSlowKey);
        AddBtn(catalog, folder + "Slow", slowScale, slowKey);
        
        var btn = catalog.AddButton(folder + "Normal", ResetSpeed);
        btn.AutoCloseOverlay();
        normalKey.TryBindTo(btn);
        
        AddBtn(catalog, folder + "Fast", fastScale, fastKey);
        AddBtn(catalog, folder + "Ultra Fast", ultraFastScale, ultraFastKey);
    }

    void AddBtn(ConsoleOptions.Catalog catalog, string path, float speed, KeyAndModifer key)
    {
        if (Mathf.Approximately(speed, 1f))
        {
            return;
        }
        var btn = catalog.AddButton(path, () =>
        {
            SetSpeed(speed);
        });
        btn.AutoCloseOverlay();
        key.TryBindTo(btn);
    }

    protected virtual void SetSpeed(float v)
    {
        Time.timeScale = v;
    }

    // If you have your own speed controls this is how you control it:
    // Make a new class extending TimeScaleNjConsoleExtension, and override this `ResetSpeed()` method and apply your own speed here instead.
    // Add your custom extension to console instead of the base one.
    protected virtual void ResetSpeed()
    {
        Time.timeScale = 1f;
    }

    [Serializable]
    struct KeyAndModifer
    {
#if INPUT_SYSTEM_ONLY
        public UnityEngine.InputSystem.Key key;
#else
        public KeyCode keyCode;
#endif
        public ConsoleKeyBindings.Modifier modifier;

        public void TryBindTo(ConsoleOptions.ItemResultWithKeyBind btn)
        {
#if INPUT_SYSTEM_ONLY
            if (key == UnityEngine.InputSystem.Key.None)
            {
                return;
            }
            btn.BindToKeyboard(key, modifier);
#else
            if (keyCode == KeyCode.None)
            {
                return;
            }
            btn.BindToKeyboard(keyCode, modifier);
#endif
        }
    }
}
