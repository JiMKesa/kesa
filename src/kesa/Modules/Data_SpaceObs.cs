using KSP.Sim;
using KSP.Sim.Definitions;
using Newtonsoft.Json;
using kesa.Utils;
using KSP.Game;
using KSP.Sim.ResourceSystem;

namespace kesa.Modules;
[Serializable]
public class Data_SpaceObs : ModuleData
{
    public override Type ModuleType => typeof(Module_SpaceObs);

    [KSPState]
    [LocalizedField("Kesa/PartModules/SpaceObs/Enabled")]
    [PAMDisplayControl(SortIndex = 1)]
    public ModuleProperty<bool> EnabledToggle = new ModuleProperty<bool>(false);

    [KSPDefinition] public string UrlPic;
    [KSPDefinition] public string DaysObj;
    [KSPDefinition] public bool Deployable;

    public bool IsDeployed = false;

    // OAB SpaceObs description
    public override List<OABPartData.PartInfoModuleEntry> GetPartInfoEntries(Type partBehaviourModuleType,
        List<OABPartData.PartInfoModuleEntry> delegateList)
    {
        if (partBehaviourModuleType == ModuleType)
        {
            // add SpaceObs module description description
            delegateList.Add(new OABPartData.PartInfoModuleEntry("", (_) => LocalizationStrings.OAB_DESCRIPTION["ModuleDescription"]));
            // MapType header
            var entry = new OABPartData.PartInfoModuleEntry(LocalizationStrings.OAB_DESCRIPTION["ResourcesRequired"],
                _ =>
                {
                    // Subentries
                    var subEntries = new List<OABPartData.PartInfoModuleSubEntry>();
                    if (UseResources)
                    {
                        subEntries.Add(new OABPartData.PartInfoModuleSubEntry(
                            LocalizationStrings.OAB_DESCRIPTION["ElectricCharge"],
                            $"{RequiredResource.Rate.ToString("N3")} /s"
                        ));
                    }
                    return subEntries;
                });
            delegateList.Add(entry);
        }
        return delegateList;
    }

    public override void SetupResourceRequest(ResourceFlowRequestBroker resourceFlowRequestBroker)
    {
        if (UseResources)
        {
            ResourceDefinitionID resourceIDFromName = GameManager.Instance.Game.ResourceDefinitionDatabase.GetResourceIDFromName(this.RequiredResource.ResourceName);
            if (resourceIDFromName == ResourceDefinitionID.InvalidID)
            {
                return;
            }
            RequestConfig = new ResourceFlowRequestCommandConfig();
            RequestConfig.FlowResource = resourceIDFromName;
            RequestConfig.FlowDirection = FlowDirection.FLOW_OUTBOUND;
            RequestConfig.FlowUnits = 0.0;
            RequestHandle = resourceFlowRequestBroker.AllocateOrGetRequest("ModuleSpaceObs", default(ResourceFlowRequestHandle));
            resourceFlowRequestBroker.SetCommands(this.RequestHandle, 1.0, new ResourceFlowRequestCommandConfig[] { this.RequestConfig });
        }
    }

    [KSPDefinition]
    public bool UseResources = true;

    public bool HasResourcesToOperate = true;

    [KSPDefinition]
    public PartModuleResourceSetting RequiredResource;

    public ResourceFlowRequestCommandConfig RequestConfig;

    [JsonIgnore]
    public PartComponentModule_SpaceObs PartComponentModule;

}
