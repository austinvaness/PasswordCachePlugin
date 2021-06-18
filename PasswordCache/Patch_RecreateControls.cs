using HarmonyLib;
using Sandbox.Game.Screens;

namespace avaness.PasswordCache
{
    [HarmonyPatch(typeof(MyGuiScreenServerPassword), "RecreateControls")]
    public static class Patch_RecreateControls
    {
        public static void Postfix(MyGuiScreenServerPassword __instance)
        {
            Main.OnPasswordNeeded(__instance);
        }
    }
}
