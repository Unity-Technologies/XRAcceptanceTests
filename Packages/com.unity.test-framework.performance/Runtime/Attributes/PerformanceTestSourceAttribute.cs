using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Unity.PerformanceTesting
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class PerformanceTestSource : Attribute, ITestBuilder
    {
        private string TestSceneRoot { get; set; }

        /// <summary>
        /// This attribute is used to generate test cases for a given performance test.
        /// It searches through the list of scenes added to the build settings and generates
        /// test cases based on scenes in directory specified by the user.
        /// </summary>
        /// <param name="testSceneRootDirectory">The directory to search for test case scenes</param>
        public PerformanceTestSource(string testSceneRootDirectory)
        {
            this.TestSceneRoot = testSceneRootDirectory;
        }

        public IEnumerable<TestMethod> BuildFrom(IMethodInfo method, Test suite)
        {
            List<TestMethod> results = new List<TestMethod>();
            try
            {
                var scenesForPerfTest = GetScenesForPerfTest(this.TestSceneRoot);
                foreach (var testScene in scenesForPerfTest)
                {
                    var test = new TestMethod(method, suite)
                    {
                        parms = new TestCaseParameters(new object[] {testScene})
                    };
                    test.parms.ApplyToTest(test);
                    test.Name = Path.GetFileNameWithoutExtension(testScene.ToString());
                    test.FullName = string.Format("{0}.{1}", test.FullName, test.Name);

                    results.Add(test);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to generate performance test cases!");
                Debug.LogException(ex);
                throw;
            }

            suite.Properties.Set("TestType", this.TestSceneRoot);

            Console.WriteLine("Generated {0} performance test cases.", results.Count);
            return results;
        }

        protected static List<string> GetScenesForPerfTest(string directory)
        {
            List<string> scenesInBuild = new List<string>();
            for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                var sceneDirectory = Path.GetDirectoryName(scenePath);
                if (sceneDirectory != null && sceneDirectory.Contains(directory))
                {
                    scenesInBuild.Add(Path.GetFileNameWithoutExtension(scenePath));
                }
            }

            return scenesInBuild;
        }
    }
}