using System.Collections.Generic;
using System.Text;
using Unity.PerformanceTesting;
using UnityEditor;
using UnityEngine;
using YamatoJobDefinition;

namespace YamatoJobDefinition
{
    [System.Serializable]
    public class Platforms
    {
        public bool xbox;
        public bool ps4;
        public bool windows;
        public bool osx;

        public bool AnyPlatformsSelected()
        {
            return xbox || ps4 || windows || osx;
        }

        public string ToPlatformsString()
        {
            StringBuilder builder = new StringBuilder();

            if (osx)
            {
                builder.Append("OSX ");
            }
            
            if (windows)
            {
                builder.Append("Windows ");
            }
            
            if (ps4)
            {
                builder.Append("PS4 ");
            }
            
            if (xbox)
            {
                builder.Append("Xbox ");
            }

            return builder.ToString().Trim();
        }
        
    }

    [System.Serializable]
    public class YamatoJobDefinitionObject
    {
        public string unityVersion;
        public string fileName;
        public Platforms platforms;
        public bool jobEnabled;
    }
    
    [CustomPropertyDrawer(typeof(YamatoJobDefinitionObject))]
    public class YamatoJobDefinitionPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (BootstrapWindow.m_Config.Scenes.Count > 0)
            {
                EditorGUI.BeginProperty(position, label, property);

                var platforms = property.FindPropertyRelative("platforms");
                var unityVersion = property.FindPropertyRelative("unityVersion");
                var fileName = property.FindPropertyRelative("fileName");
                var jobEnabled = property.FindPropertyRelative("jobEnabled");

                EditorGUIUtility.labelWidth = 190;

                EditorGUI.PropertyField(
                    position: new Rect(position.x, position.y, Screen.width * .8f, height: EditorGUIUtility.singleLineHeight),
                    property: jobEnabled,
                    label: new GUIContent("Generate Yamato Job Definition"),
                    includeChildren: false);

                if (jobEnabled.boolValue)
                {
                    EditorGUI.PropertyField(
                        position: new Rect(position.x, position.y + 20, Screen.width * .8f,
                            height: EditorGUIUtility.singleLineHeight),
                        property: platforms,
                        label: new GUIContent("Which platforms would you like your tests to run on?"),
                        includeChildren: true);

                    EditorGUIUtility.labelWidth = 500;

                    EditorGUI.PropertyField(
                        position: new Rect(position.x, position.y + 120, Screen.width * .8f,
                            height: EditorGUIUtility.singleLineHeight),
                        property: unityVersion,
                        label: new GUIContent("Which version of Unity would you like to run your tests with (revision, branch, or version)"),
                        includeChildren: false);

                    EditorGUI.PropertyField(
                        position: new Rect(position.x, position.y + 140, Screen.width * .8f,
                            height: EditorGUIUtility.singleLineHeight),
                        property: fileName,
                        label: new GUIContent("Yamato Job Definition File Name"),
                        includeChildren: false);
                }

                EditorGUI.EndProperty();
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var isFieldExpandedProp = property.FindPropertyRelative("jobEnabled");

            var propertyHeight =
                EditorGUI.GetPropertyHeight(isFieldExpandedProp, true);

            var spacing = EditorGUIUtility.singleLineHeight;

            if (isFieldExpandedProp.boolValue)
                return (propertyHeight + spacing) * 5;
            else
                return propertyHeight + spacing;
        }
    }
    
    [CustomPropertyDrawer(typeof(Platforms))]
    public class PlatformPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (BootstrapWindow.m_Config.Scenes.Count > 0)
            {
                EditorGUI.BeginProperty(position, label, property);

                var xbox = property.FindPropertyRelative("xbox");
                var windows = property.FindPropertyRelative("windows");
                var osx = property.FindPropertyRelative("osx");
                var ps4 = property.FindPropertyRelative("ps4");

                EditorGUI.PropertyField(
                    position: new Rect(position.x, position.y, Screen.width * .8f,
                        height: EditorGUIUtility.singleLineHeight),
                    property: osx,
                    label: new GUIContent("OSX"),
                    includeChildren: false);

                EditorGUI.PropertyField(
                    position: new Rect(position.x, position.y + 20, Screen.width * .8f,
                        height: EditorGUIUtility.singleLineHeight),
                    property: ps4,
                    label: new GUIContent("PS4"),
                    includeChildren: false);

                EditorGUI.PropertyField(
                    position: new Rect(position.x, position.y + 40, Screen.width * .8f,
                        height: EditorGUIUtility.singleLineHeight),
                    property: windows,
                    label: new GUIContent("Windows"),
                    includeChildren: false);

                EditorGUI.PropertyField(
                    position: new Rect(position.x, position.y + 60, Screen.width * .8f,
                        height: EditorGUIUtility.singleLineHeight),
                    property: xbox,
                    label: new GUIContent("Xbox One"),
                    includeChildren: false);

                EditorGUI.EndProperty();
            }
        }
    }
}