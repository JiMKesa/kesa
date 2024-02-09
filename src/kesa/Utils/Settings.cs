using I2.Loc;

namespace kesa.Utils;

// class for store mod datas
public class Core
{
    public Core()
    {
        StoreData = new();
    }
    public static Core Instance { get; } = new();
    public static SaveDataAdapter StoreData { get; set; }

    public static string DateOfDay;
    public static string LastModDayObs;
    public static bool PopUp = true;

    public static DateTime date_now()
    {
        var now = DateTime.Now; return now;
    }
    public static String date_to(DateTime date)
    {
        return date.ToString("dd/MM/yyyy");
    }
    public static DateTime date_from(String  date)
    {
        return new DateTime(Int32.Parse(date.Substring(6, 4)), Int32.Parse(date.Substring(3, 2)), Int32.Parse(date.Substring(0, 2)));
    }
    public static DateTime date_add(DateTime date, int nb)
    {
        return date.AddDays(nb);
    }
    public static String AjustDateLastObs(String date, String dateref, int Decalage)
    {
        var sdate = date_from(date);
        var sdateref = date_from(dateref);
        var sdatelast = date_add(sdateref, -Decalage);
        int result = DateTime.Compare(sdate, sdatelast);
        if ( result<0)
        {
            sdate = sdatelast;
        }
        return date_to(sdate);
    }
}

public static class LocalizationStrings
{
    public static readonly Dictionary<string, LocalizedString> OAB_DESCRIPTION = new()
    {
        { "ModuleDescription", "Kesa/SpaceObs/OAB/Description" },
        { "ResourcesRequired", "Kesa/SpaceObs/OAB/ResourcesRequiredTitle" },
        { "ElectricCharge", "Kesa/SpaceObs/OAB/ResourcesRequiredEntry" }
    };
    public static readonly Dictionary<string, LocalizedString> PARTMODULES = new()
    {
        {"Name", "Kesa/SpaceObs/PartModules/Name"},
        {"Alert", "Kesa/SpaceObs/PartModules/Alert"}
    };
    public static readonly Dictionary<string, LocalizedString> WINDOWS = new()
    {
        {"Popup", "Kesa/SpaceObs/Window/Popup"},
        {"NoData", "Kesa/SpaceObs/Window/NoData"}
    };
}
