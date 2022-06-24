using MelonLoader;
using static UIDisabler.UIDisabler;

namespace UIDisabler
{
    public class Preferences
    {
        public const string Dis = "UI Disabler";
        public const string reload = "GunReloadHelper";
        public const string AUI = "Ammo UI";
        public const string For = "Forcepull / Grab";
        public static void LoadPrefs()
        {
            var category = MelonPreferences.CreateCategory(Dis);
            gun = category.CreateEntry<bool>(reload, gun, reload).Value;
            ammo = category.CreateEntry<bool>(AUI, ammo, AUI).Value;
            forcePull = category.CreateEntry<bool>(For, forcePull, For).Value;
            
        }
        public static void SavePrefs()
        {
            MelonPreferences.SetEntryValue<bool>(Dis, reload, gun);
            MelonPreferences.SetEntryValue<bool>(Dis, AUI, ammo);
            MelonPreferences.SetEntryValue<bool>(Dis, For, forcePull);
            MelonPreferences.Save();
        }
    }
}
