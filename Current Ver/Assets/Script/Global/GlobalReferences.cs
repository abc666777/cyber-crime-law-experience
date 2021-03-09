using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalReferences
{
    public static class Scene
    {
        public const string StartScene = "START";
        public const string ArchiveScene = "ARCHIVE";
        public const string GameplayScene = "GAMEPLAY";
        public const string EndingScene = "ENDING";
        public const string CreditScene = "CREDIT";
    }

    public static class Panel
    {
        public static string NewGamePanel = "NewGamePanel";
        public static string LoadGamePanel = "LoadGamePanel";
        public static string SettingPanel = "SettingPanel";
        public const string SaveGamePanel = "SaveGamePanel";
    }

    public static class Path
    {
        public static string ArchivePath = "Data/Archive";
        public const string CheckpointPath = "Data/Checkpoint";
        public static string PrefabPanelsPath = "Prefabs/Panel";
        public static string PrefabCharacterPath = "Prefabs/Character";
        public static string SpriteCharacterPath = "Image/Character/";
        public const string BackgroundPath = "Image/Background";
        public const string TransitonEffectPath = "Image/TransitionEffects";
        public const string SFXPath = "Audio/SFX/";
        public const string BGMPath = "Audio/BGM/";
        public const string StoryPath = "Story/";
        public static string SavePath = Application.persistentDataPath + "/";
    }

    public static class Data
    {
        public static string ArchiveData = "/Data";
        public const string CheckPointData = "/Checkpoint";
    }

    public static class CharacterName
    {
        public const string Gabi = "กาบิ";
        public const string Kanao = "คานาโอะ";
        public const string Bill = "บิล";
        public const string BlackGuy = "บุคคลปริศนา";
        public const string MissSunday = "มิส ซันเดย์";
        public const string Nompang = "หนมปัง";
        public const string Haru = "ฮารุ";
        public const string Betty = "เบตตี้";
        public const string Botak = "โบทัก";
        public const string Daiji = "ไดจิ";
        public const string Police = "ตำรวจ";
        public const string Badguy = "นักเลง";
        public const string Auntie = "คุณป้า";
    }

    public static class CharacterExpression
    {
        public static string Default = "normal";
        public static class Female
        {
            public static string Normal = "normal";
            public static string Angry = "angry";
            public static string Delighted = "delighted";
            public static string Laugh = "laugh";
            public static string Sad = "sad";
            public static string Shocked = "shocked";
            public static string Smile = "smile";
            public static string Smile2 = "smile2";
            public static string Smug = "smug";
        }

        public static class Male
        {
            public static string Normal = "normal";
            public static string Angry = "angry";
            public static string Angry2 = "angry2";
            public static string Laugh = "laugh";
            public static string Sad = "sad";
            public static string Shocked = "shocked";
            public static string Smile = "smile";
            public static string Smile2 = "smile2";
            public static string Smile3 = "smile3";
            public static string Smug = "smug";
        }
    }
}
