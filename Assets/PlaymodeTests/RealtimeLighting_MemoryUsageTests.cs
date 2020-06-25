using System.Collections;
using Unity.PerformanceTesting;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class RealtimeLighting_MemoryUsageTests
{
    [UnityTest, Performance]
    public IEnumerator RealtimeLighting_MemoryUsageTest()
    {
        var allocated = new SampleGroup("TotalAllocatedMemory", SampleUnit.Byte);
        var reserved = new SampleGroup("TotalReservedMemory", SampleUnit.Byte);
		
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync("Assets/Scenes/LightingTest/RealtimeLighting/RealtimeLighting.unity", LoadSceneMode.Single);
		
        while (!asyncLoadLevel.isDone)
        {
            yield return null;
        }
        for (int i = 0; i < 20; i++)
        {
            using (Measure.Scope())
            {
                Measure.Custom(allocated, Profiler.GetTotalAllocatedMemoryLong());
                Measure.Custom(reserved, Profiler.GetTotalReservedMemoryLong());
                yield return null;
            }
        }
    }
}