using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ninjadini.Console.Extensions
{
    /// <summary>
    /// NjConsole extension for Key Up Binding support
    /// WARNING: This only works for old Input Manager. NOT new Input System.
    /// How to install (choose 1):
    /// a. Copy the source files to your project.
    /// b. Window > Package Manager > Add > install from git URL > https://github.com/Ninjadini/njconsole.git?path=extension-modules/KeyUpBinding
    ///
    /// <code>
    /// var keyUpBindings = NjConsole.Modules.GetOrCreateModule &lt; KeyUpBindingExtension &gt; ();
    /// keyUpBindings.BindKeyUp(() => Debug.Log("A UP"), KeyCode.A);
    /// keyUpBindings.BindKeyUp(() => Debug.Log("ctrl+shift+B UP"), KeyCode.B, ConsoleKeyBindings.Modifier.Ctrl | ConsoleKeyBindings.Modifier.Shift);
    /// </code>
    /// </summary>
    [Serializable]
    public class KeyUpBindingExtension : IConsoleExtension
    {
        ConsoleOverlay _overlay;
        readonly Dictionary<(KeyCode, ConsoleKeyBindings.Modifier), Action> _upBinding = new();

        bool _hadAnyKey;
        bool _disabled;
        
        public void TryInit()
        {
            if (_overlay?.UIDocument) return;

            _overlay = ConsoleOverlay.Instance;
            
            if (!_overlay?.UIDocument) return;

            if (!CanStart())
            {
                _disabled = true;
                return;
            }

            _overlay.schedule.Execute(Update).Every(0);
        }

        bool CanStart()
        {
            if (!Application.isEditor && _overlay != null && !_overlay.Context.Settings.inPlayerKeyBindings)
            {
                return false;
            }
            return true;
        }
        
        public void BindKeyUp(Action callback, KeyCode key, ConsoleKeyBindings.Modifier mods = default)
        {
            if (_disabled) return;
            TryInit();
            _upBinding[(key, mods)] = callback;
        }

        public bool IsBoundToKeyUp(KeyCode key, ConsoleKeyBindings.Modifier mods = default, Action requiredCallback = null)
        {
            return !_disabled && IsBound(_upBinding, key, mods, requiredCallback);
        }
        
        public void UnbindKeyUp(KeyCode key, ConsoleKeyBindings.Modifier mods, Action requiredCallback = null)
        {
            UnBind(_upBinding, key, mods, requiredCallback);
        }

        void UnBind(Dictionary<(KeyCode, ConsoleKeyBindings.Modifier), Action> dict, KeyCode key, ConsoleKeyBindings.Modifier mods, Action requiredCallback = null)
        {
            if (requiredCallback != null)
            {
                if(dict.TryGetValue((key, mods), out var existingCallback) && existingCallback == requiredCallback)
                {
                    dict.Remove((key, mods));
                }
            }
            else
            {
                dict.Remove((key, mods));
            }
        }

        public bool IsBound(Dictionary<(KeyCode, ConsoleKeyBindings.Modifier), Action> dict, KeyCode key, ConsoleKeyBindings.Modifier mods, Action requiredCallback)
        {
            if (!dict.TryGetValue((key, mods), out var existingCallback))
            {
                return false;
            }
            return requiredCallback == null || requiredCallback == existingCallback;
        }
        
        void Update()
        {
            if (!Application.isEditor && _overlay.IsAccessChallengeRequired())
            {
                return;
            }
            if (!Input.anyKey && !_hadAnyKey)
            {
                return;
            }
            _hadAnyKey = Input.anyKey;
            
            foreach (var kv in _upBinding)
            {
                if (Input.GetKeyUp(kv.Key.Item1) && ModifiersMatch(kv.Key.Item2))
                {
                    kv.Value?.Invoke();
                }
            }
        }

        bool ModifiersMatch(ConsoleKeyBindings.Modifier mods)
        {
            return  ((mods & ConsoleKeyBindings.Modifier.Ctrl) != 0) == (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                    && ((mods & ConsoleKeyBindings.Modifier.Shift) != 0) == (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                    && ((mods & ConsoleKeyBindings.Modifier.Alt) != 0) == (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
                    && ((mods & ConsoleKeyBindings.Modifier.Cmd) != 0) == (Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand));
        }
    }
}