using System;
using BepInEx.Configuration;
using BepInEx;
using System.IO;
using EconomyInfo.money_inventory;
using EconomyInfo.money_vendor;
using UnityEngine;

namespace EconomyInfo.tools
{
    internal class ConfigurationFile
    {
        public static ConfigEntry<bool> debug;
        public static ConfigEntry<bool> showInventoryMoneyBalance;
        public static ConfigEntry<bool> showContainerMoneyBalance;
        public static ConfigEntry<bool> advancedVendorMoneyPanel;

        private static ConfigFile configFile;
        private static string ConfigFileName = EconomyInfo.GUID + ".cfg";
        private static string ConfigFileFullPath = Paths.ConfigPath + Path.DirectorySeparatorChar + ConfigFileName;

        internal static void LoadConfig(BaseUnityPlugin plugin)
        {
            {
                configFile = plugin.Config;

                debug = configFile.Bind("1 - General", "DebugMode", false, "Enabling/Disabling the debugging in the console (default = false)");
                showInventoryMoneyBalance = configFile.Bind("2 - Features", "Show Inventory Money Balance", true, "Enable/disable the inventory money balance (default = true)");
                showContainerMoneyBalance = configFile.Bind("2 - Features", "Show Container Money Balance", true, "Enable/disable the container money balance (default = true)");
                advancedVendorMoneyPanel = configFile.Bind("2 - Features", "Show Advanced Vendor Money Balance", true, "Enabling/disabling the advanced money balance with all valuables at the vendor window (default = true)");
                SetupWatcher();
            }
        }
        
        private static void SetupWatcher()
        {
            FileSystemWatcher watcher = new FileSystemWatcher(Paths.ConfigPath, ConfigFileName);
            watcher.Changed += ReadConfigValues;
            watcher.Created += ReadConfigValues;
            watcher.Renamed += ReadConfigValues;
            watcher.IncludeSubdirectories = true;
            watcher.SynchronizingObject = ThreadingHelper.SynchronizingObject;
            watcher.EnableRaisingEvents = true;
        }
        
        private static void ReadConfigValues(object sender, FileSystemEventArgs e)
        {
            if (!File.Exists(ConfigFileFullPath)) return;
            try
            {
                Logger.Log("Attempting to reload configuration...");
                configFile.Reload();
                SettingsChanged(null, null);
            }
            catch (Exception ex)
            {
                Logger.LogError($"There was an issue loading {ConfigFileName}, {ex}");
            }
        }

        private static void SettingsChanged(object sender, EventArgs e)
        {
            if (GameObject.Find("Store") != null) MoneyStoreGuiShowPatch.enable(advancedVendorMoneyPanel.Value);
            MoneyInventoryGuiPatch.moneyPanelInventory.getGameObject().SetActive(showInventoryMoneyBalance.Value);
            MoneyInventoryGuiPatch.moneyPanelContainer.getGameObject().SetActive(showContainerMoneyBalance.Value);
        }
    }
}