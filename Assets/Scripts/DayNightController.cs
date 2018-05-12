using UnityEngine;
using System.Collections;
 
public class DayNightController : MonoBehaviour {
    
    public static DayNightController instance = null;
    
    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    public Light sun;
    public float sunrise = .23f;
    public float sunset = .75f;
    public float secondsInFullDay = 120f;
    [Range(0,1)]
    public float currentTimeOfDay = 0;
    [HideInInspector]
    public float timeMultiplier = 1f;
    
    public bool isDayTime;
    float sunInitialIntensity;
    
    void Start() {
        sunInitialIntensity = sun.intensity;
        isDayTime = true;
    }
    
    void Update() {
        UpdateSun();

        if (secondsInFullDay > 0) {
            currentTimeOfDay += (Time.deltaTime / secondsInFullDay) * timeMultiplier;
        }
 
        if (currentTimeOfDay >= 1) {
            currentTimeOfDay = 0;
        }

        if (currentTimeOfDay <= sunrise || currentTimeOfDay >= sunset) {
            isDayTime = false;
        } else {
            isDayTime = true;
        }
    }
    
    void UpdateSun() {
        sun.transform.localRotation = Quaternion.Euler((currentTimeOfDay * 360f) - 90, 170, 0);
 
        float intensityMultiplier = 1;
        if (currentTimeOfDay <= sunrise || currentTimeOfDay >= sunset) {
            intensityMultiplier = 0;
        }
        else if (currentTimeOfDay <= sunrise + 0.02f) {
            intensityMultiplier = Mathf.Clamp01((currentTimeOfDay - sunrise) * (1 / 0.02f));
        }
        else if (currentTimeOfDay >= sunset - 0.02f) {
            intensityMultiplier = Mathf.Clamp01(1 - ((currentTimeOfDay - sunset - 0.02f) * (1 / 0.02f)));
        }
 
        sun.intensity = sunInitialIntensity * intensityMultiplier;
    }
}