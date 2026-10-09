using HarmonyLib;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ModMenu.UI
{
    /// <summary>Keep the vanilla menu root active so its pause state and visibility checks still apply.</summary>
    [HarmonyPatch]
    internal static class InGameMenuButton
    {
        private const string ButtonName = "ModMenuButton";
        private static int _closedFrame = -1;

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Menu), "Start")]
        private static void Add(Menu __instance)
        {
            try
            {
                Button template = __instance.m_settingsButton;
                if (template == null || template.transform.parent.Find(ButtonName) != null)
                {
                    return;
                }
                GameObject copy = Object.Instantiate(template.gameObject, template.transform.parent);
                copy.name = ButtonName;
                foreach (MonoBehaviour behaviour in copy.GetComponentsInChildren<MonoBehaviour>(true))
                {
                    if (behaviour.GetType().Name == "Localize")
                    {
                        Object.Destroy(behaviour);
                    }
                }
                foreach (UIGamePad pad in copy.GetComponentsInChildren<UIGamePad>(true))
                {
                    if (pad.m_hint != null) Object.Destroy(pad.m_hint);
                    Object.Destroy(pad);
                }
                TMP_Text label = copy.GetComponentInChildren<TMP_Text>(true);
                if (label != null) label.text = T.Get("menu_button");
                else
                {
                    Text legacy = copy.GetComponentInChildren<Text>(true);
                    if (legacy != null) legacy.text = T.Get("menu_button");
                }
                copy.transform.SetSiblingIndex(template.transform.GetSiblingIndex() + 1);
                Button button = copy.GetComponent<Button>();
                button.onClick = new Button.ButtonClickedEvent();
                button.onClick.AddListener(() => OpenWindow(__instance));
                copy.SetActive(true);
                __instance.UpdateNavigation();
                __instance.m_rebuildLayout = true;
            }
            catch (System.Exception e)
            {
                Plugin.Log.LogError($"Could not add the in-game Mods button: {e}");
            }
        }

        // Vanilla rebuilds explicit navigation each time the menu opens. Splice our button into that chain.
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Menu), "UpdateNavigation")]
        private static void UpdateNavigation(Menu __instance)
        {
            Button settings = __instance.m_settingsButton;
            if (settings == null) return;
            Transform transform = settings.transform.parent.Find(ButtonName);
            Button mods = transform != null ? transform.GetComponent<Button>() : null;
            if (mods == null) return;
            Navigation navigation = settings.navigation;
            Selectable next = navigation.selectOnDown;
            if (next == null || next == mods) return;
            Navigation modsNavigation = navigation;
            modsNavigation.selectOnUp = settings;
            modsNavigation.selectOnDown = next;
            mods.navigation = modsNavigation;
            navigation.selectOnDown = mods;
            settings.navigation = navigation;
            Navigation nextNavigation = next.navigation;
            nextNavigation.selectOnUp = mods;
            next.navigation = nextNavigation;
        }

        internal static bool CanOpen()
        {
            if (Game.instance == null || Game.instance.IsShuttingDown() || Player.m_localPlayer == null
                || Menu.instance == null || ModMenuWindow.IsOpen)
            {
                return false;
            }
            // Do not steal a shortcut while typing, loading, or interacting with another game window.
            GameObject selected = EventSystem.current?.currentSelectedGameObject;
            if (selected != null && selected.GetComponent<InputField>()?.isFocused == true) return false;
            Menu menu = Menu.instance;
            return !Console.IsVisible() && !TextInput.IsVisible() && !InventoryGui.IsVisible()
                && !Minimap.IsOpen() && !StoreGui.IsVisible() && !UnifiedPopup.IsVisible()
                && !Hud.IsPieceSelectionVisible() && !Hud.InRadial() && !PlayerCustomizaton.IsBarberGuiVisible()
                && (Chat.instance == null || !Chat.instance.HasFocus())
                && (ZNet.instance == null || (!ZNet.instance.InPasswordDialog() && !ZNet.instance.InConnectingScreen()))
                && menu.m_settingsInstance == null && !menu.PlayerListActive && !Feedback.IsVisible()
                && (menu.m_quitDialog == null || !menu.m_quitDialog.gameObject.activeInHierarchy)
                && (menu.m_logoutDialog == null || !menu.m_logoutDialog.gameObject.activeInHierarchy);
        }

        internal static void OpenWindow(Menu menu)
        {
            if (menu == null || !CanOpen()) return;
            if (!menu.m_root.gameObject.activeSelf) menu.Show();
            menu.m_menuDialog.gameObject.SetActive(false);
            ModMenuWindow.Open(() =>
            {
                _closedFrame = Time.frameCount;
                if (menu != null && menu.m_root != null && menu.m_root.gameObject.activeInHierarchy)
                {
                    menu.m_menuDialog.gameObject.SetActive(true);
                    menu.UpdateNavigation();
                    EventSystem.current?.SetSelectedGameObject(menu.m_settingsButton.gameObject);
                }
            });
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Menu), "Update")]
        private static bool Update()
        {
            // Esc/B belongs to the custom window, including the frame it closes. Avoid selecting hidden buttons.
            return Game.instance == null || Game.instance.IsShuttingDown()
                || (!ModMenuWindow.IsOpen && Time.frameCount != _closedFrame);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Menu), nameof(Menu.Hide))]
        private static void MenuHidden()
        {
            // Other mods or shutdown may hide the vanilla menu without going through our close button.
            ModMenuWindow.CloseIfOpen();
        }
    }
}
