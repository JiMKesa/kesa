using kesa.Utils;
using KSP.Game;
using KSP.Modules;
using KSP.Sim.impl;
using KSP.Sim.ResourceSystem;
using BepInEx;
using kesa.UI;
using UnityEngine;

namespace kesa.Modules;

public class PartComponentModule_SpaceObs : PartComponentModule
{
    public override Type PartBehaviourModuleType => typeof(Module_SpaceObs);

    private Data_SpaceObs _dataSpaceobs;
    private static SpaceObsWindowController _SpaceObsWindowController;

    private PartComponentModule_ScienceExperiment _moduleScienceExperiment;
    public Data_Deployable DataDeployable;

    private FlowRequestResolutionState _returnedRequestResolutionState;
    private bool _hasOutstandingRequest;

    public override void OnStart(double universalTime)
    {
        if (!DataModules.TryGetByType<Data_SpaceObs>(out _dataSpaceobs))
            // check module exists
            if (!DataModules.TryGetByType<Data_SpaceObs>(out _dataSpaceobs))
        {
            return;
        }
        // setting today date
        Core.DateOfDay = DateTime.Now.ToString("dd/MM/yyyy");
        var existe = new bool();
        // check and set vessel DateLastObs
        if (Core.StoreData.SavePart.ContainsKey(base.Part.PartOwner.SimulationObject.Vessel.Name))
        {
            if (Core.StoreData.SavePart[base.Part.PartOwner.SimulationObject.Vessel.Name].ContainsKey("DateLastObs")) { existe = true; }
        }
        if (!existe)
        {
            var sv = new Dictionary<string, string>();
            sv.Add("DateLastObs", Core.date_to(Core.date_add(Core.date_from(Core.DateOfDay), Int32.Parse(_dataSpaceobs.DaysObj))));
            Core.StoreData.SavePart.Add(base.Part.PartOwner.SimulationObject.Vessel.Name, sv);
        }
        // set default screen windows
        Picture.DefaultScreen();
        _dataSpaceobs.PartComponentModule = this;
        // set Resources process for module
        _dataSpaceobs.SetupResourceRequest(base.resourceFlowRequestBroker);
    }
    // deployed part
    public bool IsPartDeployed(double universalTime)
    {
        // check if module need a deployable part and if yes, is deployed 
        var deployedbool = false;
        if (!_dataSpaceobs.Deployable)
        {
            _dataSpaceobs.IsDeployed = true;
            deployedbool = true;
        }
        else
        {
            // get the ScienceExperiment module
            Part.TryGetModule(typeof(PartComponentModule_ScienceExperiment), out var m);

            _moduleScienceExperiment = m as PartComponentModule_ScienceExperiment;
            Part.TryGetModule(typeof(PartComponentModule_Deployable), out var m2);
            if (m2 != null)
            {
                var moduleDeployable = m2 as PartComponentModule_Deployable;
                foreach (var dataDeployable in moduleDeployable.DataModules?.ValuesList)
                {
                    if (dataDeployable is Data_Deployable)
                    {
                        DataDeployable = dataDeployable as Data_Deployable;
                        if (DataDeployable.IsExtended)
                        {
                            _dataSpaceobs.IsDeployed = DataDeployable.IsExtended;
                            deployedbool = true;
                        }
                    }
                }
            }
        }
        return deployedbool;
    }
    // permanent action
    public override void OnUpdate(double universalTime, double deltaUniversalTime)
    {
        // Resource consumption for module
        ResourceConsumptionUpdate(deltaUniversalTime);
        // if deployed or not need deployed -> check if observation is needed
        if (IsPartDeployed(deltaUniversalTime))
        {
            SpaceObsCheck(deltaUniversalTime);
        }
        // Reload picture data
        if (!(Picture.DataPict) && (Core.DateOfDay == Core.LastModDayObs))
        {
            if (!(Core.DateOfDay == Picture.DatePict))
            {
                if (!Picture.DataLoading) Picture.GetXML(_dataSpaceobs.UrlPic);
            }
        }
        // check windows pooup value
        if (SpaceObsWindowController._isWindowOpen)
        {
            Core.PopUp = SpaceObsWindowController._toggle.value;
        }
        // async loading picture
        if ((!Picture.DataLoading) && (!(Picture.ShowPict)) && (!(Picture.Image.IsNullOrWhiteSpace())))
        {
            Picture.DataLoading = true;
            TextureLoader.DownloadTexture(Picture.Image);
        }
    }
    // SpaceObs check observation needed
    public void SpaceObsCheck(double universalTime)
    {
        if (!_dataSpaceobs.EnabledToggle.GetValue())
        {
            return;
        }
        var namevessel = base.Part.PartOwner.SimulationObject.Vessel.Name;
        // already checked and processed -> exit
        if (Core.DateOfDay == Core.StoreData.SavePart[namevessel]["DateLastObs"]) 
        {
            return; 
        }
        // set next observation date for vessel ( compared to last observation date)
        var nextobs = Core.date_to(Core.date_add(Core.date_from(Core.StoreData.SavePart[namevessel]["DateLastObs"]), Int32.Parse(_dataSpaceobs.DaysObj)));
        if (Core.DateOfDay == nextobs)
        {
            if (nextobs == Core.LastModDayObs)
            {
                // another vessel observation today -> exit
                Core.StoreData.SavePart[namevessel]["DateLastObs"] = Core.date_to(Core.date_add(Core.date_from(Core.StoreData.SavePart[namevessel]["DateLastObs"]), 1));
            }
            else 
            {
                // day of observation for this vessel
                doObservation(universalTime); 
            }
        }
        else
        {
            var nextdate = Core.date_add(Core.date_from(Core.StoreData.SavePart[namevessel]["DateLastObs"]), Int32.Parse(_dataSpaceobs.DaysObj));
            if (nextdate <= Core.date_from(Core.DateOfDay))
            {
                Core.StoreData.SavePart[namevessel]["DateLastObs"] = Core.date_to(nextdate);
            }
        }
    }
    public void doObservation(double universalTime)
    {
        // check and get XML data from source
        if (Picture.GetXML(_dataSpaceobs.UrlPic))
        {
            var namevessel = base.Part.PartOwner.SimulationObject.Vessel.Name;
            Core.StoreData.SavePart[namevessel]["DateLastObs"] = Core.DateOfDay;
            Core.LastModDayObs = Core.DateOfDay;
            Core.StoreData.LastModDayObs= Core.DateOfDay;
        }
    }
    // Resources 
    private void ResourceConsumptionUpdate(double deltaTime)
    {
        if (_dataSpaceobs.UseResources)
        {
            if (GameManager.Instance.Game.SessionManager.IsDifficultyOptionEnabled("InfinitePower"))
            {
                _dataSpaceobs.HasResourcesToOperate = true;
                if (base.resourceFlowRequestBroker.IsRequestActive(_dataSpaceobs.RequestHandle))
                {
                    base.resourceFlowRequestBroker.SetRequestInactive(_dataSpaceobs.RequestHandle);
                    return;
                }
            }
            else
            {
                if (this._hasOutstandingRequest)
                {
                    this._returnedRequestResolutionState = base.resourceFlowRequestBroker.GetRequestState(_dataSpaceobs.RequestHandle);
                    _dataSpaceobs.HasResourcesToOperate = this._returnedRequestResolutionState.WasLastTickDeliveryAccepted;
                }
                this._hasOutstandingRequest = false;
                if (!_dataSpaceobs.EnabledToggle.GetValue() && base.resourceFlowRequestBroker.IsRequestActive(_dataSpaceobs.RequestHandle))
                {
                    base.resourceFlowRequestBroker.SetRequestInactive(_dataSpaceobs.RequestHandle);
                    _dataSpaceobs.HasResourcesToOperate = false;
                }
                else if (_dataSpaceobs.EnabledToggle.GetValue() && base.resourceFlowRequestBroker.IsRequestInactive(_dataSpaceobs.RequestHandle))
                {
                    base.resourceFlowRequestBroker.SetRequestActive(_dataSpaceobs.RequestHandle);
                }
                if (_dataSpaceobs.EnabledToggle.GetValue())
                {
                    _dataSpaceobs.RequestConfig.FlowUnits = (double)_dataSpaceobs.RequiredResource.Rate;
                    base.resourceFlowRequestBroker.SetCommands(_dataSpaceobs.RequestHandle, 1.0, new ResourceFlowRequestCommandConfig[] { _dataSpaceobs.RequestConfig });
                    this._hasOutstandingRequest = true;
                    return;
                }
            }
        }
        else
        {
            _dataSpaceobs.HasResourcesToOperate = true;
        }
    }
    
    public override void OnShutdown()
    {

    }
}
