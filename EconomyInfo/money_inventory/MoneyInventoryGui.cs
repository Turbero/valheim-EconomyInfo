using System.Reflection;
using EconomyInfo.tools;
using HarmonyLib;
using UnityEngine;
using Logger = EconomyInfo.tools.Logger;

namespace EconomyInfo.money_inventory
{
    [HarmonyPatch(typeof(InventoryGui), "Awake")]
    public class MoneyInventoryGuiPatch {
        
        public static MoneyPanel moneyPanelInventory;
        public static MoneyPanel moneyPanelContainer;
        
        public static void Postfix(InventoryGui __instance)
        {
            Transform inventoryPanelTransform = InventoryGui.instance.m_inventoryRoot.transform.Find("Player");
            Transform containerPanelTransform = InventoryGui.instance.m_inventoryRoot.transform.Find("Container");
            
            moneyPanelInventory = new MoneyPanel(MoneyPanel.MoneyPanelType.Inventory, inventoryPanelTransform);
            moneyPanelInventory.getGameObject().SetActive(ConfigurationFile.showInventoryMoneyBalance.Value);
            moneyPanelContainer = new MoneyPanel(MoneyPanel.MoneyPanelType.Container, containerPanelTransform);
            moneyPanelContainer.getGameObject().SetActive(ConfigurationFile.showContainerMoneyBalance.Value);
        }
    }
    
    [HarmonyPatch(typeof(InventoryGui), "Show")]
    public class InventoryGui_Show_Patch {
        
        public static void Postfix(InventoryGui __instance)
        {
            Logger.Log("Inventory opened!");
            MoneyInventoryRecalculation.RecalculateMoneyInventoryValue();
        }
    }
    
    [HarmonyPatch(typeof(Inventory), "Changed")]
    class Inventory_Changed_Patch
    {
        public static void Postfix(Inventory __instance)
        {
            if (__instance == Player.m_localPlayer?.GetInventory())
            {
                MoneyInventoryRecalculation.RecalculateMoneyInventoryValue();
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
                MoneyInventoryRecalculation.RecalculateCalculateChestValue(__instance);
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
                MoneyInventoryRecalculation.RecalculateCalculateChestValue(__instance);
            }
        }
    }
}