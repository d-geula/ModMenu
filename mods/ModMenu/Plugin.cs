using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using Jotunn.Utils;

namespace ModMenu
{
    [BepInPlugin(PluginInfo.Guid, "Mod Menu", PluginInfo.Version)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    // Client-side tool only: never forces the other players or the server to have it.
    [NetworkCompatibility(CompatibilityLevel.NotEnforced, VersionStrictness.None)]
    // Claim Ctrl+M before the minimap processes the unmodified M binding in its normal Update.
    [UnityEngine.DefaultExecutionOrder(-100)]
    public class Plugin : BaseUnityPlugin
    {
        internal static Plugin Instance;
        internal static ManualLogSource Log;
        internal static ConfigEntry<bool> CheckUpdatesOnOpen;
        internal static ConfigEntry<KeyboardShortcut> OpenKey;
        internal static ConfigEntry<float> ScrollWheelMultiplier;
        internal static ConfigEntry<bool> AffectOtherMenus;

        private Harmony _harmony;

        private void Awake()
        {
            Instance = this;
            Log = Logger;
            CheckUpdatesOnOpen = Config.Bind("Updates", "CheckOnOpen", true,
                "Check Nexus Mods and Thunderstore for newer versions the first time the menu is opened in a session.");

            OpenKey = Config.Bind("General", "OpenKey", new KeyboardShortcut(UnityEngine.KeyCode.M, UnityEngine.KeyCode.LeftControl),
                "Opens the Mods window in the main menu or a loaded world. In a world, closing returns to the pause menu.");

            ScrollWheelMultiplier = Config.Bind("UI", "ScrollWheelMultiplier", 3f,
                new ConfigDescription("Mouse wheel/trackpad speed multiplier for Mod Menu. 1 keeps the original speed. Applies immediately; does not change scrollbar dragging.",
                    new AcceptableValueRange<float>(0.25f, 10f)));
            AffectOtherMenus = Config.Bind("UI", "AffectOtherMenus", false,
                "Also apply ScrollWheelMultiplier to other Unity ScrollRect menus, including game and mod menus. Does not affect custom UI systems or gameplay wheel actions. Applies immediately.");

            _harmony = new Harmony(PluginInfo.Guid);
            _harmony.PatchAll(typeof(Plugin).Assembly);

            Log.LogInfo($"{PluginInfo.Name} {PluginInfo.Version} loaded");
        }

        private void Update()
        {
            if (UI.ModMenuWindow.IsOpen || !OpenKey.Value.IsDown())
            {
                return;
            }
            FejdStartup startup = FejdStartup.instance;
            if (startup != null && startup.m_menuList != null && startup.m_menuList.activeInHierarchy)
            {
                UI.MainMenuButton.OpenWindow(startup);
            }
            else if (UI.InGameMenuButton.CanOpen())
            {
                UI.InGameMenuButton.OpenWindow(Menu.instance);
            }
        }

        private void OnDestroy()
        {
            UI.ModMenuWindow.CloseIfOpen();
            _harmony?.UnpatchSelf();
        }
    }
}
