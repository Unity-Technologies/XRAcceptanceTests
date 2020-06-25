using UnityEngine;
using UnityEngine.EventSystems;

public class VRInput : BaseInput
{
    [SerializeField, Tooltip("The keyboard key treated as the mouse button.")]
    KeyCode m_MouseKeyCode = KeyCode.F;

    public override bool mousePresent
    {
        get { return true; }
    }

    public override Vector2 mouseScrollDelta
    {
        get { return Vector2.zero; }
    }

    public override Vector2 mousePosition
    {
        get { return new Vector2(UnityEngine.XR.XRSettings.eyeTextureWidth / 2f, UnityEngine.XR.XRSettings.eyeTextureHeight / 2f); }
    }

    public override bool GetMouseButton(int button)
    {
        if (button != 0)
            return false;

        if (Application.isMobilePlatform)
        {
#if UNITY_HAS_GOOGLEVR && !UNITY_IOS
            return Input.touches.Length > 0;
#else
            return Input.GetMouseButton(0);
#endif
        }
        return Input.GetKey(m_MouseKeyCode);
    }

    public override bool GetMouseButtonDown(int button)
    {
        if (button != 0)
            return false;

        if (Application.isMobilePlatform)
        {
#if UNITY_HAS_GOOGLEVR && !UNITY_IOS
            foreach(Touch touch in Input.touches)
            {
                if(touch.phase == TouchPhase.Began)
                {
                    return true;
                }
            }
#else
            return Input.GetMouseButtonDown(0);
#endif
        }
        return Input.GetKeyDown(m_MouseKeyCode);
    }

    public override bool GetMouseButtonUp(int button)
    {
        if (button != 0)
            return false;

        if (Application.isMobilePlatform)
        {
#if UNITY_HAS_GOOGLEVR && !UNITY_IOS
            foreach(Touch touch in Input.touches)
            {
                if(touch.phase == TouchPhase.Ended)
                {
                    return true;
                }
            }
#else
            return Input.GetMouseButtonUp(0);
#endif
        }
        return Input.GetKeyUp(m_MouseKeyCode);
    }

    public override bool GetButtonDown(string buttonName)
    {
        return false;
    }

    public override bool touchSupported
    {
        get { return false; }
    }
}
