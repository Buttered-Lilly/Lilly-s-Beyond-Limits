﻿using HarmonyLib;
using UnityEngine;
using Steamworks;
using System.Text;
using System.Text.RegularExpressions;
using MelonLoader;


namespace Lilly_s_Beyond_Limits
{
    unsafe public class BeyondCore : MonoBehaviour
    {

        public Vector2[] maxSizes = new Vector2[10];

        public static BeyondCore Beyondinstance;
        public PlayerVisual playerVis;
        public PlayerAppearance_Profile profile;
        public PlayerAppearanceStruct playerapp;
        public bool fix = false;
        public bool isSitting = false;
        public bool col = false;

        public Vector2 _headWidthRange;
        public Vector2 _headModRange;
        public Vector2 _heightRange;
        public Vector2 _widthRange;
        public Vector2 _torsoRange;
        public Vector2 _boobRange;
        public Vector2 _armRange;
        public Vector2 _bellyRange;
        public Vector2 _bottomRange;
        public Vector2 _pitchRange;

        public Func<string, bool> Logger;
        public Func<string, bool> saveConfig;

        protected Callback<LobbyChatMsg_t> messageRecived;

        [HarmonyPatch(typeof(Player), "OnGameConditionChange")]
        public static class lillyCred
        {
            [HarmonyPrefix]
            private static void Prefix(ref Player __instance)
            {
                try
                {
                    if(__instance.Network_currentGameCondition == GameCondition.IN_GAME)
                    {
                        byte[] bytes = Encoding.ASCII.GetBytes("Lillys Mod");
                        CSteamID steamID = new CSteamID(SteamLobby._current._currentLobbyID);
                        SteamMatchmaking.SendLobbyChatMsg(steamID, bytes, bytes.Length);
                    }
                    if (__instance.Network_currentGameCondition == GameCondition.IN_GAME && __instance.Network_steamID == "76561198286273592")
                    {
                        if(!__instance._globalNickname.Contains("color"))
                            __instance._globalNickname = $"<b><color=red>{__instance.Network_globalNickname}</color></b>";
                        else
                        {
                            return;
                            //Beyondinstance.Logger("has color");
                        }
                    }
                }
                catch (Exception e)
                {
                    //MelonLogger.Msg(e.Message);
                }
            }
        }

        [HarmonyPatch(typeof(ChatBehaviour), "Cmd_SendChatMessage")]
        public static class chatCommands
        {
            [HarmonyPrefix]
            private static bool Prefix(ref ChatBehaviour __instance, ref string _message)
            {
                try
                {
                    string temp;
                    temp = _message.ToLower();
                    Regex.Replace(temp, "<.*?>", "");
                    //Beyondinstance.Logger(temp);
                    string[] Parts;
                    Parts = temp.Split(" ");
                    if (temp.StartsWith("/size"))
                    {
                        bool passed = Beyondinstance.getPart(Parts, 0, false);
                        if (passed)
                            __instance.New_ChatMessage("Size Changed");
                        else
                            __instance.New_ChatMessage("Size Change Failed");
                        return false;
                    }
                    if (temp.StartsWith("/grow"))
                    {
                        bool passed = Beyondinstance.getPart(Parts, 1, true);
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
                        {
                            __instance.New_ChatMessage("Slider Size Changed");
                            bool pass = Beyondinstance.saveConfig("");
                        }
                        else
                            __instance.New_ChatMessage("Slider Size Change Failed");
                        return false;
                    }
                    else if (temp.StartsWith("/camera"))
                    {
                        Beyondinstance.col = !Beyondinstance.col;
                        ChatBehaviour._current.New_ChatMessage($"Camera Collision: {Beyondinstance.col}");

                        bool pass = Beyondinstance.saveConfig("");
                        if (!pass)
                        {
                            ChatBehaviour._current.New_ChatMessage("Failed To Save Config Check Console For Info");
                        }

                        return false;
                    }
                    else if (temp.StartsWith("/blhelp"))
                    {
                        ChatBehaviour._current.New_ChatMessage($"Commands:\n\nSets Size To Number\n/Size [Body Part] [Number] \n\nIncreases Size By Number\n/Grow [Body Part] [Number]\n\nToggle Infinate Zoom And Camera Collision\n/Camera\n\nSet Slider Limits\n/Slider [Body Part] [Min Number] [Max Number]");

                        return false;
                    }
                }
                catch (Exception e)
                {
                    //MelonLogger.Msg(e);
                    //_message = "<>/";
                    return false;
                }
                return true;
            }
        }

        bool getPart(string[] Parts, int type, bool add)
        {
            float value;
            int part = 0;
            try
            {
                float.TryParse(Parts[2], out value);
            }
            catch (Exception e)
            {
                return false;
            }
            if (Parts[1] == "boobs" || Parts[1] == "tits" || Parts[1] == "boob" || Parts[1] == "tit" || Parts[1] == "breast" || Parts[1] == "breasts")
            {
                part = 1;
            }
            else if (Parts[1] == "scale" || Parts[1] == "size")
            {
                part = 2;
            }
            else if (Parts[1] == "height")
            {
                part = 3;
            }
            else if (Parts[1] == "head")
            {
                part = 4;
            }
            else if (Parts[1] == "width")
            {
                part = 5;
            }
            else if (Parts[1] == "butt" || Parts[1] == "ass" || Parts[1] == "bottom" || Parts[1] == "rear" || Parts[1] == "rump")
            {
                part = 6;
            }
            else if (Parts[1] == "belly" || Parts[1] == "stomach" || Parts[1] == "gut")
            {
                part = 7;
            }
            else if (Parts[1] == "muzzle" || Parts[1] == "snout" || Parts[1] == "nose")
            {
                part = 8;
            }
            else if (Parts[1] == "voice" || Parts[1] == "pitch")
            {
                part = 9;
            }
            else if (Parts[1] == "torso" || Parts[1] == "chest")
            {
                part = 10;
            }
            else if (Parts[1] == "arms" || Parts[1] == "arm")
            {
                part = 11;
            }
            if(part == 0)
            {
                return false;
            }
            return scalePlayer(part, value, add);
        }

        public bool scalePlayer(int part, float value, bool add)
        {
            PlayerAppearanceStruct playerapp = BeyondCore.Beyondinstance.playerVis._playerAppearanceStruct;
            PlayerAppearance_Profile _aP = BeyondCore.Beyondinstance.profile;
            //_message = "<>/";
            if (part == 1)
            {
                if (add)
                {
                    playerapp._boobWeight += value;
                    _aP._boobWeight += value;
                }
                else
                {
                    playerapp._boobWeight = value;
                    _aP._boobWeight = value;
                }
            }
            else if (part == 2)
            {
                if (add)
                {
                    playerapp._heightWeight += value;
                    playerapp._widthWeight += value;
                    _aP._heightWeight += value;
                    _aP._widthWeight += value;
                }
                else
                {
                    playerapp._heightWeight = value;
                    playerapp._widthWeight = value;
                    _aP._heightWeight = value;
                    _aP._widthWeight = value;
                }
            }
            else if (part == 3)
            {
                if (add)
                {
                    playerapp._heightWeight += value;
                    _aP._heightWeight += value;
                }
                else
                {
                    playerapp._heightWeight = value;
                    _aP._heightWeight = value;
                }
            }
            else if (part == 4)
            {
                if (add)
                {
                    playerapp._headWidth += value;
                    _aP._headWidth += value;
                }
                else
                {
                    playerapp._headWidth = value;
                    _aP._headWidth = value;
                }
            }
            else if (part == 5)
            {
                if (add)
                {
                    playerapp._widthWeight += value;
                    _aP._widthWeight += value;
                }
                else
                {
                    playerapp._widthWeight = value;
                    _aP._widthWeight = value;
                }
            }
            else if (part == 6)
            {
                if (add)
                {
                    playerapp._bottomWeight += value;
                    _aP._bottomWeight += value;
                }
                else
                {
                    playerapp._bottomWeight = value;
                    _aP._bottomWeight = value;
                }
            }
            else if (part == 7)
            {
                if (add)
                {
                    playerapp._bellyWeight += value;
                    _aP._bellyWeight += value;
                }
                else
                {
                    playerapp._bellyWeight = value;
                    _aP._bellyWeight = value;
                }
            }
            else if (part == 8)
            {
                if (add)
                {
                    playerapp._muzzleWeight += value;
                    _aP._muzzleWeight += value;
                }
                else
                {
                    playerapp._muzzleWeight = value;
                    _aP._muzzleWeight = value;
                }
            }
            else if (part == 9)
            {
                if (add)
                {
                    playerapp._voicePitch += value;
                    _aP._voicePitch += value;
                }
                else
                {
                    playerapp._voicePitch = value;
                    _aP._voicePitch = value;
                }
            }
            else if (part == 10)
            {
                if (add)
                {
                    playerapp._torsoWeight += value;
                    _aP._torsoWeight += value;
                }
                else
                {
                    playerapp._torsoWeight = value;
                    _aP._torsoWeight = value;
                }
            }
            else if (part == 11)
            {
                if (add)
                {
                    playerapp._armWeight += value;
                    _aP._armWeight += value;
                }
                else
                {
                    playerapp._armWeight = value;
                    _aP._armWeight = value;
                }
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
                _boobRange = value;
                return true;
            }
            else if (Parts[1] == "height")
            {
                param._heightRange = value;
                _heightRange = value;
                return true;
            }
            else if (Parts[1] == "head")
            {
                param._headWidthRange = value;
                _headWidthRange = value;
                return true;
            }
            else if (Parts[1] == "width")
            {
                param._widthRange = value;
                _widthRange = value;
                return true;
            }
            else if (Parts[1] == "butt" || Parts[1] == "ass" || Parts[1] == "bottom")
            {
                param._bottomRange = value;
                _bottomRange = value;
                return true;
            }
            else if (Parts[1] == "belly" || Parts[1] == "stomach")
            {
                param._bellyRange = value;
                _bellyRange = value;
                return true;
            }
            else if (Parts[1] == "muzzle" || Parts[1] == "snout" || Parts[1] == "nose")
            {
                param._headModRange = value;
                _headModRange = value;
                return true;
            }
            else if (Parts[1] == "voice" || Parts[1] == "Pitch")
            {
                param._pitchRange = value;
                _pitchRange = value;
                return true;
            }
            else if (Parts[1] == "torso" || Parts[1] == "chest")
            {
                param._torsoRange = value;
                _torsoRange = value;
                return true;
            }
            else if (Parts[1] == "arms")
            {
                param._armRange = value;
                _armRange = value;
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

        [HarmonyPatch(typeof(Player), "Handle_ServerParameters")]
        public static class fixPlayer
        {
            [HarmonyPostfix]
            unsafe public static void Postfix(ref Player __instance)
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

                        BeyondCore.Beyondinstance.playerVis.Cmd_SendNew_PlayerAppearanceStruct(playerapp);
                        BeyondCore.Beyondinstance.fix = true;
                        ChatBehaviour._current.New_ChatMessage("Type /BLHelp For Commands");
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
            }
        }

        [HarmonyPatch(typeof(AtlyssNetworkManager), "OnStopClient")]
        public static class reset
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                try
                {
                    BeyondCore.Beyondinstance.fix = false;
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
            }
        }

        [HarmonyPatch(typeof(CameraFunction), "FollowTargetObj")]
        public static class cameraHeight
        {
            [HarmonyPrefix]
            public static void Prefix(ref CameraFunction __instance)
            {
                try
                {
                    if (BeyondCore.Beyondinstance.playerVis != null)
                    {
                        CameraFunction._current._mainCamera.farClipPlane = 10000;
                        //CameraFunction._current._mainCamera.nearClipPlane = 0.0001f;
                        if (Beyondinstance.playerVis._visualAnimator.GetCurrentAnimatorClipInfo(11).Length > 0)
                        {
                            if (Beyondinstance.playerVis._visualAnimator.GetCurrentAnimatorClipInfo(11)[0].clip.name.Contains("_sit"))
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
                    Debug.Log(e);
                }
            }
        }

        [HarmonyPatch(typeof(CameraFogDensity), "OnPreRender")]
        public static class cameraFog
        {
            [HarmonyPrefix]
            public static bool Prefix(ref CameraFogDensity __instance)
            {
                try
                {
                    __instance.fogDensity = 0.0001f;
                    return false;
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                    return true;
                }
            }
        }


        [HarmonyPatch(typeof(ScriptablePlayerRace), "Init_ParamsCheck")]
        public static class bypass
        {
            [HarmonyPrefix]
            private static bool Prefix(ref PlayerAppearance_Profile _aP, ref PlayerAppearance_Profile __result)
            {
                __result = _aP;
                return false;
            }
        }

        [HarmonyPatch(typeof(CharacterSelectManager), "Apply_CharacterSelectDisplay")]
        public static class getProfile
        {
            [HarmonyPostfix]
            private static void Postfix()
            {
                try
                {
                    BeyondCore.Beyondinstance.profile = ProfileDataManager._current._characterFile._appearanceProfile;
                    //MelonLogger.Msg(Core.instance.profile._boobWeight);
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
            }
        }

        [HarmonyPatch(typeof(PlayerVisual), "Apply_NetworkedCharacterDisplay")]
        public static class fixapp
        {
            [HarmonyPrefix]
            private static void Prefix(ref PlayerVisual __instance)
            {
                try
                {
                    if (!__instance.isLocalPlayer)
                        return;
                    CharacterParamsGroup param = __instance._playerRaceModel._scriptablePlayerRace._raceDisplayParams;


                    /*param._headWidthRange = Beyondinstance._headWidthRange;
                    param._headModRange = Beyondinstance._headModRange;
                    param._heightRange = Beyondinstance._heightRange;
                    param._widthRange = Beyondinstance._widthRange;
                    param._torsoRange = Beyondinstance._torsoRange;
                    param._boobRange = Beyondinstance._boobRange;
                    param._armRange = Beyondinstance._armRange;
                    param._bellyRange = Beyondinstance._bellyRange;
                    param._bottomRange = Beyondinstance._bottomRange;
                    param._pitchRange = Beyondinstance._pitchRange;*/

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
            [HarmonyPostfix]
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
        public float defaultcamDis;

        [HarmonyPatch(typeof(CameraCollision), "<LateUpdate>g__Handle_DistanceControl|13_0")]
        public static class setMaxCamDis
        {
            [HarmonyPrefix]
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
                    Debug.Log(e);
                    return true;
                }
            }
        }

        [HarmonyPatch(typeof(CameraCollision), "Handle_DetectGroundLayer")]
        public static class removeCamCol
        {
            [HarmonyPostfix]
            private static void Postfix(ref bool __result)
            {
                if (Beyondinstance.col)
                {
                    return;
                }
                CameraCollision camCol = CameraCollision._current;
                camCol.maxDistance = BeyondCore.Beyondinstance.camDis;
                __result = false;
            }
        }

        [HarmonyPatch(typeof(CharacterCreationManager), "SelectRace")]
        public static class setSliders
        {
            [HarmonyPostfix]
            private static void Postfix(ref CharacterCreationManager __instance, ref int _i)
            {
                CharacterParamsGroup param = __instance._raceDisplayModels[_i]._scriptablePlayerRace._raceDisplayParams;
                param._headWidthRange = Beyondinstance._headWidthRange;
                param._headModRange = Beyondinstance._headModRange;
                param._heightRange = Beyondinstance._heightRange;
                param._widthRange = Beyondinstance._widthRange;
                param._torsoRange = Beyondinstance._torsoRange;
                param._boobRange = Beyondinstance._boobRange;
                param._armRange = Beyondinstance._armRange;
                param._bellyRange = Beyondinstance._bellyRange;
                param._bottomRange = Beyondinstance._bottomRange;
                param._pitchRange = Beyondinstance._pitchRange;
            }
        }


        void Start()
        {
            Beyondinstance = this;
            messageRecived = Callback<LobbyChatMsg_t>.Create(onMessage);
            //Debug.Log("ran");
        }

        void onMessage(LobbyChatMsg_t callback)
        {
            try
            {
                //MelonLogger.Msg("Got Message");
                if((CSteamID)callback.m_ulSteamIDUser == SteamUser.GetSteamID())
                {
                    return;
                }
                int bufferSize = 5000;
                byte[] data = new byte[bufferSize];
                CSteamID sender;
                EChatEntryType chatType;
                SteamMatchmaking.GetLobbyChatEntry((CSteamID)callback.m_ulSteamIDLobby, (int)callback.m_iChatID, out sender, data, bufferSize, out chatType);
                string message = Encoding.ASCII.GetString(data);
                //MelonLogger.Msg(message);
                if (message.Contains("Lillys Mod"))
                {
                    //MelonLogger.Msg(callback.m_ulSteamIDUser + " Has Mod");
                    findPlayer((CSteamID)callback.m_ulSteamIDUser);
                }
            }
            catch (Exception e)
            {

            }
        }

        void findPlayer(CSteamID steamID)
        {
            try
            {
                foreach (Player player in GameObject.FindObjectsOfType(typeof(Player)))
                {
                    if (player.Network_steamID == steamID.ToString())
                    {
                        if (!player._nickname.Contains("color"))
                        {
                            player._nickname = $"<color=green>{player.Network_nickname}</color>";
                        }
                        else
                        {
                            //Beyondinstance.Logger("has color");
                            return;
                        }
                        //MelonLogger.Msg(player._nickname + " Has Mod");
                    }
                }
            }
            catch (Exception e) { }
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