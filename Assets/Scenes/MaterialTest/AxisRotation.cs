using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisRotation : MonoBehaviour {
    public Vector3 axis;
    public float angularSpeed;
    public bool isLocal = false;
    
    private void Update()
    {
        if (isLocal)
        {
            transform.localRotation = Quaternion.AngleAxis(angularSpeed * Time.time, axis.normalized);
        }
        else
        {
            transform.rotation = Quaternion.AngleAxis(angularSpeed * Time.deltaTime, axis.normalized) * transform.rotation;
        }
    }


}
