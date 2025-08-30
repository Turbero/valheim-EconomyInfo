using BepInEx.Configuration;
using BepInEx;
using System.IO;

namespace EconomyInfo.tools
{
    internal class ConfigurationFile
    {
        public static ConfigEntry<bool> debug;

        private static ConfigFile configFile;
        private static string ConfigFileName = EconomyInfo.GUID + ".cfg";
        private static string ConfigFileFullPath = Paths.ConfigPath + Path.DirectorySeparatorChar + ConfigFileName;

        internal static void LoadConfig(BaseUnityPlugin plugin)
        {
            {
                configFile = plugin.Config;

                debug = configFile.Bind("1 - General", "DebugMode", false, "Enabling/Disabling the debugging in the console (default = false)");
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
            }
            catch
            {
                Logger.LogError($"There was an issue loading {ConfigFileName}");
            }
        }
    }
}