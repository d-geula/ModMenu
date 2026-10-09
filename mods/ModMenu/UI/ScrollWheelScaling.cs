using HarmonyLib;
using UnityEngine.UI;

namespace ModMenu.UI
{
    /// <summary>Scale only a wheel event, without changing shared event data or a menu's persistent settings.</summary>
    [HarmonyPatch(typeof(ScrollRect), nameof(ScrollRect.OnScroll))]
    internal static class ScrollWheelScaling
    {
        private static void Prefix(ScrollRect __instance, out float? __state)
        {
            __state = null;
            if (__instance == null || Plugin.ScrollWheelMultiplier == null) return;
            float original = __instance.scrollSensitivity;
            float multiplier = Plugin.ScrollWheelMultiplier.Value;
            if (multiplier == 1f) return;
            bool affectOthers = Plugin.AffectOtherMenus?.Value == true;
            // Child lists such as dropdowns belong to Mod Menu too. Other menus are untouched by default.
            bool ownMenu = affectOthers || __instance.GetComponentInParent<ModMenuWindow>() != null;
            float scaled = ScaleSensitivity(original, multiplier, ownMenu, affectOthers);
            if (scaled.Equals(original)) return;
            __state = original;
            __instance.scrollSensitivity = scaled;
        }

        private static void Finalizer(ScrollRect __instance, float? __state)
        {
            // Finalizers also run if another patch or the scroll handler throws.
            if (__state.HasValue && __instance != null) __instance.scrollSensitivity = __state.Value;
        }

        internal static float ScaleSensitivity(float original, float multiplier, bool ownMenu, bool affectOthers)
        {
            if ((!ownMenu && !affectOthers) || !IsFinite(original) || !IsFinite(multiplier) || multiplier <= 0f)
            {
                return original;
            }
            float scaled = original * multiplier;
            // Preserve disabled/reversed scrolling, and refuse corrupt or overflowing config values.
            return IsFinite(scaled) ? scaled : original;
        }

        private static bool IsFinite(float value) => !float.IsNaN(value) && !float.IsInfinity(value);
    }
}
