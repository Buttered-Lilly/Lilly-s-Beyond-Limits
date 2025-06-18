using BepInEx;
using UnityEngine;
using HarmonyLib;
using BepInEx.Configuration;


namespace Lilly_s_Beyond_Limits
{
    [BepInPlugin("0a26c5bd-f173-47f8-8f50-006dd6806ce6", "Lilly's Beyond Limits", "3.0.7")]
    public class Bepin : BaseUnityPlugin
    {
        private ConfigEntry<bool> cameraCol;
        private ConfigEntry<Vector2> _headWidthRange;
        private ConfigEntry<Vector2> _headModRange;
        private ConfigEntry<Vector2> _heightRange;
        private ConfigEntry<Vector2> _widthRange;
        private ConfigEntry<Vector2> _torsoRange;
        private ConfigEntry<Vector2> _boobRange;
        private ConfigEntry<Vector2> _armRange;
        private ConfigEntry<Vector2> _bellyRange;
        private ConfigEntry<Vector2> _bottomRange;
        private ConfigEntry<Vector2> _pitchRange;

        BeyondCore beyondcore;

        private void Awake()
        {
            cameraCol = Config.Bind("General", "CameralCollision", false, "Toggles Camera Colision and Infinite Zoom");
            _headWidthRange = Config.Bind("General", "SliderHeadWidth", new Vector2(0.95f, 1.15f), "Sets Sliider Limits For Head Width");
            _headModRange = Config.Bind("General", "SliderMuzzle", new Vector2(0f, 100f), "Sets Sliider Limits For Muzzle");
            _heightRange = Config.Bind("General", "SliderHeight", new Vector2(0.95f, 1.055f), "Sets Sliider Limits For Height");
            _widthRange = Config.Bind("General", "SliderWidth", new Vector2(0.95f, 1.15f), "Sets Sliider Limits For Width");
            _torsoRange = Config.Bind("General", "SliderTorso", new Vector2(-10f, 105f), "Sets Sliider Limits For Torso");
            _boobRange = Config.Bind("General", "SliderBoobs", new Vector2(-20f, 100f), "Sets Sliider Limits For Boobs");
            _armRange = Config.Bind("General", "SliderArms", new Vector2(-5f, 105f), "Sets Sliider Limits For Arms");
            _bellyRange = Config.Bind("General", "SliderBelly", new Vector2(-20f, 120f), "Sets Sliider Limits For Belly");
            _bottomRange = Config.Bind("General", "SliderButt", new Vector2(-50f, 100f), "Sets Sliider Limits For Butt");
            _pitchRange = Config.Bind("General", "SliderVoice", new Vector2(0.6f, 1.25f), "Sets Sliider Limits For Voice");


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

            beyondcore._headWidthRange = _headWidthRange.Value;
            beyondcore._headModRange = _headModRange.Value;
            beyondcore._heightRange = _heightRange.Value;
            beyondcore._widthRange = _widthRange.Value;
            beyondcore._torsoRange = _torsoRange.Value;
            beyondcore._boobRange = _boobRange.Value;
            beyondcore._armRange = _armRange.Value;
            beyondcore._bellyRange = _bellyRange.Value;
            beyondcore._bottomRange = _bottomRange.Value;
            beyondcore._pitchRange = _pitchRange.Value;
        }
        public bool saveSettings(string _)
        {
            try
            {
                cameraCol.Value = beyondcore.col;
                _headWidthRange.Value = beyondcore._headWidthRange;
                _headModRange.Value = beyondcore._headModRange;
                _heightRange.Value = beyondcore._heightRange;
                _widthRange.Value = beyondcore._widthRange;
                _torsoRange.Value = beyondcore._torsoRange;
                _boobRange.Value = beyondcore._boobRange;
                _armRange.Value = beyondcore._armRange;
                _bellyRange.Value = beyondcore._bellyRange;
                _bottomRange.Value = beyondcore._bottomRange;
                _pitchRange.Value = beyondcore._pitchRange;

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
