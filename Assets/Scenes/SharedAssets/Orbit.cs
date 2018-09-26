using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour {
    
    public GameObject target;
    public float angularVelocity;
    public Vector3 rotationAxis;

    private float m_Radius;
    private Vector3 m_Direction;
    private float m_AngleOffset = 0.0f;

	// Use this for initialization
	void Start () {
        m_Direction = (transform.position - target.transform.position);
        m_Radius = m_Direction.magnitude;
        m_Direction.Normalize();
    }
	
	// Update is called once per frame
	void Update ()
    {
        m_AngleOffset += Time.deltaTime * angularVelocity;
        transform.position = target.transform.position + Quaternion.AngleAxis(m_AngleOffset, rotationAxis.normalized) * m_Direction * m_Radius;
        
	}
}
