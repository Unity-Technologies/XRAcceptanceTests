using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine.UIElements;
using YamatoJobDefinition;

namespace Unity.PerformanceTesting
{
    [System.Serializable]
    public enum CaptureScope
    {
        ByFrame,
        BeginningOfTest,
        EndOfTest
    }

    [System.Serializable]
    public class FrameTimerTest
    {
        public bool Enabled;
        public string TestName;
        public int FrameCount = 1000;
    }

    [System.Serializable]
    public class EnterPlaymodeTest
    {
        public bool Enabled;
        public string TestName;
    }

    [System.Serializable]
    public class ExitPlaymodeTest
    {
        public bool Enabled;
        public string TestName;
    }

    [System.Serializable]
    public class MeasureScopeTest
    {
        public bool Enabled;
        public string TestName;
    }

    [System.Serializable]
    public class MemoryUsageTest
    {
        public bool Enabled;
        public string TestName;
    }

    [System.Serializable]
    public class SceneSetup
    {
        [HideInInspector] public string Scene;
        [HideInInspector] public string ScenePath;
        [HideInInspector] public bool GenerateTestCase = false;
        [HideInInspector] public CaptureScope CaptureScope;
        public FrameTimerTest FrameTimerTest;
        public MeasureScopeTest MeasureScopeTest;
        public MemoryUsageTest MemoryUsageTest;
        public EnterPlaymodeTest EnterPlaymodeTest;
        public ExitPlaymodeTest ExitPlaymodeTest;
        [HideInInspector] public bool GenerateProfilerTest;
        [HideInInspector] public List<string> ProfilerMarkers;
    }

    [CustomPropertyDrawer(typeof(FrameTimerTest))]
    public class FrameTimerTestPropertyDrawer : PropertyDrawer
    {
        float seperation = EditorGUIUtility.singleLineHeight + 5;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var enabledProp = property.FindPropertyRelative("Enabled");
            if (enabledProp != null)
            {
                var frameCountProp = property.FindPropertyRelative("FrameCount");
                var testNameProp = property.FindPropertyRelative("TestName");
                if (frameCountProp != null)
                {
                    EditorGUI.PropertyField(
                        position: new Rect(position.x + 20, position.y, position.width, height: EditorGUIUtility.singleLineHeight),
                        property: enabledProp,
                        label: new GUIContent("Playmode: FrameTime"),
                        includeChildren: false);
                    var showExtraFields = enabledProp.boolValue;
                    
                    if (showExtraFields)
                    {
                        if (testNameProp != null)
                        {
                            EditorGUIUtility.labelWidth = position.width * .3f;
                            EditorGUI.PropertyField(
                                position: new Rect(position.x + 40, position.y + seperation, position.width, height: EditorGUIUtility.singleLineHeight),
                                property: testNameProp,
                                label: new GUIContent("Test Name"),
                                includeChildren: false);
                        }
                        if (frameCountProp != null)
                        {
                            EditorGUIUtility.labelWidth = position.width * .3f;
                            EditorGUI.PropertyField(
                                position: new Rect(position.x + 40, position.y + (seperation * 2), position.width, height: EditorGUIUtility.singleLineHeight),
                                property: frameCountProp,
                                label: new GUIContent("# of frames"),
                                includeChildren: false);
                        }
                    }
                }
            }
            EditorGUI.EndProperty();
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var isFieldExpandedProp = property.FindPropertyRelative("Enabled");

            var propertyHeight =
                EditorGUI.GetPropertyHeight(isFieldExpandedProp, true);

            var spacing = 5;

            if (isFieldExpandedProp.boolValue)
                return (propertyHeight + spacing) * 3; // The three here is for the number of fields that will be visible when the options are expanded
            else
                return propertyHeight + spacing;
        }
    }
    
    [CustomPropertyDrawer(typeof(EnterPlaymodeTest))]
    public class EnterPlaymodeTestPropertyDrawer : PropertyDrawer
    {
        float seperation = EditorGUIUtility.singleLineHeight + 5;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            var enabledProp = property.FindPropertyRelative("Enabled");
            if (enabledProp != null)
            {
                EditorGUI.PropertyField(
                    position: new Rect(position.x + 20, position.y, Screen.width * .9f,
                        height: EditorGUIUtility.singleLineHeight),
                    property: enabledProp,
                    label: new GUIContent("Editor: EnterPlaymode"),
                    includeChildren: false);

                var showExtraFields = enabledProp.boolValue;

                if (showExtraFields)
                {
                    var testNameProp = property.FindPropertyRelative("TestName");
                    if (testNameProp != null)
                    {
                        EditorGUIUtility.labelWidth = position.width * .3f;
                        EditorGUI.PropertyField(
                            position: new Rect(position.x + 40, position.y + seperation, position.width, height: EditorGUIUtility.singleLineHeight),
                            property: testNameProp,
                            label: new GUIContent("Test Name"),
                            includeChildren: false);
                    }
                }
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var enabledProp = property.FindPropertyRelative("Enabled");

            var propertyHeight =
                EditorGUI.GetPropertyHeight(enabledProp, true);

            var spacing = 5;

            if (enabledProp.boolValue)
                return (propertyHeight + spacing) * 2; // The two here is for the number of fields that will be visible when the options are expanded
            else
                return propertyHeight + spacing;
        }
    }

    [CustomPropertyDrawer(typeof(ExitPlaymodeTest))]
    public class ExitPlaymodeTestPropertyDrawer : PropertyDrawer
    {
        float seperation = EditorGUIUtility.singleLineHeight + 5;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var enabledProp = property.FindPropertyRelative("Enabled");
            if (enabledProp != null)
            {
                EditorGUI.PropertyField(
                    position: new Rect(position.x + 20, position.y, position.width,
                        height: EditorGUIUtility.singleLineHeight),
                    property: enabledProp,
                    label: new GUIContent("Editor: ExitPlaymode"),
                    includeChildren: false);

                var showExtraFields = enabledProp.boolValue;

                if (showExtraFields)
                {
                    var testNameProp = property.FindPropertyRelative("TestName");
                    if (testNameProp != null)
                    {
                        EditorGUIUtility.labelWidth = position.width * .3f;
                        EditorGUI.PropertyField(
                            position: new Rect(position.x + 40, position.y + seperation, position.width, height: EditorGUIUtility.singleLineHeight),
                            property: testNameProp,
                            label: new GUIContent("Test Name"),
                            includeChildren: false);
                    }
                }
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var enabledProp = property.FindPropertyRelative("Enabled");

            var propertyHeight =
                EditorGUI.GetPropertyHeight(enabledProp, true);

            var spacing = 5;

            if (enabledProp.boolValue)
                return (propertyHeight + spacing) * 2; // The two here is for the number of fields that will be visible when the options are expanded
            else
                return propertyHeight + spacing;
        }
    }

    [CustomPropertyDrawer(typeof(MeasureScopeTest))]
    public class MeasureScopeTestPropertyDrawer : PropertyDrawer
    {
        float seperation = EditorGUIUtility.singleLineHeight + 5;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var enabledProp = property.FindPropertyRelative("Enabled");
            if (enabledProp != null)
            {
                EditorGUI.PropertyField(
                    position: new Rect(position.x + 20, position.y, position.width,
                        height: EditorGUIUtility.singleLineHeight),
                    property: enabledProp,
                    label: new GUIContent("Playmode: MeasureScope"),
                    includeChildren: false);

                var showExtraFields = enabledProp.boolValue;

                if (showExtraFields)
                {
                    var testNameProp = property.FindPropertyRelative("TestName");
                    if (testNameProp != null)
                    {
                        EditorGUIUtility.labelWidth = position.width * .3f;
                        EditorGUI.PropertyField(
                            position: new Rect(position.x + 40, position.y + seperation, position.width, height: EditorGUIUtility.singleLineHeight),
                            property: testNameProp,
                            label: new GUIContent("Test Name"),
                            includeChildren: false);
                    }
                }
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var enabledProp = property.FindPropertyRelative("Enabled");

            var propertyHeight =
                EditorGUI.GetPropertyHeight(enabledProp, true);

            var spacing = 5;

            if (enabledProp.boolValue)
                return (propertyHeight + spacing) * 2; // The two here is for the number of fields that will be visible when the options are expanded
            else
                return propertyHeight + spacing;
        }
    }

    [CustomPropertyDrawer(typeof(MemoryUsageTest))]
    public class MemoryUsageTestPropertyDrawer : PropertyDrawer
    {
        float seperation = EditorGUIUtility.singleLineHeight + 5;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            
            var enabledProp = property.FindPropertyRelative("Enabled");
            if (enabledProp != null)
            {
                EditorGUI.PropertyField(
                    position: new Rect(position.x + 20, position.y, position.width,
                        height: EditorGUIUtility.singleLineHeight),
                    property: enabledProp,
                    label: new GUIContent("Playmode: MemoryUsageTest", "Add a Playmode test to the scene to measure allocated memory."),
                    includeChildren: false);

                var showExtraFields = enabledProp.boolValue;

                if (showExtraFields)
                {
                    var testNameProp = property.FindPropertyRelative("TestName");
                    if (testNameProp != null)
                    {
                        EditorGUIUtility.labelWidth = position.width * .3f;
                        EditorGUI.PropertyField(
                            position: new Rect(position.x + 40, position.y + seperation, position.width, height: EditorGUIUtility.singleLineHeight),
                            property: testNameProp,
                            label: new GUIContent("Test Name"),
                            includeChildren: false);
                    }
                }
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var enabledProp = property.FindPropertyRelative("Enabled");

            var propertyHeight =
                EditorGUI.GetPropertyHeight(enabledProp, true);

            var spacing = 5;

            if (enabledProp.boolValue)
                return (propertyHeight + spacing )* 2; // The two here is for the number of fields that will be visible when the options are expanded
            else
                return propertyHeight + spacing;
        }
    }

    [Serializable]
    public class PerformanceBootstrapperConfig : ScriptableObject // Not fixed naming
    {
        public List<SceneSetup> Scenes;
        public List<string> EnabledScenesInBuild;
        public YamatoJobDefinitionObject jobDefinition;

        private YamatoJobDefinitionObject CreateJobDefinitionObject()
        {
            var yamatoJob = new YamatoJobDefinitionObject();
            yamatoJob.platforms = new Platforms();
            yamatoJob.platforms.windows = true;
            yamatoJob.fileName = "performance-tests.yml";
            yamatoJob.unityVersion = "trunk";
            return yamatoJob;
        }

        public void OnEnable ()
        {
            if (jobDefinition == null)
            {
                jobDefinition = CreateJobDefinitionObject();
            }
        }
        
        public void GetScenesForPerfTest()
        {
            Scenes = GetScenesForPerfTest(string.Empty);
        }

        private List<SceneSetup> GetScenesForPerfTest(string directory)
        {
            List<SceneSetup> scenesInBuild = new List<SceneSetup>();
            EnabledScenesInBuild = new List<string>();
            foreach (var scene in EditorBuildSettings.scenes.Where(s => s.enabled))
            {
                EnabledScenesInBuild.Add(scene.path);
            }

            if (EnabledScenesInBuild.Any())
            {
                foreach (var scene in EnabledScenesInBuild)
                {
                    var sceneDirectory = Path.GetDirectoryName(scene);
                    if (sceneDirectory != null && sceneDirectory.Contains("Scenes"))
                    {
                        var ss = new SceneSetup();
                        ss.Scene = Path.GetFileNameWithoutExtension(scene);
                        ss.ScenePath = scene;
                        ss.FrameTimerTest = new FrameTimerTest
                        {
                            TestName = PerformanceTestCreator.GetPlaymodePeformanceTestName(
                                Path.GetFileNameWithoutExtension(scene), 
                                PerformanceTestCreator.FrameRateTestNameSuffix)
                        };
                        ss.MeasureScopeTest = new MeasureScopeTest
                        {
                            TestName = PerformanceTestCreator.GetPlaymodePeformanceTestName(
                                Path.GetFileNameWithoutExtension(scene),
                                PerformanceTestCreator.MeasureScopeTestNameSuffix)
                        };
                        ss.MemoryUsageTest = new MemoryUsageTest
                        {
                            TestName = PerformanceTestCreator.GetPlaymodePeformanceTestName(
                                Path.GetFileNameWithoutExtension(scene),
                                PerformanceTestCreator.MemoryTestNameSuffix)
                        };
                        ss.EnterPlaymodeTest = new EnterPlaymodeTest
                        {
                            TestName = PerformanceTestCreator.GetPlaymodePeformanceTestName(
                                Path.GetFileNameWithoutExtension(scene),
                                PerformanceTestCreator.EnterPlaymodeTestNameSuffix)
                        };
                        ss.ExitPlaymodeTest = new ExitPlaymodeTest
                        {
                            TestName = PerformanceTestCreator.GetPlaymodePeformanceTestName(
                                Path.GetFileNameWithoutExtension(scene),
                                PerformanceTestCreator.ExitPlaymodeTestNameSuffix)
                        };
                        scenesInBuild.Add(ss);
                    }
                }
            }
            return scenesInBuild;
        }
    }
    
    [CustomEditor(typeof(PerformanceBootstrapperConfig))]
    public class PerformanceBootstrapperConfigEditor : UnityEditor.Editor
    {
        private SerializedProperty m_Scenes;
        private ReorderableList m_ReorderableList;
        private SerializedProperty jobDefinition;
        
        private void OnEnable()
        {
            m_Scenes = serializedObject.FindProperty("Scenes");

            m_ReorderableList = new ReorderableList(serializedObject: serializedObject, elements: m_Scenes, draggable: false, displayHeader: true, displayAddButton: false, displayRemoveButton: false);

            m_ReorderableList.drawHeaderCallback = DrawHeaderCallback;

            m_ReorderableList.drawElementCallback = DrawElementCallback;

            m_ReorderableList.elementHeightCallback += ElementHeightCallback;

            m_ReorderableList.onAddCallback += OnAddCallback;

            jobDefinition = serializedObject.FindProperty("jobDefinition");
        }

        private void DrawHeaderCallback(Rect rect)
        {
            GUIStyle headerStyle = new GUIStyle(EditorStyles.label);
            GUIStyle description = new GUIStyle(EditorStyles.helpBox);
            headerStyle.fontStyle = FontStyle.Bold;
            EditorGUI.LabelField(rect, "Scenes for testing", headerStyle);
        }

        private void DrawElementCallback(Rect rect, int index, bool isactive, bool isfocused)
        {
            SerializedProperty element = m_ReorderableList.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;

            SerializedProperty elementName = element.FindPropertyRelative("Scene");
            string elementTitle = string.IsNullOrEmpty(elementName.stringValue)
                ? "New Test Scene"
                : $"Scene Name: {elementName.stringValue}"; 

            bool isFPSExpanded = element.FindPropertyRelative("FrameTimerTest").FindPropertyRelative("Enabled").boolValue;

            EditorGUI.PropertyField(
                position: new Rect(rect.x += 10, rect.y, Screen.width * .5f, height: EditorGUIUtility.singleLineHeight), 
                property: element, 
                label: new GUIContent(elementTitle), 
                includeChildren: true);
        }

        private float ElementHeightCallback(int index)
        {
            float propertyHeight =
                EditorGUI.GetPropertyHeight(m_ReorderableList.serializedProperty.GetArrayElementAtIndex(index), true);

            float spacing = EditorGUIUtility.singleLineHeight / 2;

            return propertyHeight + spacing;
        }

        private void OnAddCallback(ReorderableList list)
        {
            var index = list.serializedProperty.arraySize;
            list.serializedProperty.arraySize++;
            list.index = index;
            var element = list.serializedProperty.GetArrayElementAtIndex(index);
        }

        public override void OnInspectorGUI()
        {
            // is the target destroyed?
            if (serializedObject.targetObject == null)
            {
                GUILayout.Label("No target object- close and reopen this window."); // This happens when entering and then exiting playmode - seems that the target is lost but don't know how to make this persistent. See https://www.reddit.com/r/Unity3D/comments/3ts0oi/serializedobject_target_has_been_destroyed/
            }
            else if (serializedObject != null)
            {
                serializedObject.Update();
                m_ReorderableList.DoLayoutList();
                serializedObject.ApplyModifiedProperties();
                EditorGUILayout.PropertyField(jobDefinition);
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
