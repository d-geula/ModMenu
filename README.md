# Mod Menu for Valheim

![Mod Menu](mods/ModMenu/Package/icon.png)

A **Mods** button in Valheim's main menu and in-world escape menu: manage every installed mod without leaving the game.

| | |
|---|---|
| **Download** | [Releases](https://github.com/Dismonder/ModMenu/releases/latest) · [Thunderstore (r2modman)](https://thunderstore.io/c/valheim/p/Dismonder/ModMenu/) |
| Changelog | [CHANGELOG.md](mods/ModMenu/Package/CHANGELOG.md) |
| For mod authors | [MOD_AUTHORS.md](MOD_AUTHORS.md) - how your settings show up, supported tags, update matching |
| License | [MIT](LICENSE) - open source, use and adapt freely |
| Requires | Valheim 1.0.x, [BepInEx 5](https://thunderstore.io/c/valheim/p/denikson/BepInExPack_Valheim/), [Jotunn](https://thunderstore.io/c/valheim/p/ValheimModding/Jotunn/) |

## Features

- **Switch mods on and off** with a checkbox. Nothing is renamed or moved, so Vortex, r2modman and Thunderstore installs
  stay intact: a small BepInEx patcher (`BepInEx/patchers/ModMenu`) simply does not load the mods you switched off.
  Changes apply after a restart - the *Restart game* button does it for you (Steam, other launchers, Linux).
- **Edit every mod's settings** in a window: toggles, dropdowns, sliders and text fields, saved at once, with a *Default*
  button per setting. Every config file a mod uses is found, big mods get foldable sections and a search box, and the
  usual ConfigurationManager tags (Browsable, ReadOnly, Order, DispName, Category, IsAdvanced) are honoured.
- **Profiles**: save which mods are off (e.g. "co-op" and "solo") and switch in one click.
- **Update check** against Nexus Mods (Vortex and hand installs) and Thunderstore. No account or API key needed.
  Nothing is downloaded; the *Page* button opens the mod's page.
- **Ctrl+M** opens the menu too (`General/OpenKey`), including during gameplay. Gamepad B closes it. In-world access uses the normal pause menu and blocks gameplay input; closing returns to the escape menu. Multiplayer follows the game's normal pause rules and keeps running.
- English and Polish (follows the game's language). Client-side only: other players and the server do not need it.

In a loaded world, settings notify the owning mod immediately, but only settings that support live changes take effect without a restart. Mod on/off and profile changes always need a restart. Log out normally first: the restart button is unavailable in-world.

Wheel scrolling is 3x faster in Mod Menu by default. In Mod Menu's own settings, adjust `UI/ScrollWheelMultiplier` (1 restores the original speed) and optionally enable `UI/AffectOtherMenus` for other Unity scroll menus. Changes apply immediately and do not change scrollbar dragging or gameplay wheel actions. Custom UI systems that bypass Unity's `ScrollRect.OnScroll` are not covered.

Mod Menu and Jotunn cannot be switched off from the menu (you would lose the menu). If the game does not start after
switching a mod off, delete `BepInEx/config/ModMenu/disabled.txt` and every mod loads again.

## Install

- **r2modman / Thunderstore Mod Manager:** search for *ModMenu* by Dismonder.
- **By hand:** unpack the release zip into `Valheim/BepInEx/` so that `plugins/ModMenu` ends up in `BepInEx/plugins`
  and `patchers/ModMenu` in `BepInEx/patchers`.

## For mod authors

Mod Menu reads standard BepInEx config entries - your mod needs no dependency on it. [MOD_AUTHORS.md](MOD_AUTHORS.md)
shows which editor each setting type gets, the ConfigurationManager tags it honours, how extra config files are found
and how update notices are matched. Issues and pull requests are welcome.

## Build

.NET SDK 8+ and an installed Valheim with BepInEx and Jotunn (game DLLs are referenced from the install; set
`VALHEIM_DIR` if it is not in the default Steam folder).

```
dotnet build mods/ModMenu              # builds and copies to BepInEx/plugins/ModMenu
dotnet build mods/ModMenu.Patcher      # builds and copies to BepInEx/patchers/ModMenu
dotnet test tests/ModMenu.Tests
pwsh tools/package.ps1 ModMenu         # Thunderstore zip in dist/
```

---

## Po polsku

Przycisk **Mody** w menu głównym Valheim: włączanie i wyłączanie modów bez ruszania plików (działa z Vortexem
i r2modman), ustawienia każdego moda w wygodnym oknie, profile modów i sprawdzanie aktualizacji (Nexus, Thunderstore).
**Ctrl+M** też otwiera menu. Instalacja: rozpakuj zip z [Wydań](https://github.com/Dismonder/ModMenu/releases/latest)
do `Valheim/BepInEx/` albo zainstaluj przez r2modman. Gdyby gra nie wstała po wyłączeniu jakiegoś moda, usuń
`BepInEx/config/ModMenu/disabled.txt`.

Kod jest otwarty (licencja MIT): inni twórcy mogą go sprawdzać, wykorzystywać i dopasować swoje mody - zobacz
[MOD_AUTHORS.md](MOD_AUTHORS.md).

Made by the author of [Age of Jarls](https://github.com/Dismonder/AgeOfJarls). License: [MIT](LICENSE).
