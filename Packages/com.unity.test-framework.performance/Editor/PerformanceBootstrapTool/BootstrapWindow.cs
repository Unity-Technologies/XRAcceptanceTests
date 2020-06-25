using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;
using UnityEditor.IMGUI.Controls;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine.UIElements;
using YamatoJobDefinition;

namespace Unity.PerformanceTesting
{
    public class BootstrapWindow : EditorWindow
    {
        private static int s_windowWidth = 800;
        private static int s_windowHeight = 600;
        private static string s_playmodeTestPath;
        private static string s_editmodeTestPath;
        
        private bool m_testGenerated = false;
        private bool m_generating = false;

        UnityEditor.Editor editor;

        Vector2 scrollPos;

        [SerializeField]
        public static PerformanceBootstrapperConfig m_Config;

        private static string sep = Path.DirectorySeparatorChar.ToString();


        [MenuItem("Window/Analysis/Performance Test Bootstrapper")]
        private static void Init()
        {
            var window = GetWindow<BootstrapWindow>("Perf Test Bootstrapper");
            window.minSize = new Vector2(640, 480);
            window.position.size.Set(s_windowWidth, s_windowHeight);
            window.Show();
        }

        // Currently this generates a new one everytime- would like this to be persistent
        private void LoadConfig()
        {
            m_Config = ScriptableObject.CreateInstance<PerformanceBootstrapperConfig>();
            m_Config.GetScenesForPerfTest();
        }

        private void SaveConfig()
        {
            // save m_Config to a Json file
        }

        private void OnEnable()
        {
            s_playmodeTestPath = $"Assets{sep}PlaymodeTests";
            s_editmodeTestPath = $"Assets{sep}EditmodeTests";
            LoadConfig();
        }

        private void OnDisable()
        {
            
        }

        private void OnGUI()
        {
            EditorStyles.boldLabel.wordWrap = true;
            EditorGUIUtility.labelWidth = position.width * .3f;
            EditorGUILayout.LabelField(
                "Select the type of performance test desired for each scene below, then click the Generate Tests button.", 
                EditorStyles.boldLabel,
                GUILayout.ExpandHeight(true), GUILayout.MaxHeight(40));
            SyncScenesForTestingWithLatestBuiltSettings();
            EditorGUILayout.BeginVertical();
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false);

            if (!editor || editor.serializedObject.targetObject == null)
            {
                editor = null;
                
                if (m_Config == null)
                {
                    LoadConfig();
                }
                editor = UnityEditor.Editor.CreateEditor(m_Config);
            }            
            
            if (editor)
            {
                editor.OnInspectorGUI();
            }

            if (TestScenesPresent())
            {
                if (m_generating)
                {
                    GUILayout.Label($"Tests generating...");
                }
                else if (m_testGenerated)
                {
                    //GUILayout.Label($"Tests have been created in {s_playmodeTestPath} and/ or {s_editmodeTestPath}.");
                    EditorGUILayout.LabelField(
                        $"Tests have been created.\r\nEditor tests can be found in {s_editmodeTestPath}\r\nPlaymode tests can be found in {s_playmodeTestPath}",
                        EditorStyles.boldLabel,
                        GUILayout.ExpandHeight(true), GUILayout.MaxHeight(50));
                }
                if (GUILayout.Button("Generate Tests"))
                {
                    GenerateTests();

                    if (!YamatoConfigManager.YamatoJobDefinitionFolderExists() && m_Config.jobDefinition.jobEnabled)
                    {
                        YamatoConfigManager.CreateYamatoJobDefinitionFolder();
                    }

                    if (!m_Config.jobDefinition.platforms.AnyPlatformsSelected())
                    {
                        return;
                    }

                    YamatoConfigManager.WriteYamatoJobDefinition(m_Config.jobDefinition, s_playmodeTestPath, s_editmodeTestPath);
                }
            }
            else
            {
                GUILayout.Label("No test scenes have been added to the Build Settings.");
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        private void GenerateTests()
        {
            m_testGenerated = false;
            m_generating = true; // Currently don't think this makes any difference because there probably isn't a GUI update during this operation
            foreach (var sceneSetup in m_Config.Scenes.Where(s => s.FrameTimerTest.Enabled))
            {
                PerformanceTestCreator.CreateFrameRatePeformanceTest(
                    s_playmodeTestPath,
                    sceneSetup.ScenePath,
                    sceneSetup.FrameTimerTest.TestName,
                    sceneSetup.FrameTimerTest.FrameCount);
            }

            foreach (var sceneSetup in m_Config.Scenes.Where(s => s.MeasureScopeTest.Enabled))
            {
                PerformanceTestCreator.CreateMeasureScopePeformanceTest(
                    s_playmodeTestPath,
                    sceneSetup.ScenePath,
                    sceneSetup.MeasureScopeTest.TestName);
            }

            foreach (var sceneSetup in m_Config.Scenes.Where(s => s.MemoryUsageTest.Enabled))
            {
                PerformanceTestCreator.CreateMemoryPeformanceTest(
                    s_playmodeTestPath,
                    sceneSetup.ScenePath,
                    sceneSetup.MemoryUsageTest.TestName);
            }

            foreach (var sceneSetup in m_Config.Scenes.Where(s => s.EnterPlaymodeTest.Enabled))
            {
                PerformanceTestCreator.CreateEnterPlaymodePeformanceTest(
                    s_editmodeTestPath,
                    sceneSetup.ScenePath,
                    sceneSetup.EnterPlaymodeTest.TestName);
            }

            foreach (var sceneSetup in m_Config.Scenes.Where(s => s.ExitPlaymodeTest.Enabled))
            {
                PerformanceTestCreator.CreateExitPlaymodePeformanceTest(s_editmodeTestPath,
                    sceneSetup.ScenePath,
                    sceneSetup.ExitPlaymodeTest.TestName);
            }
            m_testGenerated = true;
            m_generating = false;
        }

        private static bool TestScenesPresent()
        {
            return m_Config != null && m_Config.Scenes.Count > 0;
        }

        private void SyncScenesForTestingWithLatestBuiltSettings()
        {
            var currentEnabledScenesInBuild =
                EditorBuildSettings.scenes.Any(s => s.enabled)
                    ? EditorBuildSettings.scenes.Where(s => s.enabled).ToList().Select(ebs => ebs.path)
                    : new List<string>();

            if (m_Config.EnabledScenesInBuild.Except(currentEnabledScenesInBuild).ToList().Any()
                || currentEnabledScenesInBuild.Except(m_Config.EnabledScenesInBuild).ToList().Any())
            {
                m_testGenerated = false;
                m_Config.GetScenesForPerfTest();
            }
        }

        void OnInspectorUpdate()
        {
            //Repaint();
        }
    }
}
