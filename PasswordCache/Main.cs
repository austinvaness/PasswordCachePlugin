using HarmonyLib;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Screens;
using Sandbox.Graphics.GUI;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using VRage.Plugins;

namespace avaness.PasswordCache
{
    public class Main : IPlugin
    {
        private static readonly Dictionary<ulong, string> passwords = new Dictionary<ulong, string>();

        public void Dispose()
        { }

        public void Init(object gameInstance)
        {
            new Harmony("avaness.PasswordCache").PatchAll(Assembly.GetExecutingAssembly());
        }

        public void Update()
        { }

        public static void OnPasswordNeeded(MyGuiScreenServerPassword screen)
        {
            if(TryGetPassword(out string password) && TryGetPasswordField(screen, out MyGuiControlTextbox textbox))
            {
                textbox.SetText(new StringBuilder(password));
                textbox.SelectAll();
                textbox.MoveCarriageToEnd();
            }
        }

        public static void OnPasswordEntered(MyGuiScreenServerPassword screen)
        {
            if (TryGetServerId(out ulong id) && TryGetPasswordField(screen, out MyGuiControlTextbox textbox))
            {
                StringBuilder sb = new StringBuilder();
                textbox.GetText(sb);
                if (sb.Length <= 0)
                    passwords.Remove(id);
                else
                    passwords[id] = sb.ToString();
            }
        }

        private static bool TryGetPasswordField(MyGuiScreenServerPassword screen, out MyGuiControlTextbox textbox)
        {
            MyGuiControls controls = screen.Controls;
            for (int i = controls.Count - 1; i >= 0; i--)
            {
                if (controls[i] is MyGuiControlTextbox txt)
                {
                    textbox = txt;
                    return true;
                }
            }
            textbox = null;
            return false;
        }

        private static bool TryGetPassword(out string password)
        {
            if (TryGetServerId(out ulong id))
                return passwords.TryGetValue(id, out password);
            password = null;
            return false;
        }

        private static bool TryGetServerId(out ulong id)
        {
            if (MyMultiplayer.Static == null)
            {
                id = 0;
                return false;
            }

            id = MyMultiplayer.Static.ServerId;
            return id != 0;
        }
    }
}
