using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RealtimeLight : MonoBehaviour {
    public new Light light { get; private set; }
    //public Mesh textMesh;
    public Billboard lightHalo { get; private set; }
    public GameObject text;
    private Material m_LightHaloMaterial;

    public float maxIntensity { get; private set; }
    
    public float intensity = 0.0f;

    private Color m_PoseHaloColor;

    private void OnDisable()
    {
        if(text != null)
        {
            text.SetActive(false);
        }
    }

    private void OnEnable()
    {
        if(text != null)
        {
            text.SetActive(true);
        }
    }

    public void SetLocalRotation(Quaternion rotation)
    {
        if(text != null)
        {
            text.transform.localRotation = rotation;
        }
    }

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
            m_LightHaloMaterial.SetColor("Color_13F2995C", haloColor);
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
            m_PoseHaloColor = m_LightHaloMaterial.GetColor("Color_13F2995C");
        }
        SetIntensity(intensity);
    }
    
}
