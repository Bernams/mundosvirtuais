using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VehicleController : MonoBehaviour
{
    public WheelCollider[] wheelColliders;
    public Transform[] wheelMeshes;
    public Light Leftheadlight;
    public Light Rightheadlight;
    public float maxMotorTorque = 250;
    public float maxMotorTorqueTurbo = 500;
    public float maxSteeringAngle = 20;
    public float maxSpeedKPH = 60;
    public float maxTurboSpeedKPH = 120;
    public float decelerationForce = 10;
    public float antiRollForce = 10;
    public float centerOfMassY = -10.0f;
    public float brakePower = 1000;

    public CinemachineVirtualCamera cam;
    public GameTimer gameTimer;

    public float minPitch = 0.5f;
    public float maxPitch = 3f;
    public float volume = 0.2f;
    public AudioClip runningEngineSound;
    public AudioClip idleEngineSound;
    public AudioClip collisionSound;
    public AudioSource collisionPlayer;

    private AudioSource engineSound;
    private Rigidbody rb;
    private bool changedCam = false;
    private bool groundedVehicle = false;
    private bool groundedWheels = false;
    private bool lightsOn = false;
    private float elapsedTime = 0f;
    private CinemachineBasicMultiChannelPerlin noise;
    private bool isShaking = false;

    private void Start()
    {
        engineSound = GetComponent<AudioSource>();
        engineSound.pitch = 1;
        engineSound.volume = volume;
        engineSound.Play();
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.centerOfMass = new Vector3(rb.centerOfMass.x, 0.5f, 0.3f);
    }

    private void Update()
    {
        if (gameTimer.isTimerRunning)
        {
            if (isShaking)
            {
                elapsedTime += Time.deltaTime;
            }
            float motorInput = Input.GetAxis("Vertical");
            float steeringInput = Input.GetAxis("Horizontal");
            bool braking = Input.GetKey(KeyCode.Space);
            bool isTurbo = Input.GetKey(KeyCode.LeftShift);
            bool unflip = Input.GetKey(KeyCode.F);

            StopShake();
            Drive(motorInput, isTurbo);
            Steer(steeringInput);
            DecelerateWhenNoInput(motorInput);
            ApplyAntiRollForce();
            float brakeForce = braking ? brakePower : 0f;
            ApplyBraking(braking, brakeForce);
            UnflipCar(unflip);
            CheckOutOfBounds();
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
        bool changeLights = Input.GetKeyDown(KeyCode.L);
        UpdateWheelMeshesPositions();
        ChangeLights(changeLights);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Garage") && !changedCam)
        {
            cam.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance = 4;
            changedCam = true;
        }
    }

    private void ChangeLights(bool changeLights)
    {
        if (changeLights)
        {
            if (lightsOn)
            {
                lightsOn = false;
                Leftheadlight.intensity = 0f;
                Rightheadlight.intensity = 0f;
            }
            else
            {
                lightsOn = true;
                Leftheadlight.intensity = 3f;
                Rightheadlight.intensity = 3f;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Garage") && changedCam)
        {
            cam.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance = 8;
            changedCam = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        collisionPlayer.PlayOneShot(collisionSound);
        if (collision.gameObject.CompareTag("Terrain"))
        {
            groundedVehicle = true;
        }
        noise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noise.m_AmplitudeGain = 10;
        noise.m_FrequencyGain = 4;
        isShaking = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
        {
            groundedVehicle = false;
        }
    }

    private void StopShake()
    {
        if (elapsedTime >= 0.25)
        {
            noise.m_FrequencyGain = 0;
            noise.m_AmplitudeGain = 0;
            isShaking = false;
            elapsedTime = 0;
        }
    }


    private void Drive(float input, bool isTurbo)
    {
        if (rb.velocity.magnitude < 0.1)
        {
            engineSound.clip = idleEngineSound;
            engineSound.pitch = 1;
        }
        else
        {
            engineSound.clip = runningEngineSound;
            float speedRatio = rb.velocity.magnitude / (maxTurboSpeedKPH / 3.6f);
            engineSound.pitch = Mathf.Lerp(minPitch, maxPitch, speedRatio);
        }

        if (!engineSound.isPlaying)
        {
            engineSound.Play();
        }


        float speed;
        float torque;

        if (isTurbo)
        {
            speed = maxTurboSpeedKPH;
            torque = maxMotorTorqueTurbo;
        }
        else
        {
            if (rb.velocity.magnitude > maxSpeedKPH / 3.6f && (rb.velocity.magnitude - maxSpeedKPH / 3.6f) > 1)
            {
                speed = maxTurboSpeedKPH;
                torque = maxMotorTorque;
                if (Vector3.Dot(rb.velocity, transform.forward) > 0)
                {

                    Vector3 backwardForce = -transform.forward * 140f;
                    rb.AddForce(backwardForce, ForceMode.Impulse);
                }
                else
                {
                    Vector3 forwardForce = transform.forward * 140f;

                    rb.AddForce(forwardForce, ForceMode.Impulse);
                }

            }
            else
            {
                speed = maxSpeedKPH;
                torque = maxMotorTorque;
            }
        }

        if (rb.velocity.magnitude < speed / 3.6f)
        {
            float motorTorque;

            motorTorque = input * torque;

            foreach (WheelCollider wheel in wheelColliders)
            {
                wheel.motorTorque = motorTorque;
            }
        }
        else
        {
            rb.velocity = rb.velocity.normalized * speed / 3.6f;
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
                if (Vector3.Dot(rb.velocity, transform.forward) > 0)
                {
                    wheel.motorTorque = -maxMotorTorqueTurbo * 1000;
                }
                else
                {
                    wheel.motorTorque = maxMotorTorqueTurbo * 1000;
                }
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
        if (braking && groundedWheels)
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

            groundedWheels = groundedL && groundedR;

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

    private void UnflipCar(bool unflip)
    {
        if (unflip && groundedVehicle && transform.rotation.eulerAngles.z >= 180 && transform.rotation.eulerAngles.z < 360)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
            Quaternion targetRotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 3000 * Time.deltaTime);
        }
    }

    private void CheckOutOfBounds()
    {
        if (transform.position.y < -3)
        {
            transform.position = new Vector3(132, 20.38f, -600);
            transform.rotation = Quaternion.identity;
            rb.velocity = new Vector3(0, 0, 0);
        }
    }
}