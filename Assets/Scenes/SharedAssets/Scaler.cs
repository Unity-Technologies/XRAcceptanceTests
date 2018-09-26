using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaler : MonoBehaviour {

    public float scaleDelta = 0.25f;
    public float rate = 1.0f;
    public Vector3 axisScale = Vector3.zero;

    private Vector3 m_PoseScale;

	// Use this for initialization
	void Start () {
        m_PoseScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {

        float scale = Mathf.Sin(Time.time * rate) * 0.5f + 0.5f;

        scale *= scaleDelta;

        transform.localScale = m_PoseScale + Vector3.Scale(new Vector3(scale, scale, scale), axisScale);

	}
}
