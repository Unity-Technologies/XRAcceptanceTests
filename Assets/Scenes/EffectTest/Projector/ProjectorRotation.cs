using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectorRotation : MonoBehaviour {
    
    public float sinusoidalVelocity;
    public float sinusoidalMaxRotation;
    public Transform parentTransform;
    
	void Start () {
	}
	
    void UpdateRotation()
    {
        if (parentTransform != null)
        {
            float angle = Mathf.Sin(Time.time * sinusoidalVelocity) * sinusoidalMaxRotation;
            Quaternion sinusoidalRotation = Quaternion.AngleAxis(angle, parentTransform.forward);

            transform.localRotation = sinusoidalRotation;
        }
    }
    
	void Update () {
        UpdateRotation();
	}
}
