using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour {
    public Transform target;

	// Use this for initialization
	void Start () {
        UpdateOrientation();
    }
	
    public void UpdateOrientation()
    {
        if (target != null)
        {
            transform.LookAt(target);
        }
    }

	// Update is called once per frame
	void Update () {
        UpdateOrientation();

    }
}
