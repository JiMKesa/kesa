using KSP.Sim.Definitions;
using UnityEngine;

namespace kesa.Modules;
[DisallowMultipleComponent]
public class Module_SpaceObs : PartBehaviourModule
{
    public override Type PartComponentModuleType => typeof(PartComponentModule_SpaceObs);

    [SerializeField]
    protected Data_SpaceObs _Data_SpaceObs;
    
    public override void AddDataModules()
    {
        base.AddDataModules();
        _Data_SpaceObs ??= new Data_SpaceObs();
        DataModules.TryAddUnique(_Data_SpaceObs, out _Data_SpaceObs);
    }
    public override void OnInitialize()
    {
        base.OnInitialize();
        if (PartBackingMode == PartBackingModes.Flight)
        {
            moduleIsEnabled = true;
            _Data_SpaceObs.EnabledToggle.OnChangedValue += OnToggleChangedValue;
            var isEnabled = _Data_SpaceObs.EnabledToggle.GetValue();
            UpdateFlightPAMVisibility(isEnabled);
        }
        else
        {
            UpdateOabPAMVisibility();
        }
    }
    public override void OnModuleFixedUpdate(float fixedDeltaTime)
    {
        if (!_Data_SpaceObs.EnabledToggle.GetValue())
            return;
    }
    public override void OnShutdown()
    {
        _Data_SpaceObs.EnabledToggle.OnChangedValue -= OnToggleChangedValue;
    }
    private void OnToggleChangedValue(bool newValue)
    {
        UpdateFlightPAMVisibility(newValue);
    }
    private void UpdateFlightPAMVisibility(bool state)
    {
        _Data_SpaceObs.SetVisible(_Data_SpaceObs.EnabledToggle, true);
    }
    private void UpdateOabPAMVisibility()
    {
        _Data_SpaceObs.SetVisible(_Data_SpaceObs.EnabledToggle, false);
    }

}
