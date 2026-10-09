# Mod Menu

Adds a **Mods** button to Valheim's main menu and in-world escape menu.

- **Switch mods on and off** with a checkbox. Nothing is renamed or moved, so Vortex, r2modman and Thunderstore installs stay intact: a small BepInEx patcher (`BepInEx/patchers/ModMenu`) simply does not load the mods you switched off. Changes apply after a restart; the *Restart game* button does it for you.
- **Edit every mod's settings** in a proper window: toggles, dropdowns, sliders and text fields, saved at once, with a *Default* button per setting. Every config file a mod uses is found, big mods get foldable sections and a search box, and the usual ConfigurationManager tags are honoured.
- **Profiles**: save which mods are off (e.g. "co-op" and "solo") and switch between them in one click.
- **Update check** against Nexus Mods (mods installed through Vortex) and Thunderstore. No account or API key needed. Nothing is downloaded; the *Page* button opens the mod's page.

**Ctrl+M** opens the menu too (config `General/OpenKey`), including during gameplay. Closing returns to the escape menu. The shortcut does not interrupt chat, text entry, loading, or other game dialogs.

Settings are saved immediately and notify the owning mod; settings that support live changes can take effect in-world. Other settings require a restart. Mod on/off and profile changes always apply after a restart. The restart button is unavailable in-world: log out normally before restarting. The world follows Valheim's normal pause behaviour, so multiplayer keeps running.

Wheel scrolling is 3x faster in Mod Menu by default. In **Mod Menu -> Settings -> UI**, adjust **ScrollWheelMultiplier** (1 restores the original speed) and optionally enable **AffectOtherMenus** to speed up other Unity scroll menus too. Both settings apply immediately. Scrollbar dragging and gameplay wheel actions are unchanged. Menus with custom scrolling that bypasses Unity's ScrollRect handler are not covered.

This 0.2.2 build is a development version for testing in-world access and wheel scrolling. Install it instead of the original ModMenu, not alongside it. Requires BepInEx and Jotunn.

Mod Menu and Jotunn cannot be switched off from the menu (you would lose the menu). Client-side only: other players and the server do not need it.

If the game does not start after switching a mod off, delete `BepInEx/config/ModMenu/disabled.txt` and every mod loads again.

Source code and issues: https://github.com/Dismonder/ModMenu
