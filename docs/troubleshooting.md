---
title: Troubleshooting
nav_section: "Advanced"
nav_order: 10
nav_icon: "🧯"
---

# ⚙️ Requirements

- **Unity 2022.3 or newer.** NjConsole relies on Unity's UI Toolkit at runtime, which became
  production-ready in 2022.3 LTS. Earlier 2022 versions may have incomplete UI features.
- **Tested platforms:** Standalone Windows/Mac, iOS, Android, WebGL.
- **Tested editor versions:** 2022.3, Unity 6.0 – 6.7.

# ⚠️ Known Issues

🅰️ JetBrainsMono font may render certain letter pairs joined (e.g. xc, ex, ye, Sc).   
This appears to affect only the Editor's Game view, not builds.

🅱️ **Unity 6000.5.0 – 6000.5.8 on Vulkan:** anything inside a scroll view renders blank (log list, options,
inspector), though it still responds to taps. This is a Unity bug (UUM-142280), fixed in 6000.5.9f1.

# ❓ Troubleshooting

#### Overlay UI scale looks wrong in Player Builds
In some cases, the device DPI may not be detected correctly, causing the overlay scale to appear incorrect.
You can quickly adjust the scale per device via: `Utilities > Tools > UI Scale + / -`
For better control, create a custom PanelSettings and assign it under:
`Project Settings > Ninjadini ⌨ Console > Playmode Overlay > Custom Panel Settings`

#### Overlay UI scale looks wrong in Editor Play Mode
Unfortunately, this can happen when Unity’s Game View scaling (especially with high-DPI displays) doesn’t play nicely with UI Toolkit’s resolution scaling in certain setups.
You can manually fix it during Play Mode by opening the NjConsole overlay and adjusting the scale: `Utilities > Tools > UI Scale + / -`

#### I am getting errors after importing the package
1. Make sure your Unity is at least 2022.3.
2. Are you using the new Input system? If so, ensure the Input System package is installed from Package Manager.   
   Alternatively, go to `Project Settings > Player > Other Settings > Active Input Handling` > choose `Input Manager (old)`

#### Key bindings don't work in a player build (standalone/mobile etc)
Key bindings are disabled outside the Editor by default. Enable them from
`Project Settings > Ninjadini ⌨ Console > Features > In Player Key Bindings`.
Once enabled they only fire after the access challenge has been passed.

#### The demo scene's buttons don't respond
If your project uses the new Input System, the demo scene's EventSystem needs updating: select the
**EventSystem** object in the Hierarchy and click **Replace with InputSystemUIInputModule** in the Inspector.

#### The console overlay never appears in play mode
Check `Project Settings > Ninjadini ⌨ Console > Playmode Overlay`:
- If **Auto Start Overlay** is off, call `NjConsole.Overlay.EnsureStarted()` yourself.
- If you have any **Activation Triggers** set up, the overlay starts hidden and waits for one of them.
- If you have an **Access Challenge** set up, it must be passed first.

#### Logs appear twice
You most likely have your own logger forwarding to both `Debug.Log()` and NjLogger. Turn off
`Project Settings > Ninjadini ⌨ Console > Logging > Debug.Log() to NjLogger` — see
[the routing table](logging.md#-where-logs-go--the-three-routing-settings).

---
[NjConsole doc home](index.md)
