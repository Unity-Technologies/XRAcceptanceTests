using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Unity.PerformanceTesting
{
    public class PerformanceTestCreator
    {
        public static readonly string FrameRateTestFileSuffix = "_FrameRateTests";
        public static readonly string FrameRateTestNameSuffix = "_FrameRateTest";
        private static readonly string FrameRateTestTemplate = "FrameRateTestTemplate.txt";

        public static readonly string EnterPlaymodeTestFileSuffix = "_EnterPlaymodeTests";
        public static readonly string EnterPlaymodeTestNameSuffix = "_EnterPlaymodeTest";
        private static readonly string EnterPlaymodeTestTemplate = "EnterPlaymodeTestTemplate.txt";

        public static readonly string ExitPlaymodeTestFileSuffix = "_ExitPlaymodeTests";
        public static readonly string ExitPlaymodeTestNameSuffix = "_ExitPlaymodeTest";
        private static readonly string ExitPlaymodeTestTemplate = "ExitPlaymodeTestTemplate.txt";

        public static readonly string MeasureScopeTestFileSuffix = "_MeasureScopeTests";
        public static readonly string MeasureScopeTestNameSuffix = "_MeasureScopeTest";
        private static readonly string MeasureScopeTestTemplate = "PlaymodeMeasureScopeTemplate.txt";

        public static readonly string MemoryTestFileSuffix = "_MemoryUsageTests";
        public static readonly string MemoryTestNameSuffix = "_MemoryUsageTest";
        private static readonly string MemoryTestTemplate = "PlaymodeAllocatedMemoryTemplate.txt";

        private static readonly string AssemblyCSharpEditorTestableAsmDefName = "Assembly-CSharp-Editor-testable";
        private static readonly string AssemblyCSharpEditorTestableTemplate = "Assembly-CSharp-Editor-testable.asmdef.txt";
        private static readonly string TestWithProfilerIsolation = "TestWithProfilerIsolation.txt";
        private static readonly string FrameDataUtilities = "FrameDataUtilities.txt";

        private static readonly string AsmDefName = "PerformanceTests";
        private static readonly string AssemblyDefinitionTestTemplate = "AsmDefTemplate.asmdef.txt";
        private static readonly string AsmDefFileSuffix = ".asmdef";
        private static readonly string RelPathToTemplates =
            "Packages/com.unity.test-framework.performance/Editor/PerformanceBootstrapTool/Templates";

        private static readonly Dictionary<string, string> AdditionalFiles = new Dictionary<string, string>
        {
            { TestWithProfilerIsolation, ".cs" },
            { FrameDataUtilities, ".cs"}
        };


        public static void CreateFrameRatePeformanceTest(string targetPath, string pathToScene, string testName, int numFrames = 1000)
        {
            CreateTestDirectory(targetPath);
            CreatePerformanceTestAsmDef(targetPath);

            var sceneName = Path.GetFileNameWithoutExtension(pathToScene);

            var testFileName = sceneName + FrameRateTestFileSuffix;

            var templateContent = GetFrameRateContent(pathToScene, numFrames, sceneName, testName);

            templateContent = SetLineEndings(templateContent, EditorSettings.lineEndingsForNewScripts);

            WriteAndSaveTest(testFileName, targetPath, templateContent);

        }

        public static void CreateMeasureScopePeformanceTest(string targetPath, string pathToScene, string testName)
        {
            CreateTestDirectory(targetPath);
            CreatePerformanceTestAsmDef(targetPath);

            var sceneName = Path.GetFileNameWithoutExtension(pathToScene);

            var testFileName = sceneName + MeasureScopeTestFileSuffix;

            var templateContent = GeMeasureScopeContent(pathToScene, sceneName, testName);

            templateContent = SetLineEndings(templateContent, EditorSettings.lineEndingsForNewScripts);

            WriteAndSaveTest(testFileName, targetPath, templateContent);

        }

        public static void CreateMemoryPeformanceTest(string targetPath, string pathToScene, string testName)
        {
            CreateTestDirectory(targetPath);
            CreatePerformanceTestAsmDef(targetPath);

            var sceneName = Path.GetFileNameWithoutExtension(pathToScene);

            var testFileName = sceneName + MemoryTestFileSuffix;

            var templateContent = GetMemoryContent(pathToScene, sceneName, testName);

            templateContent = SetLineEndings(templateContent, EditorSettings.lineEndingsForNewScripts);

            WriteAndSaveTest(testFileName, targetPath, templateContent);
        }

        public static void CreateEnterPlaymodePeformanceTest(string targetPath, string pathToScene, string testName)
        {
            CreateTestDirectory(targetPath);
            CreateAssemblyCSharpEditorTestableAsmDef(targetPath);

            var sceneName = Path.GetFileNameWithoutExtension(pathToScene);

            var testFileName = sceneName + EnterPlaymodeTestFileSuffix;

            var templateContent = GetEnterPlaymodeContent(pathToScene, sceneName, testName);

            templateContent = SetLineEndings(templateContent, EditorSettings.lineEndingsForNewScripts);

            WriteAndSaveTest(testFileName, targetPath,  templateContent, AdditionalFiles);
        }

        public static void CreateExitPlaymodePeformanceTest(string targetPath, string pathToScene, string testName)
        {
            CreateTestDirectory(targetPath);
            CreateAssemblyCSharpEditorTestableAsmDef(targetPath);

            var sceneName = Path.GetFileNameWithoutExtension(pathToScene);

            var testFileName = sceneName + ExitPlaymodeTestFileSuffix;

            var templateContent = GetExitPlaymodeContent(pathToScene, sceneName, testName);

            templateContent = SetLineEndings(templateContent, EditorSettings.lineEndingsForNewScripts);

            WriteAndSaveTest(testFileName, targetPath, templateContent, AdditionalFiles);
        }

        public static string GetPlaymodePeformanceTestName(string sceneName, string suffix, string testName = null)
        {
            string exitPlaymodePeformanceTestName;
            if (sceneName == testName || string.IsNullOrEmpty(testName))
            {
                exitPlaymodePeformanceTestName = sceneName + suffix;
            }
            else
                exitPlaymodePeformanceTestName = testName;
            return exitPlaymodePeformanceTestName;
        }

        private static void WriteAndSaveTest(string testFileName, string targetPath, string templateContent, Dictionary<string, string> additionalFiles = null)
        {
            var fullPath = Path.GetFullPath(targetPath);
            if (Directory.Exists(fullPath))
            {
                var fullTestPath = Path.Combine(fullPath, testFileName + ".cs");
                var utF8Encoding = new UTF8Encoding(true);
                File.WriteAllText(fullTestPath, templateContent, utF8Encoding);

                if (additionalFiles != null)
                {
                    foreach (var additionalFile in additionalFiles)
                    {
                        var sourceFilePath = Path.Combine(RelPathToTemplates, additionalFile.Key);
                        var targetFilePath = Path.Combine(fullPath, additionalFile.Key.Substring(0,additionalFile.Key.Length - 4) + additionalFile.Value);
                        File.Copy(sourceFilePath, targetFilePath, true);
                    }
                }

                AssetDatabase.Refresh();
            }
            else
            {
                Debug.LogError($"Can't create performance test in directory {fullPath} because it doesn't exist.");
            }
        }

        private static void CreatePerformanceTestAsmDef(string testDirectoryLocalPath)
        {
            var fullPath = Path.GetFullPath(testDirectoryLocalPath);

            if (Directory.Exists(fullPath))
            {
                var dirInfo = new DirectoryInfo(fullPath);
                var asmdefFiles = dirInfo.GetFiles("*.asmdef");
                if (asmdefFiles.Length == 0)
                {
                    var templateContent = GetAsmDefContent();

                    templateContent = SetLineEndings(templateContent, EditorSettings.lineEndingsForNewScripts);

                    var fullTestPath = Path.Combine(fullPath, AsmDefName + AsmDefFileSuffix);
                    var utF8Encoding = new UTF8Encoding(true);
                    File.WriteAllText(fullTestPath, templateContent, utF8Encoding);

                    var localPath = Path.Combine(testDirectoryLocalPath, AsmDefName + AsmDefFileSuffix);

                    AssetDatabase.Refresh();
                }
                else
                {
                    Debug.Log(".asmdef already found.");
                }
            }
            else
            {
                Debug.LogError($"Can't create AsmDef in directory {fullPath} because it doesn't exist.");
            }
        }

        private static void CreateAssemblyCSharpEditorTestableAsmDef(string testDirectoryLocalPath)
        {
            var fullPath = Path.GetFullPath(testDirectoryLocalPath);

            if (Directory.Exists(fullPath))
            {
                var dirInfo = new DirectoryInfo(fullPath);
                var asmdefFiles = dirInfo.GetFiles("*.asmdef");
                if (asmdefFiles.Length == 0)
                {
                    var templateContent = GetAssemblyCSharpEditorTestableAsmDefContent();

                    templateContent = SetLineEndings(templateContent, EditorSettings.lineEndingsForNewScripts);

                    var fullTestPath = Path.Combine(fullPath, AssemblyCSharpEditorTestableAsmDefName + AsmDefFileSuffix);
                    var utF8Encoding = new UTF8Encoding(true);
                    File.WriteAllText(fullTestPath, templateContent, utF8Encoding);

                    var localPath = Path.Combine(testDirectoryLocalPath, AsmDefName + AsmDefFileSuffix);

                    AssetDatabase.Refresh();
                }
                else
                {
                    Debug.Log(".asmdef already found.");
                }
            }
            else
            {
                Debug.LogError($"Can't create AsmDef in directory {fullPath} because it doesn't exist.");
            }
        }

        private static string GetAsmDefContent()
        {
            var templateString = File
                .ReadAllText(Path.Combine(RelPathToTemplates, AssemblyDefinitionTestTemplate));
            return templateString.Replace("#SCRIPTNAME#", "Performance Test Assembly");
        }

        private static string GetAssemblyCSharpEditorTestableAsmDefContent()
        {
            var templateString = File
                .ReadAllText(Path.Combine(RelPathToTemplates, AssemblyCSharpEditorTestableTemplate));
            return templateString;
        }

        private static string GetFrameRateContent(string pathToScene, int numFrames, string sceneName, string testName)
        {
            var templateString = File
                .ReadAllText(Path.Combine(RelPathToTemplates, FrameRateTestTemplate));
            return templateString
                .Replace("#TESTCLASSNAME#", sceneName + FrameRateTestFileSuffix)
                .Replace("#PATHTOSCENE#", pathToScene)
                .Replace("#NUMFRAMES#", numFrames.ToString())
                .Replace("#TESTNAME#", testName);
        }

        private static string GeMeasureScopeContent(string pathToScene, string sceneName, string testName)
        {
            var templateString = File
                .ReadAllText(Path.Combine(RelPathToTemplates, MeasureScopeTestTemplate));
            return templateString
                .Replace("#TESTCLASSNAME#", sceneName + MeasureScopeTestFileSuffix)
                .Replace("#PATHTOSCENE#", pathToScene)
                .Replace("#TESTNAME#", testName);
        }

        private static string GetMemoryContent(string pathToScene, string sceneName, string testName)
        {
            var templateString = File
                .ReadAllText(Path.Combine(RelPathToTemplates, MemoryTestTemplate));
            return templateString
                .Replace("#TESTCLASSNAME#", sceneName + MemoryTestFileSuffix)
                .Replace("#PATHTOSCENE#", pathToScene)
                .Replace("#TESTNAME#", testName);
        }

        private static string GetEnterPlaymodeContent(string pathToScene, string sceneName, string testName)
        {

            var templateString = File
                .ReadAllText(Path.Combine(RelPathToTemplates, EnterPlaymodeTestTemplate));
            return templateString
                .Replace("#TESTCLASSNAME#", sceneName + EnterPlaymodeTestFileSuffix)
                .Replace("#PATHTOSCENE#", pathToScene)
                .Replace("#TESTNAME#", testName);
        }

        private static string GetExitPlaymodeContent(string pathToScene, string sceneName, string testName)
        {
            var templateString = File
                .ReadAllText(Path.Combine(RelPathToTemplates, ExitPlaymodeTestTemplate));
            return templateString
                .Replace("#TESTCLASSNAME#", sceneName + ExitPlaymodeTestFileSuffix)
                .Replace("#PATHTOSCENE#", pathToScene)
                .Replace("#TESTNAME#", testName);
        }

        private static bool CreateTestDirectory(string testDirectoryLocalPath)
        {
            var fullPath = Path.GetFullPath(testDirectoryLocalPath);
            Directory.CreateDirectory(fullPath);
            return Directory.Exists(fullPath); // true on successful creation
        }

        private static string SetLineEndings(string content, LineEndingsMode lineEndingsMode)
        {
            string replacement;
            switch (lineEndingsMode)
            {
                case LineEndingsMode.OSNative:
                    replacement = Application.platform != RuntimePlatform.WindowsEditor ? "\n" : "\r\n";
                    break;
                case LineEndingsMode.Unix:
                    replacement = "\n";
                    break;
                case LineEndingsMode.Windows:
                    replacement = "\r\n";
                    break;
                default:
                    replacement = "\n";
                    break;
            }
            content = Regex.Replace(content, "\\r\\n?|\\n", replacement);
            return content;
        }
    }
}