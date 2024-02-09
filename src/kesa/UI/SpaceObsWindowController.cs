using BepInEx;
using JTemp;
using kesa.Utils;
using KSP.UI.Binding;
using UitkForKsp2.API;
using UnityEngine;
using UnityEngine.UIElements;

namespace kesa.UI;

public class SpaceObsWindowController : MonoBehaviour
{
    private UIDocument _window;
    private VisualElement _rootElement;
    private Label _titre;
    private VisualElement _photo;
    private Label _copyright;
    public static Toggle _toggle;
    public static bool _isWindowOpen;

    public bool IsWindowOpen
    {
        get => _isWindowOpen;
        set
        {
            _isWindowOpen = value;
            _rootElement.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
            GameObject.Find(kesaPlugin.ToolbarFlightButtonID)
                ?.GetComponent<UIValue_WriteBool_Toggle>()
                ?.SetValue(value);
            GameObject.Find(kesaPlugin.ToolbarOabButtonID)
                ?.GetComponent<UIValue_WriteBool_Toggle>()
                ?.SetValue(value);
            if (_isWindowOpen == true) OnShow();

        }
        
    }
    private void OnEnable()
    {
        _window = GetComponent<UIDocument>();
        _rootElement = _window.rootVisualElement[0];
        _titre = _rootElement.Q<Label>("Titre");
        _photo= _rootElement.Q<VisualElement>("Photo");
        _copyright = _rootElement.Q<Label>("Copyright");
        _rootElement.CenterByDefault();
        var closeButton = _rootElement.Q<Button>("close-button");
        closeButton.clicked += () => IsWindowOpen = false;
        _toggle = _rootElement.Q <Toggle>("Toggle");

        // picture data process
        OnShow();
    }
    // Picture data process when opening UI windows
    public void OnShow()
    {
        if (Picture.ShowPict)
        {
            //element.style.backgroundImage = texture;
            //unity-background-scale-mode: stretch-to-fill | scale-and-crop | scale-to-fit
            _photo.style.backgroundImage = Picture.TexturePict;
            _titre.text = Picture.Title;
            _copyright.text = "© " + Picture.Copyright;
            _toggle.labelElement.text = LocalizationStrings.WINDOWS["Popup"];
        }
        else
        {
            //DefaultTexture;
            _photo.style.backgroundImage = Picture.DefaultTexture;
            _toggle.labelElement.text = LocalizationStrings.WINDOWS["NoData"];
            _copyright.text = "© ";
            _toggle.labelElement.text = LocalizationStrings.WINDOWS["Popup"];
        }
    }
}