using MelonLoader;
using Mirror;
using Mirror.SimpleWeb;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[assembly: MelonInfo(typeof(Lilly_s_Beyond_Limits.MelonLoad), "Lilly's Beyond Limits", "3.0.6", "Lilly", null)]
[assembly: MelonGame("KisSoft", "ATLYSS")]
[assembly: MelonOptionalDependencies("BepInEx")]

namespace Lilly_s_Beyond_Limits
{
    public class MelonLoad : MelonMod
    {
        
        private MelonPreferences_Category general;
        private MelonPreferences_Entry<bool> cameraCol;

        BeyondCore beyondcore;
        public override void OnInitializeMelon()
        {
            general = MelonPreferences.CreateCategory("General");
            cameraCol = general.CreateEntry<bool>("General", false, "CameralCollision", "Toggles Camera Colision and Infinite Zoom");

            if (BeyondCore.Beyondinstance != null)
                return;
            GameObject g = GameObject.Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube));
            g.hideFlags = UnityEngine.HideFlags.HideAndDontSave;
            beyondcore = g.AddComponent<BeyondCore>();
            beyondcore.Logger = logger;
            beyondcore.saveConfig = saveSettings;
            beyondcore.col = cameraCol.Value;
        }

        public bool saveSettings(string _)
        {
            try
            {
                cameraCol.Value = beyondcore.col;

                MelonPreferences.Save();
                return true;
            }
            catch (Exception e)
            {
                logger(e.ToString());
                return false;
            }
        }

        public override void OnApplicationQuit()
        {
            cameraCol.Value = beyondcore.col;
        }

        public bool logger(string mesg)
        {
            MelonLogger.Msg(mesg);
            return true;
        }
    }
}
