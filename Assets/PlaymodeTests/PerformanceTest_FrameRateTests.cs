using System.Collections;
using Unity.PerformanceTesting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class PerformanceTest_FrameRateTests
{
    [UnityTest, Performance]
    public IEnumerator PerformanceTest_FrameRateTest()
    {
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync("Assets/Scenes/PerformanceTest/PerformanceTest.unity", LoadSceneMode.Single);

        while (!asyncLoadLevel.isDone)
        {
            yield return null;
        }

        // Allow for initial Scene Load settle time for more stable measurement
        yield return new WaitForSecondsRealtime(1);

        yield return Measure.Frames().MeasurementCount(1000).Run();
    }
}