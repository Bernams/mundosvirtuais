using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    public WheelCollider[] wheelColliders;
    public Transform[] wheelMeshes;
    public float maxMotorTorque = 200f;
    public float maxSteeringAngle = 30f;
    public float rearSteeringAngleFactor = 0.3f; 
    public float maxSpeed = 50f;
    public float decelerationForce = 15f;
    public float antiRollForce = 10000f;
    public float centerOfMassY = -1.0f;
    public float brakePower;
    public float antiRoll = 5000.0f;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.centerOfMass = new Vector3(rb.centerOfMass.x, centerOfMassY, rb.centerOfMass.z);
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
        WheelAxle(wheelColliders[0], wheelColliders[1]);
        WheelAxle(wheelColliders[2], wheelColliders[3]);
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

    private void WheelAxle(WheelCollider wheelL, WheelCollider wheelR)
    {
        WheelHit hit;
        float travelL = 1.0f;
        float travelR = 1.0f;

        bool groundedL = wheelL.GetGroundHit(out hit);
        if (groundedL) {
            travelL = (-wheelL.transform.InverseTransformPoint(hit.point).y - wheelL.radius) / wheelL.suspensionDistance;
        }

        bool groundedR = wheelR.GetGroundHit(out hit);
        if (groundedR) {
            travelR = (-wheelR.transform.InverseTransformPoint(hit.point).y - wheelR.radius) / wheelR.suspensionDistance;
        }

        float antiRollForce = (travelL - travelR) * antiRoll;

        if (groundedL) {
            rb.AddForceAtPosition(wheelL.transform.up * -antiRollForce, wheelL.transform.position);
        }

        if (groundedR) {
            rb.AddForceAtPosition(wheelR.transform.up * antiRollForce, wheelR.transform.position);
        }
    }

    private void Steer(float input)
    {
        float frontSteeringAngle = input * maxSteeringAngle;
        wheelColliders[0].steerAngle = frontSteeringAngle;
        wheelColliders[1].steerAngle = frontSteeringAngle;

        float rearSteeringAngle = frontSteeringAngle * rearSteeringAngleFactor;
        wheelColliders[2].steerAngle = rearSteeringAngle;
        wheelColliders[3].steerAngle = rearSteeringAngle;
    }

    private void DecelerateWhenNoInput(float motorInput)
    {
        if (Mathf.Abs(motorInput) < 0.1f)
        {
            foreach (WheelCollider wheel in wheelColliders)
            {
                wheel.brakeTorque = decelerationForce;
            }
        }
        else
        {
            foreach (WheelCollider wheel in wheelColliders)
            {
                wheel.brakeTorque = 0;
            }
        }
    }

    private void ApplyBraking(bool braking, float brakeForce)
    {
        if (braking)
        {
            foreach (WheelCollider wheel in wheelColliders)
            {
                wheel.brakeTorque = brakeForce;
            }
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