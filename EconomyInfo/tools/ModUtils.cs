using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace EconomyInfo.tools
{
    public class ModUtils
    {
        private static Dictionary<string, Sprite> cachedSprites = new Dictionary<string, Sprite>();

        public static Sprite getSprite(String name)
        {
            if (!cachedSprites.ContainsKey(name))
            {
                Logger.Log($"Finding {name} sprite...");
                var allSprites = Resources.FindObjectsOfTypeAll<Sprite>();
                for (var i = 0; i < allSprites.Length; i++)
                {
                    var sprite = allSprites[i];
                    if (sprite.name == name)
                    {
                        Logger.Log($"{name} sprite found.");
                        cachedSprites.Add(name, sprite);
                        return sprite;
                    }
                }
                Logger.Log($"{name} sprite NOT found.");
                return null;
            }

            return cachedSprites.GetValueSafe(name);
        }
        
        public static void RunPrivateMethod(object obj, string name, object[] parameters = null, BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.NonPublic)
        {
            obj.GetType().GetMethod(name, bindingAttr)?.Invoke(obj, parameters);
        }
    }
}
