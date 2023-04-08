using UnityEngine;

public class LampController : MonoBehaviour
{
    [SerializeField] private NewDayNightCycle dayNightCycle;
    [SerializeField] private Light pointLight;

    private void Update()
    {
        bool isNight = dayNightCycle.isNight();

        if (isNight)
        {
            pointLight.intensity = 1f;
        }
        else
        {
            pointLight.intensity = 0f;
        }
    }
}
