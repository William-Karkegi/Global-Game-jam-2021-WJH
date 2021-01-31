using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class DayNightCycle : MonoBehaviour
{
    [Range(0, 24)] public float timeOfDay;
    public float orbitSpeed = 1.0f;
    public Light sun;
    public Light moon;
    public Volume SkyVolume;
    public AnimationCurve starsCurve;

    private bool isNight = false;
    private PhysicallyBasedSky sky;
    
    // Start is called before the first frame update
    void Start()
    {
        SkyVolume.profile.TryGet(out sky);
    }

    // Update is called once per frame
    void Update()
    {
        timeOfDay += Time.deltaTime * orbitSpeed;
        if (timeOfDay > 24)
            timeOfDay = 0;
        
        UpdateTime();
    }

    private void OnValidate()
    {
        SkyVolume.profile.TryGet(out sky);
        UpdateTime();
    }

    private void UpdateTime()
    {
        float alpha = timeOfDay / 24.0f;
        float sunRotation = Mathf.Lerp(-90, 270, alpha);
        float moonRotation = sunRotation - 180;
        
        sun.transform.rotation = Quaternion.Euler(sunRotation, -150.0f, 0);
        moon.transform.rotation = Quaternion.Euler(moonRotation, -150.0f, 0);

        sky.spaceEmissionMultiplier.value = starsCurve.Evaluate(alpha) * 1000.0f;
            
        CheckNightDayTransition();
    }

    private void CheckNightDayTransition()
    {
        if (isNight && moon.transform.rotation.eulerAngles.x > 180)
            StartDay();
        else if (sun.transform.rotation.eulerAngles.x > 180)
            StartNight();
    }

    private void StartDay()
    {
        isNight = false;
        sun.shadows = LightShadows.Soft;
        moon.shadows = LightShadows.None;
    }
    
    private void StartNight()
    {
        isNight = true;
        sun.shadows = LightShadows.None;
        moon.shadows = LightShadows.Soft;
    }
}
