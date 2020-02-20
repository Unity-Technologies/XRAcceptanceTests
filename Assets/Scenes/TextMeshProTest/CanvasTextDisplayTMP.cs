using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasTextDisplayTMP : MonoBehaviour
{

    public TextMeshPro textMesh;

    public float fadeTime = 1.0f;
    public float timer = 0.0f;
    public Color disableColor;
    public Color enableColor;

    bool m_State = false;

    void Update()
    {

        timer += m_State ? Time.deltaTime : -Time.deltaTime;
        timer = Mathf.Clamp(timer, 0.0f, fadeTime);

        if (textMesh != null)
        {
            textMesh.color = Color.Lerp(disableColor, enableColor, Mathf.Clamp01(timer / fadeTime));
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
