using JetBrains.Annotations;
using kesa.UI;
using KSP.Game;
using KSP.UI.Binding;
using UnityEngine;
using UnityEngine.Networking;

namespace kesa.Utils;

public static class TextureLoader
{
    private static SpaceObsWindowController _SpaceObsWindowController;
    public static async Task DownloadTexture(string uri)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(uri, true))
        {
            AsyncOperation asyncOp = uwr.SendWebRequest();
            while (asyncOp.isDone == false)
            {
                if (!Application.isPlaying) 
                {
                    Picture.DataLoading = false;
                    return; 
                }
                else
                {
                    K.Log($"KESALOADING : {asyncOp.progress * 100}% {uri}");
                    await Task.Delay(1000 / 60);
                }
            }
            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Picture.DataLoading = false;
                return;
            }
            else
            {
                var tex = DownloadHandlerTexture.GetContent(uwr);
                if (tex != null) Picture.TexturePict = tex;
                Picture.ShowPict = true;
                Picture.DataLoading = false;
                if (Core.PopUp)
                {
                    SpaceObsWindowController._isWindowOpen = true;
                    GameObject.Find(kesaPlugin.ToolbarFlightButtonID)
                ?.GetComponent<UIValue_WriteBool_Toggle>()
                ?.SetValue(true);
                    GameObject.Find(kesaPlugin.ToolbarOabButtonID)
                        ?.GetComponent<UIValue_WriteBool_Toggle>()
                        ?.SetValue(true);
                    _SpaceObsWindowController.OnShow();
                }
                else
                {
                    KMessages.KAlertLow(LocalizationStrings.PARTMODULES["Name"], LocalizationStrings.PARTMODULES["Alert"]);
                }
            }
        }
    }
}

