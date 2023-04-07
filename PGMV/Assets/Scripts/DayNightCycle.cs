using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private float cycleDuration = 60f; // default duration of 60 seconds

    private float timer; // keeps track of time elapsed since start of cycle
    private Light directionalLight; // reference to the directional light

    private void Start()
    {
        directionalLight = GetComponent<Light>();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        float timePercent = timer / cycleDuration; // value between 0 and 1 representing the current time in the cycle
        float sunAngle = Mathf.Lerp(-90f, 270f, timePercent); // interpolate sun/moon angle based on time

        directionalLight.transform.localRotation = Quaternion.Euler(new Vector3(sunAngle, 0f, 0f)); // rotate the light

        if (timer >= cycleDuration)
        {
            timer -= cycleDuration;
        }
    }

    public void SetCycleDuration(float duration)
    {
        cycleDuration = duration;
    }
}
