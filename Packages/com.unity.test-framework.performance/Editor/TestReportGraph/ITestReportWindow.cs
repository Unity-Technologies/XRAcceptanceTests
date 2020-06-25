using System;
using System.Collections.Generic;
using Unity.PerformanceTesting.Data;
using UnityEngine;
using UnityEngine.UIElements;

namespace Unity.PerformanceTesting
{
    public interface ITestReportWindow
    {
        Run GetResults();
        void SelectTest(int index);
        void SelectTest(string name);
        void SelectTest(TestResult result);
        bool CheckAndSetupMaterial();
        bool DrawStart(Rect r);
        bool DrawStart(float w, float h, GUIStyle style = null);
        void DrawEnd();
        void DrawBar(float x, float y, float w, float h, Color col);
        void BeginWindows();
        void EndWindows();
        void ShowNotification(GUIContent notification);
        void ShowNotification(GUIContent notification, double fadeoutWait);
        void RemoveNotification();
        void ShowTab();
        void Focus();
        void ShowUtility();
        void ShowPopup();
        void ShowModalUtility();
        void ShowAsDropDown(Rect buttonRect, Vector2 windowSize);
        void Show();
        void Show(bool immediateDisplay);
        void ShowAuxWindow();
        void ShowModal();
        void Close();
        void Repaint();
        bool SendEvent(Event e);
        IEnumerable<Type> GetExtraPaneTypes();
        VisualElement rootVisualElement { get; }
        bool wantsMouseMove { get; set; }
        bool wantsMouseEnterLeaveWindow { get; set; }
        bool autoRepaintOnSceneChange { get; set; }
        bool maximized { get; set; }
        Vector2 minSize { get; set; }
        Vector2 maxSize { get; set; }
        string title { get; set; }
        GUIContent titleContent { get; set; }
        int depthBufferBits { get; set; }
        int antiAlias { get; set; }
        Rect position { get; set; }
        string name { get; set; }
        HideFlags hideFlags { get; set; }
        void SetDirty();
        int GetInstanceID();
        int GetHashCode();
        bool Equals(object other);
        string ToString();
    }
}