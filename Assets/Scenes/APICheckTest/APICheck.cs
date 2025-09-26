using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;


public class APICheck : MonoBehaviour {

    public UnityEngine.UI.Text textField;
    
	void Update () {
        
        if (textField != null)
        {

            string text = "";

            text += "XRDevice\n";
            text += "\trefreshRate: " + XRDevice.refreshRate + "\n";
            text += "\tfovZoomFactor: " + XRDevice.fovZoomFactor + "\n";

            text += "\nXRSettings\n";
            text += "\teyeTextureHeight: " + XRSettings.eyeTextureHeight + "\n";
            text += "\teyeTextureWidth: " + XRSettings.eyeTextureWidth + "\n";
            text += "\teyeTextureResolutionScale: " + XRSettings.eyeTextureResolutionScale + "\n";
            text += "\tisDeviceActive: " + XRSettings.isDeviceActive + "\n";
            text += "\tloadedDeviceName: " + XRSettings.loadedDeviceName + "\n";
            text += "\tocclusionMaskScale: " + XRSettings.occlusionMaskScale + "\n";
            text += "\trenderViewportScale: " + XRSettings.renderViewportScale + "\n";
            text += "\tsupportedDevices: ";
            foreach(string device in XRSettings.supportedDevices)
            {
                text += "[" + device + "]";
            }
            text += "\n";
    
            textField.text = text;
        }
    }

}
