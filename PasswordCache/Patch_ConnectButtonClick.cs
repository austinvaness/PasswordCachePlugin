using HarmonyLib;
using Sandbox.Game.Screens;

namespace avaness.PasswordCache
{
    [HarmonyPatch(typeof(MyGuiScreenServerPassword), "ConnectButtonClick")]
    public static class Patch_ConnectButtonClick
    {
        public static void Postfix(MyGuiScreenServerPassword __instance)
        {
            Main.OnPasswordEntered(__instance);
        }
    }
}
