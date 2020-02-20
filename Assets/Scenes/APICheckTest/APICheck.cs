using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_2017_2_OR_NEWER
using UnityEngine.XR;
#else
using UnityEngine.VR;
#endif

public class APICheck : MonoBehaviour
{

    public UnityEngine.UI.Text textField;
    private string _deviceLoaded = "";

    private void Awake()
    {
        XRDevice.deviceLoaded += isDeviceLoaded;
    }

    private void isDeviceLoaded(string obj)
    {
        _deviceLoaded = $"Device Loaded {obj}";
    }

    void Update()
    {

        if (textField != null)
        {

            string text = "";

#if UNITY_2017_2_OR_NEWER
            text += "XRDevice\n";
            text += "\tisPresent: " + XRDevice.isPresent + "\n";
            text += "\tmodel: " + XRDevice.model + "\n";
            text += "\trefreshRate: " + XRDevice.refreshRate + "\n";
            text += "\tuserPresence: " + XRDevice.userPresence + "\n";
            text += "\tfovZoomFactor: " + XRDevice.fovZoomFactor + "\n";
            text += "\ttrackingORiginMode:" + XRDevice.trackingOriginMode + "\n";
            text += "\tgetNativePTR:" + XRDevice.GetNativePtr().ToString() + "\n";
            text += "\tgetTrackingSpaceType:" + XRDevice.GetTrackingSpaceType().ToString() + "\n";
            text += "\tdeviceloaded:" + _deviceLoaded + "\n";

            text += "\nXRSettings\n";
            text += "\teyeTextureHeight: " + XRSettings.eyeTextureHeight + "\n";
            text += "\teyeTextureWidth: " + XRSettings.eyeTextureWidth + "\n";
            text += "\teyeTextureResolutionScale: " + XRSettings.eyeTextureResolutionScale + "\n";
            text += "\tisDeviceActive: " + XRSettings.isDeviceActive + "\n";
            text += "\tloadedDeviceName: " + XRSettings.loadedDeviceName + "\n";
            text += "\tocclusionMaskScale: " + XRSettings.occlusionMaskScale + "\n";
            text += "\trenderViewportScale: " + XRSettings.renderViewportScale + "\n";
            text += "\tsupportedDevices: ";
            foreach (string device in XRSettings.supportedDevices)
            {
                text += "[" + device + "]";
            }
            text += "\tdeviceEyeTextureDimension:" + XRSettings.deviceEyeTextureDimension.ToString() + "\n";
            text += "\tgmeViewRenderMode:" + XRSettings.gameViewRenderMode.ToString() + "\n";
            text += "\tuseOcclusionMesh:" + XRSettings.useOcclusionMesh.ToString() + "\n";
            text += "\tloadedDeviceName:" + XRSettings.loadedDeviceName + "\n";

            text += "\nInputTracking\n";

            text += "\tgetLocalPos:" + $"{InputTracking.GetLocalPosition(XRNode.CenterEye)}" + "\n";
            text += "\tgetLocalRot:" + $"{InputTracking.GetLocalRotation(XRNode.CenterEye)}" + "\n";
            //text += "\tgetNodeName:" + $"{InputTracking.GetNodeName(XRNode.CenterEye)}" + "\n";

            text += "\n";
#else
            text += "XRDevice\n";
            text += "\tisPresent: " + VRDevice.isPresent + "\n";
            text += "\tmodel: " + VRDevice.model + "\n";
            text += "\trefreshRate: " + XRDevice.refreshRate + "\n";
            text += "\tuserPresence: " + XRDevice.userPresence + "\n";
            text += "\tfovZoomFactor: " + XRDevice.fovZoomFactor + "\n";

            text += "\nVRSettings\n";
            text += "\teyeTextureHeight: " + VRSettings.eyeTextureHeight + "\n";
            text += "\teyeTextureWidth: " + VRSettings.eyeTextureWidth + "\n";
            text += "\teyeTextureResolutionScale: " + VRSettings.eyeTextureResolutionScale + "\n";
            text += "\tisDeviceActive: " + VRSettings.isDeviceActive + "\n";
            text += "\tloadedDeviceName: " + VRSettings.loadedDeviceName + "\n";
            text += "\tocclusionMaskScale: " + VRSettings.occlusionMaskScale + "\n";
            text += "\trenderViewportScale: " + VRSettings.renderViewportScale + "\n";
            text += "\tsupportedDevices: ";
            foreach(string device in VRSettings.supportedDevices)
            {
                text += "[" + device + "]";
            }
            text += "\n";
#endif
            textField.text = text;
        }
    }

}
