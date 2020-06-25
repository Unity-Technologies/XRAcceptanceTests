using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PerformanceTesting;
using UnityEditor.Profiling;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Profiling;

public class TestWithProfilerIsolation
{
    protected static readonly string k_EnterPlayModeMarker = "EnterPlayMode";
    protected static readonly string k_ReloadAssembliesMarker = "ReloadAssemblies";
    protected static readonly string k_BackupScenesMarker = "EditorSceneManager.BackupScenes";
    protected static readonly string k_ExitPlayModeMarker = "ExitPlayMode";
    protected static readonly string k_ReloadAssemblyMarker = "ReloadAssembly";
    protected static readonly string k_BeginReloadAssemblyMarker = "BeginReloadAssembly";
    protected static readonly string k_EndReloadAssemblyMarker = "EndReloadAssembly";
    protected static readonly string k_InitializePlatformSupportModulesMarker = "UnityEditor.CoreModule.dll!UnityEditor.Modules::ModuleManager.InitializePlatformSupportModules()";
    protected static readonly string k_ScanAssembliesMarker = "TypeCache.Initialize";
    protected static readonly string k_ProcessInitializeOnLoadAttributesMarker = "ProcessInitializeOnLoadAttributes";
    protected static readonly string k_AwakeScriptedObjectsMarker = "AwakeScriptedObjects";
    protected static readonly string k_LoadScenesMarker = "EditorSceneManager.LoadScenes";
    protected static readonly string k_UpdateSceneMarker = "UpdateScene";
    protected static readonly string k_AssetDatabaseRefreshMarker = "AssetDatabase.Refresh";
    protected static readonly string k_UpdateAndCompileScriptsMarker = "UpdateAndCompileScripts";
    protected static readonly string k_InitializeManagedCompilationPipelineMarker = "InitializeManagedCompilationPipeline";
    protected static readonly string k_CompileScriptsMarker = "CompilationPipeline.CompileScripts";
    protected static readonly string k_SolutionSynchronizerSyncMarker = "SolutionSynchronizerSync";
    protected static readonly string k_IsReferenceAssemblyUnchangedMarker = "ReferenceAssemblyHelpers.IsReferenceAssemblyUnchanged";

    protected static readonly SampleGroup k_TimeSampleGroup = new SampleGroup("Time");

    [SerializeField]
    bool m_OldProfilerEnabled;
    [SerializeField]
    bool m_OldProfilerProfilerEditorEnabled;

    [SerializeField]
    protected int m_IterationCounter;

    [Serializable]
    public class SampleData
    {
        public string sampleGroup;
        public double timeMs;

        public SampleData(string sampleGroup, double timeMs)
        {
            this.sampleGroup = sampleGroup;
            this.timeMs = timeMs;
        }
    }

    [SerializeField]
    protected List<SampleData> samples = new List<SampleData>();

    protected void Setup()
    {
        samples.Clear();
        m_IterationCounter = 0;
        SetupProfiler();
    }

    [TearDown]
    public void TearDown()
    {
        CleanupProfiler();
    }

    void SetupProfiler()
    {
        m_OldProfilerEnabled = ProfilerDriver.enabled;
        m_OldProfilerProfilerEditorEnabled = ProfilerDriver.profileEditor;

        ProfilerDriver.ClearAllFrames();
        ProfilerDriver.profileEditor = true;
        ProfilerDriver.enabled = true;
    }

    void CleanupProfiler()
    {
        ProfilerDriver.enabled = m_OldProfilerEnabled;
        ProfilerDriver.profileEditor = m_OldProfilerProfilerEditorEnabled;
        ProfilerDriver.ClearAllFrames();
    }

    public static IEnumerator WaitForProfilerEnabled(float timeout)
    {
        var startTime = Time.realtimeSinceStartup;
        while (!Profiler.enabled)
        {
            if (Time.realtimeSinceStartup - startTime > timeout)
                break;
            yield return null;
        }
        Assert.IsTrue(Profiler.enabled);
    }

    public static IEnumerator WaitForProfilerDisabled(float timeout)
    {
        var startTime = Time.realtimeSinceStartup;
        while (Profiler.enabled)
        {
            if (Time.realtimeSinceStartup - startTime > timeout)
                break;
            yield return null;
        }
        Assert.IsFalse(Profiler.enabled);
    }

    public static IEnumerator WaitForProfileSampleOrTimeOut(string sampleName, float timeout)
    {
        var startTime = Time.realtimeSinceStartup;
        var lastCheckedFrame = -1;
        for (; ; )
        {
            if (Time.realtimeSinceStartup - startTime > timeout)
                yield break;

            if (lastCheckedFrame >= ProfilerDriver.lastFrameIndex)
            {
                yield return null;
                continue;
            }

            for (var j = lastCheckedFrame; j <= ProfilerDriver.lastFrameIndex; j++)
            {
                using (var frameData = new HierarchyFrameDataView(j, 0, HierarchyFrameDataView.ViewModes.Default, HierarchyFrameDataView.columnDontSort, false))
                {
                    if (!frameData.valid)
                        break;

                    var sampleId = frameData.FindChildItemByFunctionNameRecursively(frameData.GetRootItemID(), sampleName);
                    if (HierarchyFrameDataView.invalidSampleId != sampleId)
                        yield break;
                }
            }

            lastCheckedFrame = ProfilerDriver.lastFrameIndex;
        }
    }

    public void CollectSamplesAndReport(string rootSample, params string[] extraSamplesToReport)
    {
        CollectSamples(rootSample, extraSamplesToReport);
        ReportSamples();
    }

    public void CollectSamples(string rootSample, params string[] extraSamplesToReport)
    {
        var framesFound = 0;
        var currentFrame = ProfilerDriver.firstFrameIndex;
        for (; ; )
        {
            var frameData = ProfilerDriver.GetHierarchyFrameDataView(currentFrame++, 0, HierarchyFrameDataView.ViewModes.MergeSamplesWithTheSameName, HierarchyFrameDataView.columnTotalTime, false);
            if (!frameData.valid)
                break;

            if (!FindAndCollectSamplesData(frameData, rootSample, extraSamplesToReport))
                continue;

            framesFound++;
        }
        Assert.AreNotEqual(0, framesFound);
    }

    // Find a root sample in the profiler data and all child samples.
    public bool FindAndCollectSamplesData(HierarchyFrameDataView frameData, string rootMarkerName, string[] markerNames)
    {
        var rootSampleId = frameData.FindChildItemByFunctionNameRecursively(frameData.GetRootItemID(), rootMarkerName);
        if (rootSampleId == HierarchyFrameDataView.invalidSampleId)
            return false;

        var rootTimeMs = frameData.GetItemColumnDataAsSingle(rootSampleId, HierarchyFrameDataView.columnTotalTime);

        CollectSample(rootMarkerName, rootTimeMs);

        foreach (var markerName in markerNames)
        {
            var sampleId = frameData.FindChildItemByFunctionNameRecursively(rootSampleId, markerName);
            Assert.AreNotEqual(HierarchyFrameDataView.invalidSampleId, sampleId, "Did not find " + markerName + " sample");

            var timeMs = frameData.GetItemColumnDataAsSingle(sampleId, HierarchyFrameDataView.columnTotalTime);
            CollectSample(markerName, timeMs);
        }

        return true;
    }

    void CollectSample(string sampleGroup, double timeMs)
    {
        samples.Add(new SampleData(sampleGroup, timeMs));
    }

    public void ReportSamples()
    {
        var stringToSampleGroup = new Dictionary<string, SampleGroup>();

        foreach (var sample in samples)
        {
            SampleGroup sampleGroup;

            if (!stringToSampleGroup.TryGetValue(sample.sampleGroup, out sampleGroup))
            {
                sampleGroup = new SampleGroup(sample.sampleGroup);
                stringToSampleGroup[sample.sampleGroup] = sampleGroup;
            }

            Measure.Custom(sampleGroup, sample.timeMs);
        }
    }
}
