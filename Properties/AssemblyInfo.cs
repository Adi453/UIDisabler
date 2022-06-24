using MelonLoader;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle(UIDisabler.BuildInfo.Name)]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany(UIDisabler.BuildInfo.Company)]
[assembly: AssemblyProduct(UIDisabler.BuildInfo.Name)]
[assembly: AssemblyCopyright("Created by " + UIDisabler.BuildInfo.Author)]
[assembly: AssemblyTrademark(UIDisabler.BuildInfo.Company)]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
//[assembly: Guid("")]
[assembly: AssemblyVersion(UIDisabler.BuildInfo.Version)]
[assembly: AssemblyFileVersion(UIDisabler.BuildInfo.Version)]
[assembly: NeutralResourcesLanguage("en")]
[assembly: MelonInfo(typeof(UIDisabler.UIDisabler), UIDisabler.BuildInfo.Name, UIDisabler.BuildInfo.Version, UIDisabler.BuildInfo.Author, UIDisabler.BuildInfo.DownloadLink)]


// Create and Setup a MelonModGame to mark a Mod as Universal or Compatible with specific Games.
// If no MelonModGameAttribute is found or any of the Values for any MelonModGame on the Mod is null or empty it will be assumed the Mod is Universal.
// Values for MelonModGame can be found in the Game's app.info file or printed at the top of every log directly beneath the Unity version.
[assembly: MelonGame(null, null)]