
# ⚠️ Known Issues
🅰️  JetBrainsMono font may render certain letter pairs joined (e.g. xc, ex, ye, Sc).   
This appears to affect only the Editor's Game view, not builds.


# ❓ Troubleshooting

### I am getting errors after importing the package
1. Make sure your Unity is at least 2022.1. (best if it is 2022.3 or newer)
2. Are you using the new Input system? If so, ensure the Input System package is installed from Package Manager.   
   Alternatively, go to Project Settings > Player > Other Settings > Active Input Handling > choose `Input Manager (old)`

### Key bindings doesn't work in the player build (standalone/mobile etc)
Key bindings are disabled by default outside Editor, you can enable it from Project Settings > Ninjadini Console > Features > In Player Key Bindings

### Overlay UI scale looks wrong in Play Mode
Unfortunately, this can happen when Unity’s Game View scaling (especially with high-DPI displays) doesn’t play nicely with UI Toolkit’s resolution scaling in certain setups.
You can manually fix it during Play Mode by opening the NjConsole overlay and adjusting the scale: `Utilities > Tools > UI Scale + / -`


---
[NjConsole doc home](index.md)