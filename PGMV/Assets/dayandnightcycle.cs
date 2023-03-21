using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dayandnightcycle : MonoBehaviour
{
    
    Vector3 rot=Vector3.zero;
    float degpersec = 1;


    // Update is called once per frame
    void Update()
    {
        rot.x=degpersec*Time.deltaTime;
        transform.Rotate(rot,Space.World);

    }
}
