using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using HarmonyLib;
using System.Reflection;
using System.IO;
using UnityEngine;

namespace CustomNameList
{
    [BepInPlugin("com.thundersen.timberborn.customnamelist", "Custom Name List", "0.1.2")]
    [BepInProcess("Timberborn.exe")]
    public class Plugin : BaseUnityPlugin
    {
        internal static ManualLogSource Log;

        private static string _namesFilePath = $"{Path.GetDirectoryName(Paths.ExecutablePath)}{Path.DirectorySeparatorChar}names.txt";

        internal static CustomNameService NameService = new CustomNameService(_namesFilePath);

        internal static ConfigEntry<int> NameServiceRandomSeed;
        internal static ConfigEntry<int> NameServiceLastIndex;

        private void Awake()
        {
            Log = base.Logger;

            NameServiceRandomSeed = Config.Bind("customnamelist.meta", "NameServiceRandomSeed", 0, "");
            NameServiceLastIndex = Config.Bind("customnamelist.meta", "NameServiceLastIndex", 0, "");

            NameService.Init();

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());

            Logger.LogInfo($"Plugin com.thundersen.timberborn.customnamelist is loaded!");
        }
    }
}
