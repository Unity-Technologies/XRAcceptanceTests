using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealtimeLighting : MonoBehaviour {
    
    public RealtimeLight[] realtimeLights;
    public float fadeDuration = 0.5f;
    public float idleDuration = 2.0f;
    public Orbit orbit;
    
    private IEnumerator Fade(float direction, RealtimeLight light)
    {
        
        light.gameObject.SetActive(true);
        direction = Mathf.Sign(direction);

        light.SetIntensity(direction > 0f ? 0f : 1.0f);
        
        float timer = 0.0f;
        while(timer < fadeDuration)
        {
            float t = Mathf.Clamp01(timer / fadeDuration);

            light.SetIntensity(direction > 0 ? t : (1.0f - t));
            
            if (direction > 0f)
            {
                light.SetLocalRotation(Quaternion.AngleAxis(180.0f - t * 180.0f, Vector3.right));
            }
            else
            {
                light.SetLocalRotation(Quaternion.AngleAxis(180.0f + (1.0f - t) * 180.0f, Vector3.right));
            }

            timer += Time.deltaTime;
            yield return null;
        }

        light.SetIntensity(direction > 0f ? 1f : 0.0f);
        light.gameObject.SetActive(false);

        light.SetLocalRotation(direction > 0 ? Quaternion.AngleAxis(0.0f, Vector3.right) : Quaternion.AngleAxis(90.0f, Vector3.right));
        
    }

    private IEnumerator Idle(RealtimeLight light)
    {
        light.gameObject.SetActive(true);
        yield return new WaitForSeconds(idleDuration);
    }

    public IEnumerator Start()
    {
        int index = 0;

        //Initialize
        DisableLights();
        if(realtimeLights.Length > 0)
        {
            realtimeLights[0].SetIntensity(1.0f);
        }
        
        //Loop through lights
        if (realtimeLights.Length > 1)
        {
            while (true)
            {
                yield return StartCoroutine(Idle(realtimeLights[index]));
                yield return StartCoroutine(Fade(-1.0f, realtimeLights[index]));
                index++;
                if (index >= realtimeLights.Length)
                {
                    index = 0;
                }
                yield return StartCoroutine(Fade(1.0f, realtimeLights[index]));
            }
        }
    }

    public void Update()
    {
        if (orbit != null)
        {
            foreach (RealtimeLight light in realtimeLights)
            {
                light.transform.position = orbit.transform.position;
            }
        }
    }

    private void DisableLights()
    {
        foreach(RealtimeLight light in realtimeLights)
        {
            light.gameObject.SetActive(false);
        }
    }
}
