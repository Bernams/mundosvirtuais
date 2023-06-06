using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindZoneController : MonoBehaviour
{

    public WindZone windZone;

    private float timeElapsed;
    private bool isRising = true;

    void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= 1f)
        {
            if (isRising)
            {
                if (windZone.windMain < 3)
                {
                    windZone.windMain += 0.01f;
                }
                else
                {
                    isRising = false;
                }
            }
            else
            {
                if (windZone.windMain > 0)
                {
                    windZone.windMain -= 0.01f;
                }
                else
                {
                    isRising = true;
                }
            }
            timeElapsed = 0;
        }

        windZone.transform.rotation = Quaternion.Euler(windZone.transform.rotation.eulerAngles.x, windZone.transform.rotation.eulerAngles.y + 0.05f, windZone.transform.rotation.eulerAngles.z);
    }
}
