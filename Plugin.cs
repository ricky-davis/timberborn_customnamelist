using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Timberborn.Beavers;

namespace CustomNameList
{
    [BepInPlugin("com.spyci.timberborn.customnamelist", "Custom Name List", "0.2.0")]
    [BepInProcess("Timberborn.exe")]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        internal static ManualLogSource Log;

        private static string _namesFilePath = $"{Path.GetDirectoryName(Paths.ExecutablePath)}{Path.DirectorySeparatorChar}names.txt";

        private void Awake()
        {
            Log = base.Logger;
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
            Logger.LogInfo($"Plugin com.spyci.timberborn.customnamelist is loaded!");
        }

        static void LoadNewestNames(BeaverNameService __instance, bool initial = false)
        {
            if (!File.Exists(_namesFilePath))
            {
                if (initial)
                {
                    Log.LogError($"Could not find names file at: {_namesFilePath}");
                    Log.LogWarning("Will use standard game names instead.");
                }
                return;
            }

            List<string> nameList = File.ReadAllLines(_namesFilePath).ToList().Select(e => e.Trim().Replace("\r", "")).ToList();
            if (nameList.Count == 0)
            {
                if (initial)
                {
                    Log.LogError("names.txt file empty!");
                    Log.LogWarning("Will use standard game names instead.");
                }
                return;
            }

            if (!Enumerable.SequenceEqual(__instance._completeNamePool, nameList))
            {
                Log.LogInfo($"Loading {nameList.Count} custom names.");
                __instance._completeNamePool.Clear();
                __instance._completeNamePool.AddRange(nameList);
            }

            //Log.LogInfo(string.Join(", ", __instance._completeNamePool));
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(BeaverNameService), "InitializeCompleteNamePool")]
        static void Patch_BeaverNameService_InitializeCompleteNamePool_Postfix(ref BeaverNameService __instance)
        {
            //Log.LogInfo("InitializeCompleteNamePool BeaverNameService");
            LoadNewestNames(__instance, true);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(BeaverNameService), "RandomName")]
        static void Patch_BeaverNameService_RandomName_Postfix(ref BeaverNameService __instance)
        {
            //Log.LogInfo("RandomName BeaverNameService");
            //Log.LogInfo($"{string.Join(", ", __instance._names)}");
            if (__instance._names.Count == 0)
                LoadNewestNames(__instance, false);
        }
    }
}
