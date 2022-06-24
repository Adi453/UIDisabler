using MelonLoader;
using StressLevelZero.Interaction;
using UnityEngine;
using ModThatIsNotMod;
using ModThatIsNotMod.BoneMenu;
using Object = UnityEngine.Object;
using Colour = UnityEngine.Color;
using HarmonyLib;
using StressLevelZero.Props.Weapons;
using System.Reflection;

namespace UIDisabler
{
    public static class BuildInfo
    {
        public const string Name = "UIDisabler"; // Name of the Mod.  (MUST BE SET)
        public const string Author = "Adidasaurus"; // Author of the Mod.  (Set as null if none)
        public const string Company = null; // Company that made the Mod.  (Set as null if none)
        public const string Version = "1.0.0"; // Version of the Mod.  (MUST BE SET)
        public const string DownloadLink = null; // Download Link for the Mod.  (Set as null if none)
    }

    public class UIDisabler : MelonMod
    {
        public static bool forcePull = true;
        public static bool ammo = true;
        public static bool gun = true;
        public static GameObject hud;

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            hud = Player.GetRigManager().transform.Find("[UIRig]/PLAYERUI/Hud").gameObject;
            hud.SetActive(!ammo);
        }
        public override void OnApplicationQuit() => Preferences.SavePrefs();
        public override void OnApplicationStart()
        {
            Preferences.LoadPrefs();
            foreach(MethodInfo meth in typeof(InteractableIcon).GetMethods())
            {
                if (meth.Name.Contains("My") && meth.Name.Contains("Hand") && meth.Name.Contains("Hover"))
                    HarmonyInstance.Patch(meth, prefix: typeof(UIDisabler).GetMethod("Prefix").ToNewHarmonyMethod());
            }
                
            MenuCategory cat = MenuManager.CreateCategory("UI Disabler", Colour.white);
            cat.CreateBoolElement("Disable Force Pull / Grab Indicator", Colour.white, forcePull, delegate (bool value)
            {
                forcePull = value;
            });
            
            cat.CreateBoolElement("Disable Ammo UI", Colour.white, ammo, delegate (bool value)
            {
                ammo = value;
                hud.SetActive(!ammo);
            });
            cat.CreateBoolElement("Disable Gun Reload Helper Thing", Colour.white, gun, delegate (bool value)
            {
                gun = value;
            });
        }
        public static void Prefix(InteractableIcon __instance) => __instance.IconSize = forcePull? 0 : 0.015f;

        
        [HarmonyPatch(typeof(Gun), "CoBlinkHighlight")]
        public static class GunPatch
        {
            public static bool Prefix(Gun __instance) => !gun;
        }

        
    }
}
