using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Translate : MonoBehaviour {

    public float rate;
    public float maxTranslation;
    public Vector3 axis;
    
    private Vector3 m_PosePosition;
    private float m_AnimationTimeOffset;
    
    public bool randomize;

	// Use this for initialization
	void Start () {
        m_PosePosition = transform.position;
        if(randomize)
        {
            m_AnimationTimeOffset = Random.Range(0.0f, 360.0f);
        }
	}
	
	// Update is called once per frame
	void Update () {
        float offset = Mathf.Sin( (Time.time + m_AnimationTimeOffset) * rate) * 0.5f + 0.5f;

        transform.position = ((axis.normalized) * maxTranslation * offset) + m_PosePosition;
	}
}
