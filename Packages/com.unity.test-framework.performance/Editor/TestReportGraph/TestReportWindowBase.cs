using System.Collections.Generic;
using Unity.PerformanceTesting.Data;
using UnityEditor;
using UnityEngine;

namespace Unity.PerformanceTesting
{
    public class TestReportWindowBase : EditorWindow
    {
        protected GUIStyle m_glStyle = null;
        private Material m_material;
        public Color m_colorWhite = new Color(1.0f, 1.0f, 1.0f);
        public Color m_colorBarBackground = new Color(0.5f, 0.5f, 0.5f);
        public Color m_colorBoxAndWhiskerBackground = new Color(0.4f, 0.4f, 0.4f);
        public Color m_colorBar = new Color(0.95f, 0.95f, 0.95f);
        public Color m_colorStandardLine = new Color(1.0f, 1.0f, 1.0f);
        public Color m_colorMedianLine = new Color(0.2f, 0.5f, 1.0f, 0.5f);
        public Color m_colorMedianText = new Color(0.4f, 0.7f, 1.0f, 1.0f);
        public Color m_colorWarningText = Color.red;
        protected Run m_resultsData;
        protected string m_selectedTest;
        private List<string> m_sampleGroups = new List<string>();

        public Run GetResults()
        {
            return m_resultsData;
        }

        public void SelectTest(int index)
        {
            if (index < 0 || index >= m_resultsData.Results.Count)
                return;

            var result = m_resultsData.Results[index];
            SelectTest(result);
        }

        public void SelectTest(string name)
        {
            foreach (var result in m_resultsData.Results)
            {
                if (result.Name == name)
                {
                    SelectTest(result);
                    return;
                }
            }
        }

        public void SelectTest(TestResult result)
        {
            m_selectedTest = result.Name;

            m_sampleGroups.Clear();
            foreach (var sampleGroup in result.SampleGroups)
            {
                m_sampleGroups.Add(sampleGroup.Name);
            }
        }

        public bool CheckAndSetupMaterial()
        {
            if (m_material == null)
                m_material = new Material(Shader.Find("Unlit/TestReportShader"));

            if (m_material == null)
                return false;

            return true;
        }

        public bool DrawStart(Rect r)
        {
            if (Event.current.type != EventType.Repaint)
                return false;

            if (!CheckAndSetupMaterial())
                return false;

            GL.PushMatrix();
            CheckAndSetupMaterial();
            m_material.SetPass(0);

            Matrix4x4 matrix = new Matrix4x4();
            matrix.SetTRS(new Vector3(r.x, r.y, 0), Quaternion.identity, Vector3.one);
            GL.MultMatrix(matrix);
            return true;
        }

        public bool DrawStart(float w, float h, GUIStyle style = null)
        {
            Rect r = GUILayoutUtility.GetRect(w, h, style == null ? m_glStyle : style);
            return DrawStart(r);
        }

        public void DrawEnd()
        {
            GL.PopMatrix();
        }

        public void DrawBar(float x, float y, float w, float h, Color col)
        {
            GL.Begin(GL.TRIANGLE_STRIP);
            GL.Color(col);
            GL.Vertex3(x, y, 0);
            GL.Vertex3(x + w, y, 0);
            GL.Vertex3(x, y + h, 0);
            GL.Vertex3(x + w, y + h, 0);
            GL.End();
        }
    }
}