using HarmonyLib;
using UnityEngine;
using Logger = EconomyInfo.tools.Logger;

namespace EconomyInfo.money_vendor
{
    [HarmonyPatch(typeof(StoreGui), "Show")]
    public class MoneyStoreGuiShowPatch {
        
        private static VendorPanelValuable rubyPanel;
        private static VendorPanelValuable amberPanel;
        private static VendorPanelValuable pearlPanel;
        private static VendorPanelValuable silverNecklacePanel;
        
        private static bool resized = false;
        
        public static void Postfix(StoreGui __instance, Trader trader)
        {
            if (!resized)
            {
                resize();
                resized = true;
            }

            updateValuables();
        }

        private static void resize()
        {
            Transform storeTransform = GameObject.Find("Store").transform;
            storeTransform.Find("border (1)").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -90);
            storeTransform.Find("border (1)").GetComponent<RectTransform>().sizeDelta = new Vector2(40, 220);

            amberPanel = new VendorPanelValuable(storeTransform, "amberPanel", "amber", new Vector2(0, -15), new Vector2(20, 20), new Vector2(42, 42));
            pearlPanel = new VendorPanelValuable(storeTransform, "pearlPanel", "AmberPearl", new Vector2(0, -60), new Vector2(8, 32));
            rubyPanel = new VendorPanelValuable(storeTransform, "rubyPanel", "ruby", new Vector2(0, -105), new Vector2(20, 20), new Vector2(42, 42));
            silverNecklacePanel = new VendorPanelValuable(storeTransform, "silverNecklacePanel", "silvernecklace", new Vector2(0, -150), new Vector2(18, 20), new Vector2(46, 46));
        }

        public static void updateValuables()
        {
            int totalAmber = 0;
            int totalAmberPearl = 0;
            int totalRuby = 0;
            int totalSilverNecklace = 0;
            
            int totalAmountAmber = 0;
            int totalAmountAmberPearl = 0;
            int totalAmountRuby = 0;
            int totalAmountSilverNecklace = 0;

            if (Player.m_localPlayer != null)
            {
                foreach (var item in Player.m_localPlayer.GetInventory().GetAllItems())
                {
                    if (item.m_shared.m_value > 0)
                    {
                        Logger.Log("Found in player inventory: " + item.m_shared.m_name + " = " + item.m_shared.m_value);
                        if (item.m_shared.m_name.ToLower().Contains("amber"))
                        {
                            totalAmber += item.m_stack * item.m_shared.m_value;
                            totalAmountAmber++;
                        }
                        else if (item.m_shared.m_name.ToLower().Contains("AmberPearl"))
                        {
                            totalAmberPearl += item.m_stack * item.m_shared.m_value;
                            totalAmountAmberPearl++;
                        }
                        else if (item.m_shared.m_name.ToLower().Contains("ruby"))
                        {
                            totalRuby += item.m_stack * item.m_shared.m_value;
                            totalAmountRuby++;
                        }
                        else if (item.m_shared.m_name.ToLower().Contains("silvernecklace"))
                        {
                            totalSilverNecklace += item.m_stack * item.m_shared.m_value;
                            totalAmountSilverNecklace++;
                        }
                    }
                }
            }
            
            amberPanel.updateValue(totalAmountAmber, totalAmber);
            pearlPanel.updateValue(totalAmountAmberPearl, totalAmberPearl);
            rubyPanel.updateValue(totalAmountRuby, totalRuby);
            silverNecklacePanel.updateValue(totalAmountSilverNecklace, totalSilverNecklace);
        }
    }

    [HarmonyPatch(typeof(StoreGui), "OnSellItem")]
    public class MoneyStoreGuiOnSellItemPatch
    {

        public static void Postfix(StoreGui __instance)
        {
            Logger.Log("Item sold. Recalculating...");
            MoneyStoreGuiShowPatch.updateValuables();
        }
    }
    
    [HarmonyPatch(typeof(Inventory), "Changed")]
    class Inventory_Changed_StoreGui_Patch
    {
        public static void Postfix(Inventory __instance)
        {
            if (__instance == Player.m_localPlayer?.GetInventory())
            {
                // If trader is opened, update
                if (GameObject.Find("Store") != null)
                {
                    Logger.Log("Inventory changed while trader opened. Recalculating...");
                    MoneyStoreGuiShowPatch.updateValuables();
                }
            }
        }
    }
}