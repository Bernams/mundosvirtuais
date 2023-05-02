using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VehicleController : MonoBehaviour
{
    public WheelCollider[] wheelColliders;
    public Transform[] wheelMeshes;
    public float maxMotorTorque = 250;
    public float maxSteeringAngle = 20;
    public float maxSpeed = 10;
    public float decelerationForce = 10;
    public float antiRollForce = 10;
    public float centerOfMassY = -10.0f;
    public float brakePower = 1000;
    
    public CinemachineVirtualCamera cam;

    private Rigidbody rb;
    private bool changedCam = false;
    private bool grounded = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.centerOfMass = new Vector3(rb.centerOfMass.x, 0.5f, 0.3f);
    }

    private void Update()
    {
        UpdateWheelMeshesPositions();
    }

    private void FixedUpdate()
    {
        float motorInput = Input.GetAxis("Vertical");
        float steeringInput = Input.GetAxis("Horizontal");
        bool braking = Input.GetKey(KeyCode.Space);


        Drive(motorInput);
        Steer(steeringInput);
        DecelerateWhenNoInput(motorInput);
        ApplyAntiRollForce();
        float brakeForce = braking ? brakePower : 0f;
        ApplyBraking(braking, brakeForce);
        UnflipCar();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Garage") && !changedCam){
            cam.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance = 4;
            changedCam = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Garage") && changedCam){
            cam.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance = 8;
            changedCam = false;
        }
    }



    private void Drive(float input)
    {
        if (rb.velocity.magnitude < maxSpeed)
        {
            float motorTorque = input * maxMotorTorque;
            foreach (WheelCollider wheel in wheelColliders)
            {
                wheel.motorTorque = motorTorque;
            }
        }
    }

    private void Steer(float input)
    {
        float frontSteeringAngle = input * maxSteeringAngle;
        wheelColliders[0].steerAngle = frontSteeringAngle;
        wheelColliders[1].steerAngle = frontSteeringAngle;
    }

    private void DecelerateWhenNoInput(float motorInput)
    {
        if (Mathf.Abs(motorInput) < 0.1f)
        {
            foreach (WheelCollider wheel in wheelColliders)
            {
                wheel.brakeTorque = decelerationForce;
            }
            rb.drag = 0.7f;
        }
        else
        {
            foreach (WheelCollider wheel in wheelColliders)
            {
                wheel.brakeTorque = 0;
            }
            rb.drag = 0;
        }
    }

    private void ApplyBraking(bool braking, float brakeForce)
    {
        if (braking && grounded)
        {
            foreach (WheelCollider wheel in wheelColliders)
            {
                wheel.brakeTorque = brakeForce;
            }
            rb.drag = 1;
        }
        else
        {
            rb.drag = 0;
        }
    }

    private void ApplyAntiRollForce()
    {
        for (int i = 0; i < wheelColliders.Length - 1; i += 2)
        {
            WheelCollider wheelL = wheelColliders[i];
            WheelCollider wheelR = wheelColliders[i + 1];

            WheelHit hit;
            float travelL = 1.0f;
            float travelR = 1.0f;

            bool groundedL = wheelL.GetGroundHit(out hit);
            if (groundedL)
                travelL = (-wheelL.transform.InverseTransformPoint(hit.point).y - wheelL.radius) / wheelL.suspensionDistance;

            bool groundedR = wheelR.GetGroundHit(out hit);
            if (groundedR)
                travelR = (-wheelR.transform.InverseTransformPoint(hit.point).y - wheelR.radius) / wheelR.suspensionDistance;

            grounded = groundedL && groundedR;

            antiRollForce = (travelL - travelR) * antiRollForce;

            if (groundedL)
                rb.AddForceAtPosition(wheelL.transform.up * -antiRollForce, wheelL.transform.position);
            if (groundedR)
                rb.AddForceAtPosition(wheelR.transform.up * antiRollForce, wheelR.transform.position);
        }
    }

    private void UpdateWheelMeshesPositions()
    {
        for (int i = 0; i < wheelColliders.Length; i++)
        {
            Quaternion quat;
            Vector3 pos;
            wheelColliders[i].GetWorldPose(out pos, out quat);

            wheelMeshes[i].position = pos;
            wheelMeshes[i].rotation = quat;
        }
    }

    private void UnflipCar()
    {
        if (transform.up.y < 0)
        {
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, Vector3.up) * transform.rotation;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 100 * Time.deltaTime);
        }
    }
}