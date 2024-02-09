using System.Reflection;
using BepInEx;
using JetBrains.Annotations;
using SpaceWarp;
using SpaceWarp.API.Assets;
using SpaceWarp.API.Mods;
using SpaceWarp.API.UI.Appbar;
using kesa.UI;
using UitkForKsp2.API;
using UnityEngine;
using UnityEngine.UIElements;
using kesa.Utils;
using kesa.Modules;

namespace kesa;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency(SpaceWarpPlugin.ModGuid, SpaceWarpPlugin.ModVer)]
public class kesaPlugin : BaseSpaceWarpPlugin
{
    // Useful in case some other mod wants to use this mod a dependency
    [PublicAPI] public const string ModGuid = MyPluginInfo.PLUGIN_GUID;
    [PublicAPI] public const string ModName = MyPluginInfo.PLUGIN_NAME;
    [PublicAPI] public const string ModVer = MyPluginInfo.PLUGIN_VERSION;

    /// Singleton instance of the plugin class
    [PublicAPI] public static kesaPlugin Instance { get; set; }

    // AppBar button IDs
    internal const string ToolbarFlightButtonID = "BTN-kesaFlight";
    internal const string ToolbarOabButtonID = "BTN-kesaOAB";
    internal const string ToolbarKscButtonID = "BTN-kesaKSC";

    public override void OnInitialized()
    {
        base.OnInitialized();

        Instance = this;

        // Day date set
        Core.DateOfDay = DateTime.Now.ToString("dd/MM/yyyy");
        //Register for events : load and save 
        SaveManager.Instance.Register();
        // Register module for background resource processing
        SpaceWarp.API.Parts.PartComponentModuleOverride
            .RegisterModuleForBackgroundResourceProcessing<PartComponentModule_SpaceObs>();
        
        // Load all the other assemblies used by this mod
        LoadAssemblies();

        // Load the UI from the asset bundle
        var SpaceObsWindowUxml = AssetManager.GetAsset<VisualTreeAsset>(
            $"{ModGuid}/" +
            "kesa_ui/" +
            "Mod/UI/SpaceObs/SpaceObs.uxml"
        );

        // Create the window options object
        var windowOptions = new WindowOptions
        {
            WindowId = "kesa_SpaceObs",
            Parent = null,
            IsHidingEnabled = true,
            DisableGameInputForTextFields = true,
            MoveOptions = new MoveOptions
            {
                IsMovingEnabled = true,
                CheckScreenBounds = true
            }
        };

        // Create the window
        var SpaceObsWindow = Window.Create(windowOptions, SpaceObsWindowUxml);
        // Add a controller for the UI to the window's game object
        var SpaceObsWindowController = SpaceObsWindow.gameObject.AddComponent<SpaceObsWindowController>();

        SpaceObsWindowController.IsWindowOpen = false;

        // Register Flight AppBar button
        Appbar.RegisterAppButton(
        ModName,
            ToolbarFlightButtonID,
            AssetManager.GetAsset<Texture2D>($"{ModGuid}/images/icon.png"),
            isOpen => SpaceObsWindowController.IsWindowOpen = isOpen
        );

        // Register OAB AppBar Button
        Appbar.RegisterOABAppButton(
            ModName,
            ToolbarOabButtonID,
            AssetManager.GetAsset<Texture2D>($"{ModGuid}/images/icon.png"),
            isOpen => SpaceObsWindowController.IsWindowOpen = isOpen
        );

        // Register KSC AppBar Button
        Appbar.RegisterKSCAppButton(
            ModName,
            ToolbarKscButtonID,
            AssetManager.GetAsset<Texture2D>($"{ModGuid}/images/icon.png"),
            () => SpaceObsWindowController.IsWindowOpen = !SpaceObsWindowController.IsWindowOpen
        );
    }

    public void Update()
    {
        Core.DateOfDay = Core.date_to(Core.date_now());
    }

    /// Loads all the assemblies for the mod.
    private static void LoadAssemblies()
    {
        // Load the Unity project assembly
        var currentFolder = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory!.FullName;
        var unityAssembly = Assembly.LoadFrom(Path.Combine(currentFolder, "kesa.Unity.dll"));
        // Register any custom UI controls from the loaded assembly
        CustomControls.RegisterFromAssembly(unityAssembly);
    }
}
