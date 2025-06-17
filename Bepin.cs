using BepInEx;
using UnityEngine;
using HarmonyLib;
using BepInEx.Configuration;


namespace Lilly_s_Beyond_Limits
{
    [BepInPlugin("0a26c5bd-f173-47f8-8f50-006dd6806ce6", "Lilly's Beyond Limits", "3.0.6")]
    public class Bepin : BaseUnityPlugin
    {
        private ConfigEntry<bool> cameraCol;

        BeyondCore beyondcore;

        private void Awake()
        {
            cameraCol = Config.Bind("General", "CameralCollision", false, "Toggles Camera Colision and Infinite Zoom");
            if (BeyondCore.Beyondinstance != null)
                return;
            GameObject g = GameObject.Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube));
            g.hideFlags = UnityEngine.HideFlags.HideAndDontSave;
            beyondcore = g.AddComponent<BeyondCore>();
            beyondcore.Logger = logger;
            beyondcore.saveConfig = saveSettings;
            var harmony = new HarmonyLib.Harmony("Lilly's Beyond Limits");
            harmony.PatchAll();
            beyondcore.col = cameraCol.Value;
        }
        public bool saveSettings(string _)
        {
            try
            {
                cameraCol.Value = beyondcore.col;

                Config.Save();
                return true;
            }
            catch (Exception e)
            {
                logger(e.ToString());
                return false;
            }
        }

        private void OnApplicationQuit()
        {
            cameraCol.Value = beyondcore.col;
        }

        public bool logger(string mesg)
        {
            Logger.LogInfo($"{mesg}");
            return true;
        }
    }
}
