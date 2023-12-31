﻿using BepInEx;
using SpaceWarp;
using SpaceWarp.API.Mods;
using System.Runtime.InteropServices;

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
        //Logger.LogInfo("Kesa Awake");
    }

    public override void OnPreInitialized()
    {
        //Logger.LogInfo("Kesa OnPreInitialized");
        KesaPlugin.Path = base.PluginFolderPath;
    }

    public override void OnInitialized()
    {
        //Logger.LogInfo("Kesa OnInitialized");
        base.OnInitialized();
        Instance = this;
        Logger.LogInfo("Kesa Mod Initialized");
        //Logger.LogInfo("Kesa sound bank");
        //byte[] bytes = File.ReadAllBytes(PluginFolderPath + @"/assets/soundbank/kesa.bnk");
        //Logger.LogInfo("Kesa sound bank:" + AkSoundEngine.LoadBankMemoryView(GCHandle.Alloc(bytes, GCHandleType.Pinned).AddrOfPinnedObject(), (uint)bytes.Length, out uint bankId));
    }
}