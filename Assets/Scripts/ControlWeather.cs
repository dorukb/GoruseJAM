using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class ControlWeather : MonoBehaviour
{
    public float overcastAbsorption = 1.27f;
    public float overcastDensityOffset = -0.38f;
    public float overcastTimeScale = 0.5f;

    public float clearAbsorption = 0.75f;
    public float clearDensityOffset = -4.27f;
    public float clearTimeScale = 0.85f;

    CloudMaster cloudController;
    public float transitionDuration = 2f;

    bool isInTransition = false;


    public bool isOvercast = false;
    private void Awake()
    {
        cloudController = FindObjectOfType<CloudMaster>();
    }

    private void Update()
    {
        // For test purposes, call these via triggers automatically.
        if (Input.GetKeyDown(KeyCode.O))
        {
            // overcast transition
            TransitionToOvercast(transitionDuration);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            TransitionToClearSky(transitionDuration);
        }
    }

    public void ToggleWeather(float duration)
    {
        if (isOvercast)
        {
            TransitionToClearSky(duration);
        }
        else
        {
            TransitionToOvercast(duration);
        }
    }
    public void TransitionToOvercast(float duration)
    {
        if (!isInTransition && !isOvercast)
        {
            StopAllCoroutines();

            StartCoroutine(OvercastTransition(duration));
            FindObjectOfType<Light>().DOIntensity(.5f, duration);
        }
    }

    public void TransitionToClearSky(float duration)
    {
        if (!isInTransition && isOvercast)
        {
            StopAllCoroutines();
            StartCoroutine(ClearSkyTransition(duration));
            FindObjectOfType<Light>().DOIntensity(1f, duration);
        }
    }

    public IEnumerator OvercastTransition(float duration)
    {
        isInTransition = true;
        float timePassed = 0f;
        while(timePassed < duration)
        {
            float t = timePassed / duration;
            float absorption = Mathf.Lerp(clearAbsorption, overcastAbsorption, t);
            float densityOffset = Mathf.Lerp(clearDensityOffset, overcastDensityOffset, t);
            float timeScale = Mathf.Lerp(clearTimeScale, overcastTimeScale, t);
            cloudController.SetWeatherParams(densityOffset, absorption, timeScale);

            timePassed += Time.deltaTime;
            yield return null;
        }
        cloudController.SetWeatherParams(overcastDensityOffset, overcastAbsorption, overcastTimeScale);
        isInTransition = false;
        isOvercast = true;

    }
    public IEnumerator ClearSkyTransition(float duration)
    {
        isInTransition = true;
        float timePassed = 0f;
        while (timePassed < duration)
        {
            float t = timePassed / duration;
            float absorption = Mathf.Lerp(overcastAbsorption, clearAbsorption, t);
            float densityOffset = Mathf.Lerp(overcastDensityOffset, clearDensityOffset, t);
            float timeScale = Mathf.Lerp(overcastTimeScale, clearTimeScale, t);
            cloudController.SetWeatherParams(densityOffset, absorption, timeScale);

            timePassed += Time.deltaTime;
            yield return null;
        }
        cloudController.SetWeatherParams(clearDensityOffset, clearAbsorption, clearTimeScale);
        isInTransition = false;
        isOvercast = false;
    }
}
