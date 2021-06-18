using HarmonyLib;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Screens;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage.Plugins;

namespace avaness.PasswordCache
{
    public class Main : IPlugin
    {
        private static readonly Dictionary<string, string> passwords = new Dictionary<string, string>();

        public void Dispose()
        { }

        public void Init(object gameInstance)
        {
            Harmony harmony = new Harmony("avaness.PasswordCache");
            harmony.PatchAll();
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
            if (TryGetServerId(out string id) && TryGetPasswordField(screen, out MyGuiControlTextbox textbox))
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
            if (TryGetServerId(out string id))
                return passwords.TryGetValue(id, out password);
            password = null;
            return false;
        }

        private static bool TryGetServerId(out string id)
        {
            if (MyMultiplayer.Static == null)
            {
                id = null;
                return false;
            }

            id = MyMultiplayer.Static.HostName;
            return !string.IsNullOrWhiteSpace(id);
        }
    }
}
