using EconomyInfo.tools;
using HarmonyLib;
using TMPro;
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

        private static bool panelsCreated = false;

        public static void enable(bool enable)
        {
            if (enable)
            {
                resize();
            }
            else
            {
                Transform storeTransform = GameObject.Find("Store").transform;
                storeTransform.Find("border (1)").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                storeTransform.Find("border (1)").GetComponent<RectTransform>().sizeDelta = new Vector2(40, 40);
                enableValuablePanels(false);
            }
        }

        public static void Postfix(StoreGui __instance, Trader trader)
        {
            bool configActive = ConfigurationFile.advancedVendorMoneyPanel.Value;
            if (!panelsCreated)
            {
                Transform storeTransform = GameObject.Find("Store").transform;
                amberPanel = new VendorPanelValuable(storeTransform, "amberPanel", "amber", configActive, new Vector2(0, -15), new Vector2(20, 20), new Vector2(42, 42));
                pearlPanel = new VendorPanelValuable(storeTransform, "amberpearlPanel", "AmberPearl", configActive, new Vector2(0, -60), new Vector2(8, 32));
                rubyPanel = new VendorPanelValuable(storeTransform, "rubyPanel", "ruby", configActive, new Vector2(0, -105), new Vector2(20, 20), new Vector2(42, 42));
                silverNecklacePanel = new VendorPanelValuable(storeTransform, "silverNecklacePanel", "silvernecklace", configActive, new Vector2(0, -150), new Vector2(18, 20), new Vector2(46, 46));
                panelsCreated = true;
            }
            if (configActive)
            {
                resize();
                updateValuables();
                updateCoinsColor();     
            }
        }

        private static void resize()
        {
            Transform storeTransform = GameObject.Find("Store").transform;
            storeTransform.Find("border (1)").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -90);
            storeTransform.Find("border (1)").GetComponent<RectTransform>().sizeDelta = new Vector2(40, 220);
            enableValuablePanels(true);
        }

        private static void enableValuablePanels(bool enable)
        {
            amberPanel.getMainPanel().SetActive(enable);
            pearlPanel.getMainPanel().SetActive(enable);
            rubyPanel.getMainPanel().SetActive(enable);
            silverNecklacePanel.getMainPanel().SetActive(enable);
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
                    }
                }
            }
            
            amberPanel.updateValue(totalAmountAmber, totalAmber);
            pearlPanel.updateValue(totalAmountAmberPearl, totalAmberPearl);
            rubyPanel.updateValue(totalAmountRuby, totalRuby);
            silverNecklacePanel.updateValue(totalAmountSilverNecklace, totalSilverNecklace);
            
            //Update coins color
            updateCoinsColor();
        }

        public static void updateCoinsColor()
        {
            Transform coinsValueTransform = GameObject.Find("Store").transform.Find("coins").transform.Find("coins");
            TextMeshProUGUI coinsValueText = coinsValueTransform.GetComponent<TextMeshProUGUI>();
            
            int value = Player.m_localPlayer.GetInventory().CountItems(StoreGui.instance.m_coinPrefab.m_itemData.m_shared.m_name);
            Logger.Log("Value to calculate color: "+value);
            if (value == 0)
            {
                coinsValueText.faceColor = new Color(255, 0, 0, 255); 
            }
            else
            {
                coinsValueText.faceColor = new Color(255, 255, 255, 255);
            }
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
    
    [HarmonyPatch(typeof(StoreGui), "OnBuyItem")]
    public class MoneyStoreGuiOnBuyItemPatch
    {

        public static void Postfix(StoreGui __instance)
        {
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