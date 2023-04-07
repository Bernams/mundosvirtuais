using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moon_Controller : MonoBehaviour
{
    [SerializeField] public float distance = 1000.0f;
    [SerializeField] public float scale = 15.0f;


    void Start()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, distance);
        transform.localScale = new Vector3(scale, scale, scale);
    
    }
}
