using System.Reflection;
using EconomyInfo.tools;

namespace EconomyInfo
{
    public class MoneyInventoryRecalculation
    {
        public static void RecalculateMoneyInventoryValue()
        {
            int total = 0;

            if (Player.m_localPlayer != null)
            {
                foreach (var item in Player.m_localPlayer.GetInventory().GetAllItems())
                {
                    if (item.m_shared.m_value > 0)
                    {
                        Logger.Log("Found in player inventory: " + item.m_shared.m_name + " = " + item.m_shared.m_value);
                        total += item.m_stack * item.m_shared.m_value;
                    }
                }
            }

            MoneyInventoryGuiPatch.moneyPanelInventory.updateMoneyValue(total.ToString());
        }
        
        public static void RecalculateCalculateChestValue(Container chest)
        {
            if (chest == null) return;

            int total = 0;

            var field = typeof(Container).GetField("m_inventory", BindingFlags.NonPublic | BindingFlags.Instance);
            Inventory inventoryContainer = (Inventory)field.GetValue(chest);
            foreach (var item in inventoryContainer.GetAllItems())
            {
                if (item.m_shared.m_value > 0)
                {
                    Logger.Log("Found in container inventory: " + item.m_shared.m_name + " = " + item.m_shared.m_value);
                    total += item.m_stack * item.m_shared.m_value;
                }
            }

            MoneyInventoryGuiPatch.moneyPanelContainer.updateMoneyValue(total.ToString());
        }
    }
}