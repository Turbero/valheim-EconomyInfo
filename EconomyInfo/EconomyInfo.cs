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
        public const string VERSION = "1.0.0";

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
    
    [HarmonyPatch(typeof(InventoryGui), "Show")]
    public class InventoryGui_Show_Patch {
        
        public static void Postfix(InventoryGui __instance)
        {
            Logger.Log("Inventory opened!");
            Recalculation.RecalculateMoneyInventoryValue();
        }
    }
    
    [HarmonyPatch(typeof(Inventory), "Changed")]
    class Inventory_Changed_Patch
    {
        public static void Postfix(Inventory __instance)
        {
            if (__instance == Player.m_localPlayer?.GetInventory())
            {
                Recalculation.RecalculateMoneyInventoryValue();
            }
        }
    }

    [HarmonyPatch(typeof(Container), "Interact")]
    public class Container_Interact_Patch
    {
        public static void Postfix(Container __instance, Humanoid character, bool hold, bool alt, bool __result)
        {
            if (__instance != null)
            {
                Logger.Log($"Chest opened in {__instance.transform.position}!");
                Recalculation.RecalculateCalculateChestValue(__instance);
            }
        }
    }

    [HarmonyPatch]
    public class Container_Changed_patch
    {
        static MethodBase TargetMethod()
        {
            return AccessTools.Method(typeof(Container), "OnContainerChanged");
        }

        public static void Postfix(ref Container __instance)
        {
            if (__instance != null)
            {
                Logger.Log($"Chest opened in {__instance.transform.position}!");
                Recalculation.RecalculateCalculateChestValue(__instance);
            }
        }
    }
}
