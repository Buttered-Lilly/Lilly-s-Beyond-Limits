using MelonLoader;
using Mirror.SimpleWeb;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[assembly: MelonInfo(typeof(Lilly_s_Beyond_Limits.MelonLoad), "Lilly's Beyond Limits", "3.0.2", "Lilly", null)]
[assembly: MelonGame("KisSoft", "ATLYSS")]
[assembly: MelonOptionalDependencies("BepInEx")]

namespace Lilly_s_Beyond_Limits
{
    public class MelonLoad : MelonMod
    {
        BeyondCore beyondcore;
        public override void OnInitializeMelon()
        {
            if (BeyondCore.Beyondinstance != null)
                return;
            GameObject g = GameObject.Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube));
            g.hideFlags = UnityEngine.HideFlags.HideAndDontSave;
            beyondcore = g.AddComponent<BeyondCore>();
        }
    }
}
