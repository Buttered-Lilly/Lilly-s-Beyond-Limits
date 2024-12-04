using MelonLoader;
using HarmonyLib;
using UnityEngine;
using System.Collections;
using static MelonLoader.MelonLogger;

[assembly: MelonInfo(typeof(Lilly_s_Beyond_Limits.BeyondCore), "Lilly's Beyond Limits", "1.0.0", "Lilly", null)]
[assembly: MelonGame("KisSoft", "ATLYSS")]

namespace Lilly_s_Beyond_Limits
{
    public class BeyondCore : MelonMod
    {

        public Vector2[] maxSizes = new Vector2[10];

        static BeyondCore Beyondinstance;
        public PlayerVisual playerVis;
        public PlayerAppearance_Profile profile;
        public PlayerAppearanceStruct playerapp;
        public bool fix = false;
        public bool isSitting = false;

        [HarmonyPatch(typeof(ChatBehaviour), "Cmd_SendChatMessage")]
        public static class chatCommands
        {
            private static bool Prefix(ref ChatBehaviour __instance,  ref string _message)
            {
                try
                {
                    string temp;
                    temp = _message.ToLower();
                    MelonLogger.Msg(temp);
                    string[] Parts;
                    Parts = temp.Split(" ");
                    if (temp.StartsWith("/size"))
                    {
                        bool passed = Beyondinstance.scalePlayer(Parts);
                        if (passed)
                            __instance.New_ChatMessage("Size Changed");
                        else
                            __instance.New_ChatMessage("Size Change Failed");
                        return false;
                    }
                    else if (temp.StartsWith("/slider"))
                    {
                        bool passed = Beyondinstance.setSliderLimit(Parts);
                        if (passed)
                            __instance.New_ChatMessage("Slider Size Changed");
                        else
                            __instance.New_ChatMessage("Slider Size Change Failed");
                        return false;
                    }
                }
                catch (Exception e)
                {
                    MelonLogger.Msg(e);
                    //_message = "<>/";
                    return false;
                }
                return true;
            }
        }

        public bool scalePlayer(string[] Parts)
        {
            PlayerAppearanceStruct playerapp = BeyondCore.Beyondinstance.playerVis._playerAppearanceStruct;
            PlayerAppearance_Profile _aP = BeyondCore.Beyondinstance.profile;
            //_message = "<>/";
            float value;
            try
            {
                float.TryParse(Parts[2], out value);
            }
            catch(Exception e)
            {
                return false;
            }
            if (Parts[1] == "boobs" || Parts[1] == "tits" || Parts[1] == "boob" || Parts[1] == "tit" || Parts[1] == "breast" || Parts[1] == "breasts")
            {
                playerapp._boobWeight = value;
                _aP._boobWeight = value;
            }
            else if (Parts[1] == "scale")
            {
                playerapp._heightWeight = value;
                playerapp._widthWeight = value;
                _aP._heightWeight = value;
                _aP._widthWeight = value;
            }
            else if (Parts[1] == "height")
            {
                playerapp._heightWeight = value;
                _aP._heightWeight = value;
            }
            else if (Parts[1] == "head")
            {
                playerapp._headWidth = value;
                _aP._headWidth = value;
            }
            else if (Parts[1] == "width")
            {
                playerapp._widthWeight = value;
                _aP._widthWeight = value;
            }
            else if (Parts[1] == "butt" || Parts[1] == "ass" || Parts[1] == "bottom")
            {
                playerapp._bottomWeight = value;
                _aP._bottomWeight = value;
            }
            else if (Parts[1] == "belly" || Parts[1] == "stomach")
            {
                playerapp._bellyWeight = value;
                _aP._bellyWeight = value;
            }
            else if (Parts[1] == "muzzle" || Parts[1] == "snout" || Parts[1] == "nose")
            {
                playerapp._muzzleWeight = value;
                _aP._muzzleWeight = value;
            }
            else if (Parts[1] == "voice" || Parts[1] == "Pitch")
            {
                playerapp._voicePitch = value;
                _aP._voicePitch = value;
            }
            else if (Parts[1] == "torso" || Parts[1] == "chest")
            {
                playerapp._torsoWeight = value;
                _aP._torsoWeight = value;
            }
            else if (Parts[1] == "arms")
            {
                playerapp._armWeight = value;
                _aP._armWeight = value;
            }
            else
                return false;
            //MelonLogger.Msg(_message);
            BeyondCore.Beyondinstance.playerVis.Cmd_SendNew_PlayerAppearanceStruct(playerapp);
            ProfileDataManager._current._characterFile._appearanceProfile = _aP;
            ProfileDataManager._current.Save_ProfileData();
            return true;
        }

        public bool setSliderLimit(string[] Parts)
        {
            CharacterParamsGroup param = playerVis._playerRaceModel._scriptablePlayerRace._raceDisplayParams;
            float x, y;
            try
            {
                float.TryParse(Parts[2], out x);
                float.TryParse(Parts[3], out y);
            }
            catch (Exception e)
            {
                return false;
            }
            Vector2 value;
            value = new Vector2(x, y);
            if (Parts[1] == "boobs" || Parts[1] == "tits" || Parts[1] == "boob" || Parts[1] == "tit" || Parts[1] == "breast" || Parts[1] == "breasts")
            {
                param._boobRange = value;
                return true;
            }
            else if (Parts[1] == "height")
            {
                param._heightRange = value;
                return true;
            }
            else if (Parts[1] == "head")
            {
                param._headWidthRange = value;
                return true;
            }
            else if (Parts[1] == "width")
            {
                param._widthRange = value;
                return true;
            }
            else if (Parts[1] == "butt" || Parts[1] == "ass" || Parts[1] == "bottom")
            {
                param._bottomRange = value;
                return true;
            }
            else if (Parts[1] == "belly" || Parts[1] == "stomach")
            {
                param._bellyRange = value;
                return true;
            }
            else if (Parts[1] == "muzzle" || Parts[1] == "snout" || Parts[1] == "nose")
            {
                param._headModRange = value;
                return true;
            }
            else if (Parts[1] == "voice" || Parts[1] == "Pitch")
            {
                param._pitchRange = value;
                return true;
            }
            else if (Parts[1] == "torso" || Parts[1] == "chest")
            {
                param._torsoRange = value;
                return true;
            }
            else if (Parts[1] == "arms")
            {
                param._armRange = value;
                return true;
            }
            return false;
        }


        //level cap remover
        /*[HarmonyPatch(typeof(GameManager), "Start")]
        public static class gamemanager
        {
            private static void Postfix(ref GameManager __instance)
            {
                try
                {
                    __instance._statLogics._maxMainLevel = 10000;
                }
                catch (Exception e)
                {
                    MelonLogger.Msg(e);
                }
            }
        }*/

        [HarmonyPatch(typeof(Player), "Handle_ServerConditions")]
        public static class fixPlayer
        {
            public static void Postfix(ref Player __instance)
            {
                try
                {
                    if (BeyondCore.Beyondinstance.fix || __instance.Network_currentPlayerCondition != PlayerCondition.ACTIVE || !__instance.isLocalPlayer)
                        return;
                    else if (__instance.Network_currentPlayerCondition == PlayerCondition.ACTIVE && __instance.isLocalPlayer)
                    {
                        PlayerAppearanceStruct playerapp = BeyondCore.Beyondinstance.playerVis._playerAppearanceStruct;
                        PlayerAppearance_Profile _aP = BeyondCore.Beyondinstance.profile;
                        playerapp._boobWeight = _aP._boobWeight;
                        playerapp._headWidth = _aP._headWidth;
                        playerapp._bottomWeight = _aP._bottomWeight;
                        playerapp._heightWeight = _aP._heightWeight;
                        playerapp._widthWeight = _aP._widthWeight;
                        playerapp._torsoWeight = _aP._torsoWeight;
                        playerapp._muzzleWeight = _aP._muzzleWeight;
                        playerapp._voicePitch = _aP._voicePitch;
                        playerapp._bellyWeight = _aP._bellyWeight;
                        playerapp._armWeight = _aP._armWeight;
                        //MelonLogger.Msg(_aP._boobWeight);
                        BeyondCore.Beyondinstance.playerVis.Cmd_SendNew_PlayerAppearanceStruct(playerapp);
                        BeyondCore.Beyondinstance.fix = true;
                    }
                }
                catch (Exception e)
                {
                    MelonLogger.Msg(e);
                }
            }
        }

        [HarmonyPatch(typeof(AtlyssNetworkManager), "OnStopClient")]
        public static class reset
        {
            public static void Postfix()
            {
                try
                {
                    BeyondCore.Beyondinstance.fix = false;
                }
                catch (Exception e)
                {
                    MelonLogger.Msg(e);
                }
            }
        }

        [HarmonyPatch(typeof(CameraFunction), "FollowTargetObj")]
        public static class cameraHeight
        {
            public static void Prefix(ref CameraFunction __instance)
            {
                try
                {
                    if(BeyondCore.Beyondinstance.playerVis != null)
                    {
                        CameraFunction._current._mainCamera.farClipPlane = 10000;
                        CameraFunction._current._mainCamera.nearClipPlane = 0.0001f;
                        if (Beyondinstance.playerVis._visualAnimator.GetCurrentAnimatorClipInfo(11).Length > 0)
                        {
                            if(Beyondinstance.playerVis._visualAnimator.GetCurrentAnimatorClipInfo(11)[0].clip.name.Contains("_sit"))
                                __instance.positionAdjust = new Vector3(0, 2.15f, 0) * BeyondCore.Beyondinstance.playerVis._playerAppearanceStruct._heightWeight / 2;
                            else
                                __instance.positionAdjust = new Vector3(0, 2.15f, 0) * BeyondCore.Beyondinstance.playerVis._playerAppearanceStruct._heightWeight;
                        }
                        else
                            __instance.positionAdjust = new Vector3(0, 2.15f, 0) * BeyondCore.Beyondinstance.playerVis._playerAppearanceStruct._heightWeight;
                    }
                    //MelonLogger.Msg(__instance.positionAdjust);
                }
                catch (Exception e)
                {
                    MelonLogger.Msg(e);
                }
            }
        }

        [HarmonyPatch(typeof(CameraFogDensity), "OnPreRender")]
        public static class cameraFog
        {
            public static bool Prefix(ref CameraFogDensity __instance)
            {
                try
                {
                    __instance.fogDensity = 0.0001f;
                    return false;
                }
                catch (Exception e)
                {
                    MelonLogger.Msg(e);
                    return true;
                }
            }
        }

        [HarmonyPatch(typeof(ScriptablePlayerRace), "Init_ParamsCheck")]
        public static class bypass
        {
            private static bool Prefix(ref PlayerAppearance_Profile _aP, ref PlayerAppearance_Profile __result)
            {
                __result = _aP;
                return false;
            }
        }

        [HarmonyPatch(typeof(CharacterSelectManager), "Apply_CharacterSelectDisplay")]
        public static class getProfile
        {
            private static void Postfix()
            {
                try
                {
                    BeyondCore.Beyondinstance.profile = ProfileDataManager._current._characterFile._appearanceProfile;
                    //MelonLogger.Msg(Core.instance.profile._boobWeight);
                }
                catch (Exception e)
                {
                    MelonLogger.Msg(e);
                }
            }
        }

        [HarmonyPatch(typeof(PlayerVisual), "Apply_NetworkedCharacterDisplay")]
        public static class fixapp
        {
            private static void Prefix(ref PlayerVisual __instance)
            {
                try
                {
                    if (!__instance.isLocalPlayer)
                        return;
                    var param = __instance._playerRaceModel._scriptablePlayerRace._raceDisplayParams;
                    BeyondCore.Beyondinstance.playerVis = __instance;
                }
                catch (Exception e)
                {
                    //MelonLogger.Msg(e);
                }
            }
        }

        [HarmonyPatch(typeof(SettingsManager), "Load_SettingsData")]
        public static class fixSettings
        {
            private static void Postfix(ref SettingsManager __instance)
            {
                try
                {
                    __instance._limitProportionsToggle.isOn = false;
                }
                catch (Exception e)
                {
                    //MelonLogger.Msg(e);
                }
            }
        }

        public float camDis;

        [HarmonyPatch(typeof(CameraCollision), "<LateUpdate>g__Handle_DistanceControl|13_0")]
        public static class setMaxCamDis
        {
            private static bool Prefix(ref CameraCollision __instance)
            {
                try
                {
                    float axis = Input.GetAxis("Mouse ScrollWheel");
                    if (axis != 0f && !Player._mainPlayer._bufferingStatus && !Player._mainPlayer._inChat && !Player._mainPlayer._inUI)
                    {
                        __instance.maxDistance += axis * (35.5f + MathF.Abs(BeyondCore.Beyondinstance.camDis));
                    }
                    BeyondCore.Beyondinstance.camDis = __instance.maxDistance;
                    return false;
                }
                catch (Exception e)
                {
                    MelonLogger.Msg(e);
                    return true;
                }
            }
        }

        [HarmonyPatch(typeof(CameraCollision), "Handle_DetectGroundLayer")]
        public static class removeCamCol
        {
            private static void Postfix(ref bool __result)
            {
                CameraCollision camCol = CameraCollision._current;
                camCol.maxDistance = BeyondCore.Beyondinstance.camDis;
                if (!col)
                {
                    __result = false;
                }
            }
        }

        static bool col = false;

        public override void OnUpdate()
        {
            if(Input.GetKeyDown(KeyCode.BackQuote) && Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift))
            {
                col = !col;
                ChatBehaviour._current.New_ChatMessage($"Camera Collision: {col}");
            }
        }

        public override void OnLateInitializeMelon()
        {
            Beyondinstance = this;
            //uncapSizes();
        }

        /*void uncapSizes()
        {
            try
            {
                foreach (PlayerRaceModel go in Resources.FindObjectsOfTypeAll(typeof(PlayerRaceModel)) as PlayerRaceModel[])
                {
                    var param = go._scriptablePlayerRace._raceDisplayParams;
                    maxSizes[0] = new Vector2(param._headWidthRange.x / 2, param._headWidthRange.y * 2);
                    maxSizes[1] = new Vector2(param._headModRange.x / 2, param._headModRange.y * 2);
                    maxSizes[2] = new Vector2(param._heightRange.x / 2, param._heightRange.y * 2);
                    maxSizes[3] = new Vector2(param._widthRange.x / 2, param._widthRange.y * 2);
                    maxSizes[4] = new Vector2(param._torsoRange.x * 2, param._torsoRange.y * 2);
                    maxSizes[5] = new Vector2(param._boobRange.x * 2, param._boobRange.y * 2);
                    maxSizes[6] = new Vector2(param._armRange.x * 2, param._armRange.y * 2);
                    maxSizes[7] = new Vector2(param._bellyRange.x * 2, param._bellyRange.y * 2);
                    maxSizes[8] = new Vector2(param._bottomRange.x * 2, param._bottomRange.y * 2);
                    maxSizes[9] = new Vector2(param._pitchRange.x * 2, param._pitchRange.y * 2);

                    
                    param._headWidthRange = BeyondCore.Beyondinstance.maxSizes[0];
                    param._headModRange = BeyondCore.Beyondinstance.maxSizes[1];
                    param._heightRange = BeyondCore.Beyondinstance.maxSizes[2];
                    param._widthRange = BeyondCore.Beyondinstance.maxSizes[3];
                    param._torsoRange = BeyondCore.Beyondinstance.maxSizes[4];
                    param._boobRange = BeyondCore.Beyondinstance.maxSizes[5];
                    param._armRange = BeyondCore.Beyondinstance.maxSizes[6];
                    param._bellyRange = BeyondCore.Beyondinstance.maxSizes[7];
                    param._bottomRange = BeyondCore.Beyondinstance.maxSizes[8];
                    param._pitchRange = BeyondCore.Beyondinstance.maxSizes[9];
                }
            }
            catch (Exception e)
            {
                MelonLogger.Msg(e);
            }
        }*/
    }
}