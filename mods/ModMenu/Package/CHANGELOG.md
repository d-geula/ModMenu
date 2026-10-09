# Changelog

## 0.2.2 (development)
- Configurable wheel/trackpad scrolling: 3x speed in Mod Menu by default; set UI/ScrollWheelMultiplier to 1 for the original speed.
- UI/AffectOtherMenus optionally applies the multiplier to other Unity scroll menus. It is off by default. Changes take effect immediately.
- Only wheel handling is scaled; scrollbar dragging, gameplay wheel actions and the menus' saved sensitivity values are unchanged.

## 0.2.1 (development)
- Mods button in the in-world escape menu; Ctrl+M also opens it during gameplay.
- Opening uses the normal pause menu and blocks gameplay input. Closing restores that menu; Escape/B does not close both windows at once.
- Shortcuts do not interrupt chat, text fields, loading screens, or other game dialogs. Input blocking is released when the window closes or is destroyed.
- Restart is unavailable while a world is loaded. Log out normally first; mod on/off and profile changes still apply at the next restart.
- Settings changes still notify the owning mod immediately; whether they apply live depends on that mod.

## 0.2.0
- Works with far more mods: settings from every config file a mod uses (not only its main one), the ConfigurationManager Category, IsAdvanced and HideDefaultButton tags, and mods that turn auto-save off.
- Big mods: sections fold (folded by default above 40 settings) and a settings search box.
- Long lists of allowed values are typed in and checked instead of a huge dropdown; invalid values are refused.
- Update check: parallel Thunderstore requests and batched Nexus queries (fast with 100+ mods); hand-installed Nexus mods are found by name.
- Ctrl+M opens the menu even when another mod reworks the main menu (General/OpenKey).
- Gamepad B closes the window; if Jotunn's GUI is not ready the main menu stays usable instead of hiding.
- Numbers accept a decimal comma (0,5); a slider over a huge range can no longer throw.
- Restart also works outside Steam and on Linux/macOS; the disabling patcher handles loaders that scan several plugin folders.

## 0.1.0
- Mods button in the main menu with the list of every installed mod (loaded, switched off, failed).
- Switch mods on/off without touching their files (preloader patcher; safe with Vortex and r2modman), restart button.
- Settings editor for every loaded mod, with a Default button per setting.
- Profiles of switched-off mods.
- Update check against Nexus Mods (Vortex installs) and Thunderstore, no account needed.
