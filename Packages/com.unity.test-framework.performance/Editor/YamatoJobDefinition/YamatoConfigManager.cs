using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework.Internal;
using UnityEngine;

namespace YamatoJobDefinition
{
    public static class YamatoConfigManager
    {
        private static string GitFolderName = ".git";
        private static string YamatoFolderName = ".yamato";
        private static string sep = Path.DirectorySeparatorChar.ToString();

        private static string _templatePath =
            $"Packages{sep}com.unity.test-framework.performance{sep}Editor{sep}YamatoJobDefinition{sep}Templates{sep}template.yml";

        public static bool YamatoJobDefinitionFolderExists()
        {
            var projectRootPath = GetProjectRoot();

            if (projectRootPath != null)
            {
                var di = new DirectoryInfo(projectRootPath);
                return ContainsFolder(di, YamatoFolderName);
            }

            return false;
        }

        public static DirectoryInfo CreateYamatoJobDefinitionFolder()
        {
            var projectRootPath = GetProjectRoot();
            var di = new DirectoryInfo(projectRootPath);

            if (!ContainsFolder(di, YamatoFolderName))
            {
                return di.CreateSubdirectory(YamatoFolderName);
            }

            return new DirectoryInfo($"{projectRootPath}{Path.DirectorySeparatorChar.ToString()}{YamatoFolderName}");
        }

        public static void WriteYamatoJobDefinition(YamatoJobDefinitionObject definition, string playModeTestsPath,
            string editModeTestsPath)
        {
            var yamatoTemplate = File.ReadAllText(Path.GetFullPath(_templatePath));
            var definitionFileName = definition.fileName.EndsWith(".yml")
                ? definition.fileName
                : $"{definition.fileName}.yml";

            yamatoTemplate = yamatoTemplate.Replace("<<UNITY_VERSION>>", definition.unityVersion);
            yamatoTemplate = yamatoTemplate.Replace("<<FILENAME>>", definitionFileName);
            yamatoTemplate = yamatoTemplate.Replace("<<TARGET_PLATFORMS>>", definition.platforms.ToPlatformsString());
            yamatoTemplate = yamatoTemplate.Replace("<<TESTS_LOCATION>>", GetRelativeTestsPath());
            yamatoTemplate = yamatoTemplate.Replace("<<PERFORMANCE_PROJECT_ID>>", GetProjectName());
            yamatoTemplate =
                yamatoTemplate.Replace("<<TARGET_MODES>>", GetTargetModes(playModeTestsPath, editModeTestsPath));

            var yamatoJobDefinitionFolder = CreateYamatoJobDefinitionFolder();

            var fullPath = Path.Combine(yamatoJobDefinitionFolder.FullName, definitionFileName);
            File.WriteAllText(fullPath, yamatoTemplate);
        }

        private static string GetRelativeTestsPath()
        {
            var di = new DirectoryInfo(Directory.GetCurrentDirectory());
            var root = new DirectoryInfo(GetProjectRoot());
            var testsRelativePath = di.Name;
            if (di.Name.Equals(root.Name))
            {
                return ".";
            }

            while (di.Parent != null && !di.Parent.Name.Equals(root.Name))
            {
                di = di.Parent;
                testsRelativePath = Path.Combine(di.Name, testsRelativePath);
            }

            return testsRelativePath;
        }

        public static string GetProjectRoot()
        {
            DirectoryInfo currentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());

            var rootDirectoryFound = ContainsFolder(currentDirectory, GitFolderName);

            while (currentDirectory.Parent != null && !rootDirectoryFound)
            {
                currentDirectory = currentDirectory.Parent;
                rootDirectoryFound = ContainsFolder(currentDirectory, GitFolderName);
            }

            return rootDirectoryFound ? currentDirectory.FullName : null;
        }

        public static bool TestsExist(string path)
        {
            if (!Directory.Exists(path))
            {
                return false;
            }

            var di = new DirectoryInfo(path);

            var asmdefFiles = di.GetFiles("*.asmdef");

            if (asmdefFiles.Length == 0)
            {
                return false;
            }

            var testFiles = di.GetFiles("*.cs");

            return testFiles.Length != 0;
        }

        public static string GetTargetModes(string playModeTestsPath, string editModeTestsPath)
        {
            var playModeTestsExist = TestsExist(playModeTestsPath);
            var editModeTestsExist = TestsExist(editModeTestsPath);

            StringBuilder builder = new StringBuilder();

            if (playModeTestsExist)
            {
                builder.Append("playmode ");
            }

            if (editModeTestsExist)
            {
                builder.Append("editor ");
            }

            return builder.ToString().Trim();
        }

        private static bool ContainsFolder(DirectoryInfo di, string folderName)
        {
            return Directory.Exists($"{di.FullName}{Path.DirectorySeparatorChar.ToString()}{folderName}");
        }
        
        public static string GetProjectName()
        {
            string[] s = Application.dataPath.Split('/');
            string projectName = s[s.Length - 2];
            return projectName;
        }
    }
}