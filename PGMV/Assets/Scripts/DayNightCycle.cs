using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private float cycleDuration = 60f;

    private float timer;
    private Light directionalLight;

    private void Start()
    {
        directionalLight = GetComponent<Light>();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        float timePercent = timer / cycleDuration;
        float sunAngle = Mathf.Lerp(-90f, 270f, timePercent);

        directionalLight.transform.localRotation = Quaternion.Euler(new Vector3(sunAngle, 0f, 0f));

        if (timer >= cycleDuration)
        {
            timer -= cycleDuration;
        }
    }

    public void SetCycleDuration(float duration)
    {
        cycleDuration = duration;
    }

    public float GetTimePercent()
    {
        return timer / cycleDuration;
    }

    public float GetTimer()
    {
        return timer;
    }

    public float GetSunAngle()
    {
        return Mathf.Lerp(-90f, 270f, GetTimePercent());
    }
}
