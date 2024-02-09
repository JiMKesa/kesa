using BepInEx;
using kesa.Modules;
using System.Xml;
using UnityEngine;
using SpaceWarp.API.Assets;
using System;
using UnityEngine.Networking;
using MoonSharp.VsCodeDebugger.SDK;
using kesa.UI;

namespace kesa.Utils;

public class Picture
{
    public static string DatePict;
    public static string Copyright;
    public static string Title;
    public static string Image;
    public static bool XMLLoading = false;
    public static bool DataPict = false;
    public static bool DataLoading = false;
    public static bool ShowPict = false;
    public static Texture2D TexturePict;
    public static Texture2D DefaultTexture;
    public static Stream StreamPic;
    public static Byte[] ImageBytes;

    private Data_SpaceObs _dataSpaceobs;

    // set default screen texture
    public static void DefaultScreen()
    {
        DefaultTexture = AssetManager.GetAsset<Texture2D>($"{kesaPlugin.ModGuid}/kesa_ui/mod/ui/spaceobs/default_screen.png");
    }
    // loader xml & picture infos
    
    public static bool GetXML(string urlpic)
    {
        // get xml data from source
        string URLString = urlpic;
        var found = false;
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(URLString);
        XmlNode DataNode0 = xmlDoc.SelectSingleNode("//channel/copyright");
        if (DataNode0 != null)
            Picture.Copyright = DataNode0.InnerText;
        XmlNode DataNode1 = xmlDoc.SelectSingleNode("//channel/item/title");
        if (DataNode1 != null)
            Picture.Title = DataNode1.InnerText;
        XmlNode DataNode2 = xmlDoc.SelectSingleNode("//channel/item/enclosure ");
        if (DataNode2 != null)
        {
            Picture.Image = DataNode2.Attributes["url"].Value;
            if (!(Picture.Image.IsNullOrWhiteSpace()))
                found = true;
        }
        if (found)
        {
            //set date of picture
            DatePict = Core.DateOfDay;
        }
        return found;
    }
}
