using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(VRInput))]
[AddComponentMenu("Event/VR Input Module")]
public class VRInputModule : StandaloneInputModule
{
    protected override void Awake()
    {
        m_InputOverride = GetComponent<VRInput>();
    }

    public override bool IsModuleSupported()
    {
        return base.IsModuleSupported() && UnityEngine.XR.XRSettings.isDeviceActive;
    }

    protected override MouseState GetMousePointerEventData(int id)
    {
        var state = base.GetMousePointerEventData(id);
        var button = state.GetButtonState(PointerEventData.InputButton.Left);
        
        button.eventData.buttonData.delta = Vector2.one;

        state.SetButtonState(PointerEventData.InputButton.Left, button.eventData.buttonState, button.eventData.buttonData);
        return state;
    }
}
