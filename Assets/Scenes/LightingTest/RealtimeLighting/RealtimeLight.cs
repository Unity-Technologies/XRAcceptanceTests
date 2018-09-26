using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RealtimeLight : MonoBehaviour {
    public new Light light { get; private set; }
    public Mesh textMesh;
    public Billboard lightHalo { get; private set; }
    private Material m_LightHaloMaterial;

    public float maxIntensity { get; private set; }
    
    public float intensity = 0.0f;

    private Color m_PoseHaloColor;

    //SetIntensity between 0 and 1
    public void SetIntensity(float factor)
    {
        if(light != null)
        {
            light.intensity = factor * maxIntensity;
        }
        if(m_LightHaloMaterial != null)
        {
            Color haloColor = m_PoseHaloColor;
            haloColor.a *= factor;
            m_LightHaloMaterial.SetColor("_TintColor", haloColor);
        }
        intensity = factor;
    }
    
    void Start()
    {
        light = GetComponentInChildren<Light>();
        lightHalo = GetComponentInChildren<Billboard>();
        if (light != null)
        {
            maxIntensity = light.intensity;
        }

        if (lightHalo != null) {
            m_LightHaloMaterial = lightHalo.gameObject.GetComponentInChildren<MeshRenderer>().material;
            m_PoseHaloColor = m_LightHaloMaterial.GetColor("_TintColor");
        }
        SetIntensity(intensity);
    }
    
}
