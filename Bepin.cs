using BepInEx;
using UnityEngine;
using HarmonyLib;


namespace Lilly_s_Beyond_Limits
{
    [BepInPlugin("0a26c5bd-f173-47f8-8f50-006dd6806ce6", "Lilly's Beyond Limits", "3.0.2")]
    public class Bepin : BaseUnityPlugin
    {
        BeyondCore beyondcore;
        private void Awake()
        {
            if (BeyondCore.Beyondinstance != null)
                return;
            GameObject g = GameObject.Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube));
            g.hideFlags = UnityEngine.HideFlags.HideAndDontSave;
            beyondcore = g.AddComponent<BeyondCore>();
            var harmony = new HarmonyLib.Harmony("Lilly's Beyond Limits");
            harmony.PatchAll();
        }
    }
}
