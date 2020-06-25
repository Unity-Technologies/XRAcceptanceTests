using System.Collections;
using Unity.PerformanceTesting;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine.TestTools;

public class RealtimeLighting_EnterPlaymodeTests : TestWithProfilerIsolation
{
    const int k_MeasurementsCount = 5;

    [UnityTest, Performance]
    public IEnumerator RealtimeLighting_EnterPlaymodeTest()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/LightingTest/RealtimeLighting/RealtimeLighting.unity", OpenSceneMode.Single);

        Setup();
        yield return WaitForProfilerEnabled(5.0f);

        while (m_IterationCounter++ < k_MeasurementsCount)
        {
            yield return new EnterPlayMode();
            yield return new ExitPlayMode();
        }

        // Extract data from profiler
        ProfilerDriver.enabled = false;
        yield return WaitForProfilerDisabled(5.0f);

        CollectSamplesAndReport(k_EnterPlayModeMarker,
            k_BackupScenesMarker,
            k_ReloadAssembliesMarker, k_BeginReloadAssemblyMarker, k_EndReloadAssemblyMarker,
            k_LoadScenesMarker, k_UpdateSceneMarker);
    }
}

