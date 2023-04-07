using UnityEngine;

public class LampController : MonoBehaviour
{
    [SerializeField] private DayNightCycle dayNightCycle;
    [SerializeField] private Light pointLight;

    private void Start()
    {
        if (dayNightCycle == null)
        {
            Debug.LogError("DayNightCycle reference not set in LightControl script!");
            return;
        }

        if (pointLight == null)
        {
            Debug.LogError("PointLight reference not set in LightControl script!");
            return;
        }
    }

    private void Update()
    {
        float timePercent = dayNightCycle.GetTimePercent();

        if (timePercent > 0.25f && timePercent < 0.75f)
        {
            pointLight.intensity = 0f;
        }
        else
        {
            pointLight.intensity = 1f;
        }
    }
}
