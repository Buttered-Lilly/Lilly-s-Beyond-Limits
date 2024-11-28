using MelonLoader;
using HarmonyLib;
using UnityEngine;
using System.Collections;
using static MelonLoader.MelonLogger;

[assembly: MelonInfo(typeof(Lilly_s_Beyond_Limits.Core), "Lilly's Beyond Limits", "1.0.0", "Lilly", null)]
[assembly: MelonGame("KisSoft", "ATLYSS")]

namespace Lilly_s_Beyond_Limits
{
    public class Core : MelonMod
    {

        public Vector2[] maxSizes = new Vector2[10];

        static Core instance;
        public PlayerVisual playerVis;
        public PlayerAppearance_Profile profile;
        public PlayerAppearanceStruct playerapp;
        public bool fix = false;

        [HarmonyPatch(typeof(ChatBehaviour), "Cmd_SendChatMessage")]
        public static class chatCommands
        {
            private static bool Prefix(ref ChatBehaviour __instance,  ref string _message)
            {
                try
                {
                    _message = _message.ToLower();
                    MelonLogger.Msg(_message);
                    if (_message.StartsWith("/size"))
                    {
                        PlayerAppearanceStruct playerapp = Core.instance.playerVis._playerAppearanceStruct;
                        PlayerAppearance_Profile _aP = Core.instance.profile;

                        string[] Parts;
                        Parts = _message.Split(" ");
                        //_message = "<>/";
                        float value;
                        float.TryParse(Parts[2], out value);
                        if (Parts[1] == "boobs")
                        {
                            playerapp._boobWeight = value;
                        }
                        else if(Parts[1] == "height")
                        {
                            playerapp._heightWeight = value;
                        }
                        else if(Parts[1] == "head")
                        {
                            playerapp._headWidth = value;
                        }
                        else if(Parts[1] == "width")
                        {
                            playerapp._widthWeight = value;
                        }
                        else if(Parts[1] == "butt")
                        {
                            playerapp._bottomWeight = value;
                        }
                        else if(Parts[1] == "belly")
                        {
                            playerapp._bellyWeight = value;
                        }
                        else if(Parts[1] == "muzzle")
                        {
                            playerapp._muzzleWeight = value;
                        }
                        else if (Parts[1] == "voice")
                        {
                            playerapp._voicePitch = value;
                        }
                        else if (Parts[1] == "torso")
                        {
                            playerapp._torsoWeight = value;
                        }
                        else if (Parts[1] == "arms")
                        {
                            playerapp._armWeight = value;
                        }
                        MelonLogger.Msg(_message);
                        Core.instance.playerVis.Cmd_SendNew_PlayerAppearanceStruct(playerapp);
                        ProfileDataManager._current.Save_ProfileData();
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

        [HarmonyPatch(typeof(GameManager), "Start")]
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
        }

        [HarmonyPatch(typeof(Player), "Handle_ServerConditions")]
        public static class fixPlayer
        {
            public static void Postfix(ref Player __instance)
            {
                try
                {
                    if (Core.instance.fix || __instance.Network_currentPlayerCondition != PlayerCondition.ACTIVE || !__instance.isLocalPlayer)
                        return;
                    else if (__instance.Network_currentPlayerCondition == PlayerCondition.ACTIVE && __instance.isLocalPlayer)
                    {
                        PlayerAppearanceStruct playerapp = Core.instance.playerVis._playerAppearanceStruct;
                        PlayerAppearance_Profile _aP = Core.instance.profile;
                        playerapp._boobWeight = _aP._boobWeight;
                        playerapp._headWidth = _aP._headWidth;
                        playerapp._bottomWeight = _aP._bottomWeight;
                        playerapp._heightWeight = _aP._heightWeight;
                        playerapp._torsoWeight = _aP._torsoWeight;
                        playerapp._muzzleWeight = _aP._muzzleWeight;
                        playerapp._voicePitch = _aP._voicePitch;
                        playerapp._bellyWeight = _aP._bellyWeight;
                        playerapp._armWeight = _aP._armWeight;
                        MelonLogger.Msg(_aP._boobWeight);
                        Core.instance.playerVis.Cmd_SendNew_PlayerAppearanceStruct(playerapp);
                        Core.instance.fix = true;
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
                    Core.instance.fix = false;
                }
                catch (Exception e)
                {
                    MelonLogger.Msg(e);
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
            private static void Postfix(ref CharacterSelectManager __instance)
            {
                try
                {
                    Core.instance.profile = ProfileDataManager._current._characterFile._appearanceProfile;
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
            private static bool Prefix(PlayerVisual __instance)
            {
                try
                {
                    if (!__instance.isLocalPlayer)
                        return true;
                    var param = __instance._playerRaceModel._scriptablePlayerRace._raceDisplayParams;
                    Core.instance.playerVis = __instance;
                    param._headWidthRange = Core.instance.maxSizes[0];
                    param._headModRange = Core.instance.maxSizes[1];
                    param._heightRange = Core.instance.maxSizes[2];
                    param._widthRange = Core.instance.maxSizes[3];
                    param._torsoRange = Core.instance.maxSizes[4];
                    param._boobRange = Core.instance.maxSizes[5];
                    param._armRange = Core.instance.maxSizes[6];
                    param._bellyRange = Core.instance.maxSizes[7];
                    param._bottomRange = Core.instance.maxSizes[8];
                    param._pitchRange = Core.instance.maxSizes[9];
                }
                catch (Exception e)
                {
                    MelonLogger.Msg(e);
                }
                return true;
            }
        }

        public override void OnLateInitializeMelon()
        {
            instance = this;
            uncapSizes();
        }

        void uncapSizes()
        {
            try
            {
                foreach (PlayerRaceModel go in Resources.FindObjectsOfTypeAll(typeof(PlayerRaceModel)) as PlayerRaceModel[])
                {
                    var param = go._scriptablePlayerRace._raceDisplayParams;
                    maxSizes[0] = new Vector2(param._headWidthRange.x, param._headWidthRange.y * 5);
                    maxSizes[1] = new Vector2(param._headModRange.x, param._headModRange.y * 5);
                    maxSizes[2] = new Vector2(param._heightRange.x, param._heightRange.y * 5);
                    maxSizes[3] = new Vector2(param._widthRange.x, param._widthRange.y * 5);
                    maxSizes[4] = new Vector2(param._torsoRange.x, param._torsoRange.y * 5);
                    maxSizes[5] = new Vector2(param._boobRange.x, param._boobRange.y * 5);
                    maxSizes[6] = new Vector2(param._armRange.x, param._armRange.y * 5);
                    maxSizes[7] = new Vector2(param._bellyRange.x, param._bellyRange.y * 5);
                    maxSizes[8] = new Vector2(param._bottomRange.x, param._bottomRange.y * 5);
                    maxSizes[9] = new Vector2(param._pitchRange.x * 2, param._pitchRange.y * 2);


                    param._headWidthRange.y = param._headWidthRange.y * 5;
                    param._headModRange.y = param._headModRange.y * 5;
                    param._heightRange.y = param._heightRange.y * 5;
                    param._widthRange.y = param._widthRange.y * 5;
                    param._torsoRange.y = param._torsoRange.y * 5;
                    param._boobRange.y = param._boobRange.y * 5;
                    param._armRange.y = param._armRange.y * 5;
                    param._bellyRange.y = param._bellyRange.y * 5;
                    param._bottomRange.y = param._bottomRange.y * 5;
                    param._pitchRange.y = param._pitchRange.y * 2;
                    param._pitchRange.x = param._pitchRange.x * 2;
                }
            }
            catch (Exception e)
            {
                MelonLogger.Msg(e);
            }
        }
    }
}