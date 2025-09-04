using System.Reflection;
using BepInEx;
using EconomyInfo.tools;
using HarmonyLib;

namespace EconomyInfo
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class EconomyInfo : BaseUnityPlugin
    {
        public const string GUID = "Turbero.EconomyInfo";
        public const string NAME = "Economy Info";
        public const string VERSION = "1.1.0";
        private readonly Harmony harmony = new Harmony(GUID);
        void Awake()
        {
            ConfigurationFile.LoadConfig(this);
            harmony.PatchAll();
        }

        void onDestroy()
        {
            harmony.UnpatchSelf();
        }
    }
}