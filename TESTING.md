# In-world ModMenu development build

The local package is `dist/ModMenu-0.2.1.zip`. This is a development build, not an upstream release.

## Install

1. In r2modman or Thunderstore Mod Manager, use **Import local mod** and select the ZIP.
2. Ensure BepInEx and Jotunn are enabled. Only one ModMenu package should be enabled: replace or disable the original package when testing this build.
3. Launch Valheim through the mod manager.

The ZIP includes both `plugins/ModMenu/ModMenu.dll` and `patchers/ModMenu/ModMenu.Patcher.dll`, plus the package manifest, icon, README and changelog. It does not bundle game or dependency DLLs.

## In-game checks

- Check the main-menu Mods button and Ctrl+M still work.
- Load a test world. Open the escape menu and click **Mods**, then try Ctrl+M directly during gameplay. The map should not open with the default Ctrl+M shortcut.
- Confirm the cursor can operate the window, movement/attacks/camera input are blocked, and closing returns to the escape menu. Continue should restore normal controls.
- Open a mod's settings and change a setting known to support live updates. Verify the effect and that the config file saves. Mods that only read settings at startup still require a restart.
- Check Escape/B returns from settings to the mod list, then closes the window without also dismissing the escape menu. Escape while typing should only leave the field.
- Check the shortcut is ignored while chatting, typing, using inventory/map, or in a settings/quit/logout dialog.
- Change a mod's enabled state or a profile. The status should explain that a restart is needed, and **Restart game** should be absent while in-world. Log out normally, then check it appears in the main menu.
- Log out and load another world; reopen the window and check input is restored after closing.
- With VikingQoL enabled, verify both escape-menu buttons remain usable.
- If using multiplayer, check both host and client. The world continues running according to Valheim's normal pause rules.

For errors, inspect the profile's `BepInEx/LogOutput.log` for `Mod Menu`, `ModMenu`, `Jotunn` or Harmony exceptions.

## Validation performed

Release builds use the locally installed Valheim 1.0 assemblies, BepInEx 5.4.2351 and Jotunn 2.30.2, without deploying to the game or modifying a mod-manager profile. The existing unit tests cover config parsing and update/source matching; they do not validate Unity UI behaviour. In-game checks above remain manual.

## Rebuild on this machine

The ignored `_ref` folder contains copied BepInEx/Jotunn references and a junction to the game's managed assemblies. In PowerShell from the repository:

```powershell
$env:VALHEIM_DIR = Join-Path (Get-Location) '_ref'
./tools/package.ps1 ModMenu
dotnet test tests/ModMenu.Tests -c Release -p:DeployToGame=false
```

On another machine, set `VALHEIM_DIR` to a game installation with BepInEx and `BepInEx/plugins/Jotunn/Jotunn.dll`. The packaging script builds without deploying to the installation.
