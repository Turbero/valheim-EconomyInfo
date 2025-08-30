using HarmonyLib;
using UnityEngine;

namespace EconomyInfo
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
            moneyPanelInventory.getGameObject().SetActive(true);
            moneyPanelContainer = new MoneyPanel(MoneyPanel.MoneyPanelType.Container, containerPanelTransform);
            moneyPanelContainer.getGameObject().SetActive(true);
        }
    }
}