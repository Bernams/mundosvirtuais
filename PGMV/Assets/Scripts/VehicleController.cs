using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    private float horInput, vertInput;
    private float currSteerAngle, currBrakeForce;
    private bool braking;

    // Configuration
    [SerializeField] private float enginePower, brakePower, maxSteeringAngle;

    // Wheel Colliders
    [SerializeField] private WheelCollider flWheelCollider, frWheelCollider;
    [SerializeField] private WheelCollider rlWheelCollider, rrWheelCollider;

    // Wheel Transforms
    [SerializeField] private Transform flWheelTransform, frWheelTransform;
    [SerializeField] private Transform rlWheelTransform, rrWheelTransform;

    private void FixedUpdate() {
        AcquireInput();
        ManageMotor();
        ManageSteering();
        UpdateWheelPositions();
    }

    private void AcquireInput() {
        // Steering Input
        horInput = Input.GetAxis("Horizontal");

        // Throttle Input
        vertInput = Input.GetAxis("Vertical");

        // Brake Input
        braking = Input.GetKey(KeyCode.Space);
    }

    private void ManageMotor() {
        flWheelCollider.motorTorque = vertInput * enginePower;
        frWheelCollider.motorTorque = vertInput * enginePower;
        currBrakeForce = braking ? brakePower : 0f;
        ApplyBraking();
    }

    private void ApplyBraking() {
        frWheelCollider.brakeTorque = currBrakeForce;
        flWheelCollider.brakeTorque = currBrakeForce;
        rlWheelCollider.brakeTorque = currBrakeForce;
        rrWheelCollider.brakeTorque = currBrakeForce;
    }

    private void ManageSteering() {
        currSteerAngle = maxSteeringAngle * horInput;
        flWheelCollider.steerAngle = currSteerAngle;
        frWheelCollider.steerAngle = currSteerAngle;
    }

    private void UpdateWheelPositions() {
        UpdateSingleWheelPosition(flWheelCollider, flWheelTransform);
        UpdateSingleWheelPosition(frWheelCollider, frWheelTransform);
        UpdateSingleWheelPosition(rrWheelCollider, rrWheelTransform);
        UpdateSingleWheelPosition(rlWheelCollider, rlWheelTransform);
    }

    private void UpdateSingleWheelPosition(WheelCollider wheelCol, Transform wheelTrans) {
        Vector3 position;
        Quaternion rotation; 
        wheelCol.GetWorldPose(out position, out rotation);
        wheelTrans.rotation = rotation;
        wheelTrans.position = position;
    }
}