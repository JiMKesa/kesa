using Newtonsoft.Json;
using System.Text.Json;
using SpaceWarp.API.SaveGameManager;

namespace kesa.Utils;

public class SaveManager
{
    private SaveManager() 
    {
    }
    public static SaveManager Instance { get; } = new();
    // loading buffer
    public SaveDataAdapter bufferedLoadData;
    public bool HasBufferedLoadData;
    public void Register()
    {
        ModSaves.RegisterSaveLoadGameData<SaveDataAdapter>(
            kesaPlugin.ModGuid,
            OnSave,
            OnLoad
        );
    }
    // save data on campaign 
    public void OnSave(SaveDataAdapter dataToSave)
    {
        dataToSave.LastModDayObs = Core.LastModDayObs;
        dataToSave.PopUp = Core.PopUp;
        dataToSave.SavePart = Core.StoreData.SavePart;
    }
    // load data from campaign 
    public void OnLoad(SaveDataAdapter dataToLoad)
    {
        bufferedLoadData = dataToLoad;
        HasBufferedLoadData = true;
        Core.LastModDayObs = bufferedLoadData.LastModDayObs;
        Core.PopUp = bufferedLoadData.PopUp;
        Core.StoreData.SavePart.Clear();
        Core.StoreData.SavePart = bufferedLoadData.SavePart;
        bufferedLoadData = null;
        HasBufferedLoadData = false;
    }
}
