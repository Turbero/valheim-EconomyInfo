using System.Reflection;
using EconomyInfo.tools;
using HarmonyLib;
using TMPro;
using UnityEngine;
using Logger = EconomyInfo.tools.Logger;

namespace EconomyInfo.money_vendor
{
    public static class TraderChecks {
        public static bool HasStoreGuiValidTrader()
        {
            Trader trader = (Trader) typeof(StoreGui).GetField("m_trader", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(StoreGui.instance);
            if (trader == null)
                return false;
            
            Logger.Log("Trader.m_name: "+trader.m_name);
            return true;
        }
    }
    
    [HarmonyPatch(typeof(StoreGui), "Show")]
    public class MoneyStoreGuiShowPatch {
        
        private static VendorPanelValuable rubyPanel;
        private static VendorPanelValuable amberPanel;
        private static VendorPanelValuable pearlPanel;
        private static VendorPanelValuable silverNecklacePanel;
        private static VendorPanelValuable draumyxPanel;
        private static VendorPanelValuable grimvarnPanel;
        private static VendorPanelValuable solrythPanel;
        private static VendorPanelValuable veydrisPanel;

        private static bool panelsCreated = false;

        public static void enable(bool enable)
        {
            if (enable)
            {
                resize();
                updateValuables();
            }
            else
            {
                Transform storeTransform = GameObject.Find("Store").transform;
                storeTransform.Find("border (1)").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                storeTransform.Find("border (1)").GetComponent<RectTransform>().sizeDelta = new Vector2(40, 40);
                storeTransform.Find("SellPanel").GetComponent<RectTransform>().anchoredPosition = new Vector2(350, -16);
                enableValuablePanels(false);
            }
            updateCoinsColor();
        }

        public static void Postfix(StoreGui __instance, Trader trader)
        {
            if (trader == null)
                return;

            Logger.Log("Trader.m_name: "+trader.m_name);
            
            createPanels();
            enable(ConfigurationFile.advancedVendorMoneyPanel.Value);
        }

        private static void resize()
        {
            Transform storeTransform = GameObject.Find("Store")?.transform;
            if (storeTransform != null)
            {
                storeTransform.Find("border (1)").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -90);
                storeTransform.Find("border (1)").GetComponent<RectTransform>().sizeDelta = new Vector2(140, 220);
                storeTransform.Find("SellPanel").GetComponent<RectTransform>().anchoredPosition = new Vector2(400, 20);
                enableValuablePanels(true);
            }
        }

        private static void createPanels()
        {
            bool configActive = ConfigurationFile.advancedVendorMoneyPanel.Value;
            if (!configActive)
                return;

            if (panelsCreated && amberPanel != null) return;
            
            Transform storeTransform = GameObject.Find("Store").transform;
            amberPanel = new VendorPanelValuable(storeTransform, "amberPanel", "amber", configActive, new Vector2(-75, -15), new Vector2(20, 20), new Vector2(42, 42));
            pearlPanel = new VendorPanelValuable(storeTransform, "amberpearlPanel", "AmberPearl", configActive, new Vector2(-75, -60), new Vector2(8, 32));
            rubyPanel = new VendorPanelValuable(storeTransform, "rubyPanel", "ruby", configActive, new Vector2(-75, -105), new Vector2(20, 20), new Vector2(42, 42));
            silverNecklacePanel = new VendorPanelValuable(storeTransform, "silverNecklacePanel", "silvernecklace", configActive, new Vector2(-75, -150), new Vector2(18, 20), new Vector2(46, 46));
                
            draumyxPanel = new VendorPanelValuable(storeTransform, "draumyxPanel", "ancientgemstone_black", configActive, new Vector2(110, -15), new Vector2(20, 20), new Vector2(42, 42));
            grimvarnPanel = new VendorPanelValuable(storeTransform, "grimvarnPanel", "ancientgemstone_green", configActive, new Vector2(110, -60), new Vector2(8, 32));
            solrythPanel = new VendorPanelValuable(storeTransform, "solrythPanel", "ancientgemstone_orange", configActive, new Vector2(110, -105), new Vector2(20, 20), new Vector2(42, 42));
            veydrisPanel = new VendorPanelValuable(storeTransform, "veydrisPanel", "ancientgemstone_purple", configActive, new Vector2(110, -150), new Vector2(18, 20), new Vector2(46, 46));
                
            panelsCreated = true;
        }

        private static void enableValuablePanels(bool enable)
        {
            createPanels();
            if (!panelsCreated) return;
            
            amberPanel.getMainPanel().SetActive(enable);
            pearlPanel.getMainPanel().SetActive(enable);
            rubyPanel.getMainPanel().SetActive(enable);
            silverNecklacePanel.getMainPanel().SetActive(enable);
            draumyxPanel.getMainPanel().SetActive(enable);
            grimvarnPanel.getMainPanel().SetActive(enable);
            solrythPanel.getMainPanel().SetActive(enable);
            veydrisPanel.getMainPanel().SetActive(enable);
        }

        public static void updateValuables()
        {
            int totalAmber = 0;
            int totalAmberPearl = 0;
            int totalRuby = 0;
            int totalSilverNecklace = 0;
            int totalDraumyx = 0;
            int totalGrimvarn = 0;
            int totalSolryth = 0;
            int totalVeydris = 0;


            int totalAmountAmber = 0;
            int totalAmountAmberPearl = 0;
            int totalAmountRuby = 0;
            int totalAmountSilverNecklace = 0;
            int totalAmountDraumyx = 0;
            int totalAmountGrimvarn = 0;
            int totalAmountSolryth = 0;
            int totalAmountVeydris = 0;

            if (Player.m_localPlayer != null)
            {
                foreach (var item in Player.m_localPlayer.GetInventory().GetAllItems())
                {
                    if (item.m_shared.m_value > 0)
                    {
                        Logger.Log("Found in player inventory: " + item.m_shared.m_name + " = " + item.m_shared.m_value);
                        if (item.m_shared.m_name.ToLower().Contains("amberpearl"))
                        {
                            totalAmberPearl += item.m_stack * item.m_shared.m_value;
                            totalAmountAmberPearl += item.m_stack;
                        }
                        else if (item.m_shared.m_name.ToLower().Contains("amber"))
                        {
                            totalAmber += item.m_stack * item.m_shared.m_value;
                            totalAmountAmber += item.m_stack;
                        }
                        else if (item.m_shared.m_name.ToLower().Contains("ruby"))
                        {
                            totalRuby += item.m_stack * item.m_shared.m_value;
                            totalAmountRuby += item.m_stack;
                        }
                        else if (item.m_shared.m_name.ToLower().Contains("silvernecklace"))
                        {
                            totalSilverNecklace += item.m_stack * item.m_shared.m_value;
                            totalAmountSilverNecklace += item.m_stack;
                        }
                        else if (item.m_shared.m_name.ToLower().Contains("ancientgemstone_black"))
                        {
                            totalDraumyx += item.m_stack * item.m_shared.m_value;
                            totalAmountDraumyx += item.m_stack;
                        }
                        else if (item.m_shared.m_name.ToLower().Contains("ancientgemstone_green"))
                        {
                            totalGrimvarn += item.m_stack * item.m_shared.m_value;
                            totalAmountGrimvarn += item.m_stack;
                        }
                        else if (item.m_shared.m_name.ToLower().Contains("ancientgemstone_orange"))
                        {
                            totalSolryth += item.m_stack * item.m_shared.m_value;
                            totalAmountSolryth += item.m_stack;
                        }
                        else if (item.m_shared.m_name.ToLower().Contains("ancientgemstone_purple"))
                        {
                            totalVeydris += item.m_stack * item.m_shared.m_value;
                            totalAmountVeydris += item.m_stack;
                        }
                    }
                }
            }
            
            amberPanel.updateValue(totalAmountAmber, totalAmber);
            pearlPanel.updateValue(totalAmountAmberPearl, totalAmberPearl);
            rubyPanel.updateValue(totalAmountRuby, totalRuby);
            silverNecklacePanel.updateValue(totalAmountSilverNecklace, totalSilverNecklace);
            draumyxPanel.updateValue(totalAmountDraumyx, totalDraumyx);
            grimvarnPanel.updateValue(totalAmountGrimvarn, totalGrimvarn);
            solrythPanel.updateValue(totalAmountSolryth, totalSolryth);
            veydrisPanel.updateValue(totalAmountVeydris, totalVeydris);
            
            //Update coins color
            updateCoinsColor();
        }

        public static void updateCoinsColor()
        {
            Transform coinsValueTransform = GameObject.Find("Store").transform.Find("coins").transform.Find("coins");
            TextMeshProUGUI coinsValueText = coinsValueTransform.GetComponent<TextMeshProUGUI>();
            
            int value = Player.m_localPlayer.GetInventory().CountItems(StoreGui.instance.m_coinPrefab.m_itemData.m_shared.m_name);
            Logger.Log("Value to calculate color: "+value);
            
            if (value == 0 && ConfigurationFile.advancedVendorMoneyPanel.Value)
                coinsValueText.faceColor = new Color(255, 0, 0, 255); 
            else
                coinsValueText.faceColor = new Color(255, 255, 255, 255);
        }
    }

    [HarmonyPatch(typeof(StoreGui), "OnSellItem")]
    public class MoneyStoreGuiOnSellItemPatch
    {
        public static void Postfix(StoreGui __instance)
        {
            if (!TraderChecks.HasStoreGuiValidTrader()) return;
            Logger.Log("Item sold. Recalculating...");
            MoneyStoreGuiShowPatch.updateValuables();
        }
    }
    
    [HarmonyPatch(typeof(StoreGui), "OnBuyItem")]
    public class MoneyStoreGuiOnBuyItemPatch
    {

        public static void Postfix(StoreGui __instance)
        {
            if (!TraderChecks.HasStoreGuiValidTrader()) return;
            Logger.Log("Item bought. Recalculating...");
            MoneyStoreGuiShowPatch.updateCoinsColor();
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
                if (!TraderChecks.HasStoreGuiValidTrader()) return;
                if (GameObject.Find("Store") != null)
                {
                    Logger.Log("Inventory changed while trader opened. Recalculating...");
                    MoneyStoreGuiShowPatch.updateValuables();
                    MoneyStoreGuiShowPatch.updateCoinsColor();
                }
            }
        }
    }
}