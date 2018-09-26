using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Line : MonoBehaviour {

    public int pointCount = 16;
    public float spiralFactor = 1f;
    public float spiralLength = 10.0f;
    public float minSpiralFactor = 0.05f;
    public float maxSpiralFactor = 0.1f;
    public float animationSpeed = 2.0f;

    LineRenderer m_LineRenderer;
    Vector3[] m_Points;
    
    public Vector3 GetSphericalSpiralPoint(float a, float t)
    {
        Vector3 point;

        t = (t - 0.5f) * 2.0f;

        float d = t * spiralLength;

        float c = Mathf.Sqrt(1 + (a * a) * (d * d));

        point.x = Mathf.Cos(d) / c;
        point.y = Mathf.Sin(d) / c;
        point.z = -((a * d) / c);

        return point;
    }

    void Awake()
    {
        m_LineRenderer = GetComponent<LineRenderer>();
        
    }

    void Start()
    {
        m_Points = new Vector3[pointCount];
    }

    void Update()
    {
        if(m_Points.Length != pointCount)
        {
            m_Points = new Vector3[pointCount];
        }

        spiralFactor = ((Mathf.Sin(Time.time * animationSpeed) * 0.5f + 0.5f) * (maxSpiralFactor - minSpiralFactor)) + minSpiralFactor;

        float denominator = 1.0f / (float)pointCount;
        for(int i = 0; i < pointCount; i++)
        {
            m_Points[i] = transform.TransformPoint(GetSphericalSpiralPoint(spiralFactor, (float)i * denominator));
        }

        m_LineRenderer.useWorldSpace = true;
        m_LineRenderer.positionCount = pointCount;
        m_LineRenderer.SetPositions(m_Points);
    }
}
