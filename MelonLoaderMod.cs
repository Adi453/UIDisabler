using MelonLoader;
using StressLevelZero.Interaction;
using UnityEngine;
using ModThatIsNotMod;
using ModThatIsNotMod.BoneMenu;
using Object = UnityEngine.Object;
using Colour = UnityEngine.Color; //god save the queen
using HarmonyLib;
using StressLevelZero.Props.Weapons;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace UIDisabler
{
    public static class BuildInfo
    {
        public const string Name = "UIDisabler"; // Name of the Mod.  (MUST BE SET)
        public const string Author = "Adidasaurus"; // Author of the Mod.  (Set as null if none)
        public const string Company = null; // Company that made the Mod.  (Set as null if none)
        public const string Version = "1.0.1"; // Version of the Mod.  (MUST BE SET)
        public const string DownloadLink = "https://boneworks.thunderstore.io/package/Adidasaurus/UIDisabler/"; // Download Link for the Mod.  (Set as null if none)
    }

    public class UIDisabler : MelonMod
    {
        public static bool forcePull = true;
        public static bool ammo = true;
        public static bool gun = true;
        public static bool ammoPouch = false;
        public static bool holster = false;
        public static bool accesories = false;
        public static List<MeshRenderer> pouch = new List<MeshRenderer>();
        public static List<MeshRenderer> holsters = new List<MeshRenderer>();
        public static List<Renderer> ac = new List<Renderer>();
        public static GameObject hud;

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            pouch.Clear();
            holsters.Clear();
            ac.Clear();
            hud = Player.GetRigManager().transform.Find("[UIRig]/PLAYERUI/Hud").gameObject;
            hud.SetActive(!ammo);
            GameObject bruhhh = Player.GetRigManager()?.transform?.Find("[SkeletonRig (GameWorld Brett)]/Body/skull/c4Vertebra/t1Vertebra/t7Vertebra/l1Vertebra/l3Vertebra/sacrum/BeltLf1/slot_container/prop_RifleMagHolder")?.gameObject;
            
            foreach(var m in bruhhh.GetComponentsInChildren<MeshRenderer>(true))
                pouch.Add(m);

            GameObject back_pouch = Player.GetRigManager()?.transform?.Find("[SkeletonRig (GameWorld Brett)]/Body/skull/c4Vertebra/t1Vertebra/t7Vertebra/l1Vertebra/l3Vertebra/sacrum/BackCt/prop_pouch")?.gameObject;
            ac.Add(back_pouch.GetComponent<MeshRenderer>());

            GameObject holsterParent = Player.GetRigManager()?.transform?.Find("[SkeletonRig (GameWorld Brett)]/Body/skull/c4Vertebra/t1Vertebra/t7Vertebra")?.gameObject;

            foreach(var h in holsterParent.GetComponentsInChildren<SlotContainer>(true))
                foreach (var m in h.gameObject.GetComponentsInChildren<MeshRenderer>(true))
                    if(!pouch.Any(i => i.GetInstanceID() == m.GetInstanceID()))
                        holsters.Add(m);
            GameObject actualBelt = Player.GetRigManager()?.transform?.Find("[SkeletonRig (GameWorld Brett)]/Brett@neutral/geoGrp/brett_accessories_belt_mesh")?.gameObject;
            ac.Add(actualBelt.GetComponent<SkinnedMeshRenderer>()); //Keep the belt on bro

            GameObject straps = Player.GetRigManager()?.transform?.Find("[SkeletonRig (GameWorld Brett)]/Brett@neutral/geoGrp/brett_accessories_shoulderStraps")?.gameObject;
            ac.Add(straps.GetComponent<SkinnedMeshRenderer>());

            foreach (var r in pouch)
                r.enabled = !ammoPouch;

            foreach (var r in holsters)
                r.enabled = !holster;

            foreach (var r in ac)
                r.enabled = !accesories;

        }
        public override void OnApplicationQuit() => Preferences.SavePrefs();
        public override void OnApplicationStart()
        {
            Preferences.LoadPrefs();
            foreach(MethodInfo meth in typeof(InteractableIcon).GetMethods().Where(i => i.Name.Contains("HandHover")))
                HarmonyInstance.Patch(meth, prefix: typeof(UIDisabler).GetMethod("Prefix").ToNewHarmonyMethod());

            MenuCategory cat = MenuManager.CreateCategory("UI Disabler", Colour.white);
            cat.CreateBoolElement("Disable Force Pull / Grab Indicator", Colour.white, forcePull, delegate (bool value) { forcePull = value; });
            cat.CreateBoolElement("Disable Ammo UI", Colour.white, ammo, delegate (bool value)
            {
                ammo = value;
                hud.SetActive(!ammo);
            });
            cat.CreateBoolElement("Disable Gun Reload Helper Thing", Colour.white, gun, delegate (bool value) { gun = value; });
            cat.CreateBoolElement("Hide Ammo Pouch", Colour.white, ammoPouch, delegate (bool value)
            {
                ammoPouch = value;
                foreach (var r in pouch)
                    r.enabled = !ammoPouch;

            });
            cat.CreateBoolElement("Hide Holsters", Colour.white, holster, delegate (bool value)
            {
                holster = value;
                foreach (var r in holsters)
                    r.enabled = !holster;
            });
            cat.CreateBoolElement("Hide Accessories", Colour.white, accesories, delegate (bool value)
            {
                accesories = value; 
                foreach (var r in ac)
                    r.enabled = !accesories;
            });
        }
        public static void Prefix(InteractableIcon __instance) => __instance.IconSize = forcePull ? 0 : 0.015f;

        [HarmonyPatch(typeof(Gun), "CoBlinkHighlight")]
        public static class GunPatch
        {
            public static bool Prefix() => !gun;
        }
    }
}
