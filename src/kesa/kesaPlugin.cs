using BepInEx;
using SpaceWarp;
using SpaceWarp.API.Mods;

namespace kesa;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency(SpaceWarpPlugin.ModGuid, SpaceWarpPlugin.ModVer)]
public class KesaPlugin : BaseSpaceWarpPlugin
{
    public const string ModGuid = MyPluginInfo.PLUGIN_GUID;
    public const string ModName = MyPluginInfo.PLUGIN_NAME;
    public const string ModVer = MyPluginInfo.PLUGIN_VERSION;
    public static KesaPlugin Instance { get; set; }
    public static string Path { get; private set; }
    private void Awake()
    {

    }

    public override void OnPreInitialized()
    {
        KesaPlugin.Path = base.PluginFolderPath;
    }

    public override void OnInitialized()
    {
        base.OnInitialized();
        Instance = this;
    }
}
