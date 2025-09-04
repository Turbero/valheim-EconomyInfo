using HarmonyLib;
using UnityEngine;

namespace EconomyInfo
{
    [HarmonyPatch(typeof(StoreGui), "Show")]
    public class MoneyStoreGuiAwakePatch {
        
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

            //TODO Update valuable 
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
    }

    [HarmonyPatch(typeof(StoreGui), "OnSellItem")]
    public class MoneyStoreGuiOnSellItemPatch
    {

        public static void Postfix(StoreGui __instance)
        {

        }
    }
    
    [HarmonyPatch(typeof(StoreGui), "Show")]
    public class MoneyStoreGuiShowPatch
    {

        public static void Postfix(StoreGui __instance, Trader trader)
        {
            //TODO Recalculate valuable panel values
            
        }
    }
}