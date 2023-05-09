using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeedHUD : MonoBehaviour
{

    public Rigidbody rb;
    public GameObject speedometer;

    // Update is called once per frame
    void Update()
    {
        int kph = (int) Mathf.Round(rb.velocity.magnitude * 3.6f);
        speedometer.GetComponent<TextMeshProUGUI>().text = kph + " KPH";
    }
}
