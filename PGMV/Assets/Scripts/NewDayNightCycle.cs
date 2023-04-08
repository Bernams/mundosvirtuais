using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewDayNightCycle : MonoBehaviour
{

    [Range(0.0f, 1.0f)]
    public float time;
    public float dayLength;
    public float start = 0.4f;
    private float timeRate;
    public Vector3 noon;

    public Light sun;
    public Gradient sunColor;
    public AnimationCurve sunIntensity;

    public Light moon;
    public Gradient moonColor;
    public AnimationCurve moonIntensity;

    public AnimationCurve lightingIntensityMultiplier;
    public AnimationCurve reflectionsIntensityMultiplier;



    // Start is called before the first frame update
    void Start()
    {
        timeRate = 1.0f / dayLength;
        time = start;
    }

    // Update is called once per frame
    void Update()
    {
        time += timeRate * Time.deltaTime;
        
        if(time >= 1.0f)
        {
            time = 0.0f;
        }

        sun.transform.eulerAngles = (time - 0.25f) * noon * 4.0f;
        moon.transform.eulerAngles = (time - 0.75f) * noon * 4.0f;

        sun.intensity = sunIntensity.Evaluate(time);
        moon.intensity = moonIntensity.Evaluate(time);

        sun.color = sunColor.Evaluate(time);
        moon.color = moonColor.Evaluate(time);

        if(sun.intensity == 0 && sun.gameObject.activeInHierarchy)
        {
            sun.gameObject.SetActive(false);
        }else if(sun.intensity > 0 && !sun.gameObject.activeInHierarchy)
        {
            sun.gameObject.SetActive(true);
        }

        if (moon.intensity == 0 && moon.gameObject.activeInHierarchy)
        {
            moon.gameObject.SetActive(false);
        }
        else if (moon.intensity > 0 && !moon.gameObject.activeInHierarchy)
        {
            moon.gameObject.SetActive(true);
        }

        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time);
        RenderSettings.reflectionIntensity = reflectionsIntensityMultiplier.Evaluate(time);

    }

    public bool isNight()
    {
        return sun.intensity < 0.35;
    }
}
