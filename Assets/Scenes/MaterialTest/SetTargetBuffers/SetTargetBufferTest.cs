using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTargetBufferTest : MonoBehaviour {
    public Camera depthCamera;
    public Camera colorCamera;
    public Camera clearCamera;
    
    public RenderTexture outputRT;
    private RenderTexture m_UnusedRT;        //Used as a throw-away color buffer when rendering the Depth Camera
    
    void Start () {
        
        if(outputRT != null)
        {
            m_UnusedRT = new RenderTexture(outputRT.width, outputRT.height, 0, RenderTextureFormat.ARGB32);

            //Clear output buffers
            clearCamera.SetTargetBuffers(outputRT.colorBuffer, outputRT.depthBuffer);
            clearCamera.depthTextureMode = DepthTextureMode.Depth;
            clearCamera.depth = Camera.main.depth - 3;

            //Draw to depth buffer, sending color buffer to the unused render texture which will be discarded.
            depthCamera.SetTargetBuffers(m_UnusedRT.colorBuffer, outputRT.depthBuffer);
            depthCamera.depthTextureMode = DepthTextureMode.Depth;
            depthCamera.depth = Camera.main.depth - 2;

            //Draw to color and depth buffer without writing depth values
            colorCamera.SetTargetBuffers(outputRT.colorBuffer, outputRT.depthBuffer);
            colorCamera.depthTextureMode = DepthTextureMode.None;
            colorCamera.depth = Camera.main.depth - 1;
        }
	}
	
}
