using MelonLoader;
using UnityEngine;

[assembly: MelonInfo(typeof(Lilly_s_Beyond_Limits.MelonLoad), "Lilly's Beyond Limits", "3.0.7", "Lilly", null)]
[assembly: MelonGame("KisSoft", "ATLYSS")]
[assembly: MelonOptionalDependencies("BepInEx")]

namespace Lilly_s_Beyond_Limits
{
    public class MelonLoad : MelonMod
    {
        
        private MelonPreferences_Category general;
        private MelonPreferences_Entry<bool> cameraCol;
        private MelonPreferences_Entry<Vector2> _headWidthRange;
        private MelonPreferences_Entry<Vector2> _headModRange;
        private MelonPreferences_Entry<Vector2> _heightRange;
        private MelonPreferences_Entry<Vector2> _widthRange;
        private MelonPreferences_Entry<Vector2> _torsoRange;
        private MelonPreferences_Entry<Vector2> _boobRange;
        private MelonPreferences_Entry<Vector2> _armRange;
        private MelonPreferences_Entry<Vector2> _bellyRange;
        private MelonPreferences_Entry<Vector2> _bottomRange;
        private MelonPreferences_Entry<Vector2> _pitchRange;

        BeyondCore beyondcore;
        public override void OnInitializeMelon()
        {
            general = MelonPreferences.CreateCategory("General");
            cameraCol = general.CreateEntry<bool>("General", false, "CameralCollision", "Toggles Camera Colision and Infinite Zoom");
            _headWidthRange = general.CreateEntry<Vector2>("SliderHeadWidth", new Vector2(0.95f, 1.15f), "SliderHeadWidth", "Sets Sliider Limits For Head Width");
            _headModRange = general.CreateEntry<Vector2>("SliderMuzzle", new Vector2(0f, 100f), "SliderMuzzle", "Sets Sliider Limits For Muzzle");
            _heightRange = general.CreateEntry<Vector2>("SliderHeight", new Vector2(0.95f, 1.055f), "SliderHeight", "Sets Sliider Limits For Height");
            _widthRange = general.CreateEntry<Vector2>("SliderWidth", new Vector2(0.95f, 1.15f), "SliderWidth", "Sets Sliider Limits For Width");
            _torsoRange = general.CreateEntry<Vector2>("SliderTorso", new Vector2(-10f, 105f), "SliderTorso", "Sets Sliider Limits For Torso");
            _boobRange = general.CreateEntry<Vector2>("SliderBoobs", new Vector2(-20f, 100f), "SliderBoobs", "Sets Sliider Limits For Boobs");
            _armRange = general.CreateEntry<Vector2>("SliderArms", new Vector2(-5f, 105f), "SliderArms", "Sets Sliider Limits For Arms");
            _bellyRange = general.CreateEntry<Vector2>("SliderBelly", new Vector2(-20f, 120f), "SliderBelly", "Sets Sliider Limits For Belly");
            _bottomRange = general.CreateEntry<Vector2>("SliderButt", new Vector2(-50f, 100f), "SliderButt", "Sets Sliider Limits For Butt");
            _pitchRange = general.CreateEntry<Vector2>("SliderVoice", new Vector2(0.6f, 1.25f), "SliderVoice", "Sets Sliider Limits For Voice");

            if (BeyondCore.Beyondinstance != null)
                return;
            GameObject g = GameObject.Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube));
            g.hideFlags = UnityEngine.HideFlags.HideAndDontSave;
            beyondcore = g.AddComponent<BeyondCore>();
            beyondcore.Logger = logger;
            beyondcore.saveConfig = saveSettings;

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
