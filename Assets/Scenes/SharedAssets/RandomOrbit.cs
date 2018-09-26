using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomOrbit : MonoBehaviour {

    public GameObject orbitTarget;
    public Vector3 eulerFactors = Vector3.one;
    public float velocityMagnitude;
    public Vector3 offsetScale = Vector3.one;
    public float orbitRadius;
    
    private Vector3 m_Euler;
    private Vector3 m_EulerVelocity;
    private Vector3 m_PosePosition;
    
	// Use this for initialization
	void Start () {
        m_EulerVelocity = Vector3.forward * velocityMagnitude;
        m_PosePosition = transform.position;
        UpdatePosition();
	}
	
    private void UpdatePosition()
    {
        m_EulerVelocity = velocityMagnitude * eulerFactors;
        
        m_Euler += m_EulerVelocity * Time.deltaTime;

        Quaternion rot = Quaternion.Euler(m_Euler);
        Vector3 offset = Vector3.Scale((rot * Vector3.forward) * orbitRadius, offsetScale);

        if (orbitTarget != null)
        {
            transform.position = orbitTarget.transform.position + offset;
        }
        else
        {
            transform.position = m_PosePosition + offset;
        }
    }

	// Update is called once per frame
	void Update () {
        UpdatePosition();
	}
}
