using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasTextDisplay : MonoBehaviour {

    public TextMesh m_TextMesh;

    public float m_FadeTime = 1.0f;
    public float m_Timer = 0.0f;
    public Color m_DisableColor;
    public Color m_EnableColor;

    private bool m_State = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

        m_Timer += m_State ? Time.deltaTime : -Time.deltaTime;

        m_Timer = Mathf.Clamp(m_Timer, 0.0f, m_FadeTime);

        if (m_TextMesh != null)
        {
            m_TextMesh.color = Color.Lerp(m_DisableColor, m_EnableColor, Mathf.Clamp01(m_Timer / m_FadeTime));
        }
        
    }

    public void OnEnter()
    {
        m_State = true;

    }

    public void OnExit()
    {
        m_State = false;
    }
}
