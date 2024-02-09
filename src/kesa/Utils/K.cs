using BepInEx.Logging;

namespace kesa.Utils;

public static class K
{
    private static readonly ManualLogSource kesa = Logger.CreateLogSource("Kesa Log");
    public static void Log(string msg) 
    {
        kesa.LogDebug(msg);
    }
}
