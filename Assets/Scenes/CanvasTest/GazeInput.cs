using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
#if UNITY_2017_2_OR_NEWER
using UnityEngine.XR;
#else
using UnityEngine.VR;
#endif

[RequireComponent(typeof(MeshRenderer))]
public class GazeInput : MonoBehaviour {
    List<RaycastResult> m_RaycastResults;
    Camera m_Camera;
    EventSystem m_EventSystem;
    MeshRenderer m_MeshRenderer;
    Material m_Material;

    public Color disableColor;
    public Color enableColor;
    public float defaultDistance = 15.0f;

    void Start()
    {
        m_RaycastResults = new List<RaycastResult>();
        m_Camera = Camera.main;
        m_EventSystem = FindObjectOfType<EventSystem>();
        m_MeshRenderer = GetComponent<MeshRenderer>();
        m_Material = m_MeshRenderer.material;
    }

    void Update()
    {
        if (!m_EventSystem)
            return;

        var pointerData = new PointerEventData(m_EventSystem);
#if UNITY_2017_2_OR_NEWER
        pointerData.position = new Vector2(UnityEngine.XR.XRSettings.eyeTextureWidth / 2f, UnityEngine.XR.XRSettings.eyeTextureHeight / 2f);
#else
        pointerData.position = new Vector2(UnityEngine.VR.VSettings.eyeTextureWidth / 2f, UnityEngine.VR.VRSettings.eyeTextureHeight / 2f);
#endif
        m_RaycastResults.Clear();
        m_EventSystem.RaycastAll(pointerData, m_RaycastResults);

        if (m_RaycastResults.Count == 0)
        {
            m_Material.SetColor("_TintColor", disableColor);

            transform.position = m_Camera.transform.position + m_Camera.transform.forward * defaultDistance;
            transform.LookAt(m_Camera.transform);

        }
        else
        {
            m_Material.SetColor("_TintColor", enableColor);

            var closestResult = new RaycastResult { distance = float.MaxValue };
            foreach (var r in m_RaycastResults)
            {
                if (r.distance < closestResult.distance)
                    closestResult = r;
            }

            transform.position = m_Camera.transform.position + m_Camera.transform.forward * (closestResult.distance - 0.1f);
            transform.LookAt(m_Camera.transform);
        }
    }
}
