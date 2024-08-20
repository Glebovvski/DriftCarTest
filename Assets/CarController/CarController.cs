using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    public InputActionAsset inputActions;

    public WheelCollider frontLeftWheel, frontRightWheel;
    public WheelCollider rearLeftWheel, rearRightWheel;

    public Transform frontLeftTransform, frontRightTransform;
    public Transform rearLeftTransform, rearRightTransform;

    public float maxSteerAngle = 30f;
    public float motorForce = 50f;
    public float brakeForce = 100f;
    public float steerSpeed = 5f;

    private float steeringInput;
    private float currentSteerAngle;
    private float accelerationInput;
    private float brakeInput;
    private bool isHandbraking;

    private Rigidbody rb;

    private void OnEnable()
    {
        inputActions["Driving/Steer"].performed += ctx => steeringInput = ctx.ReadValue<float>();
        inputActions["Driving/Steer"].canceled += ctx => steeringInput = 0f;

        inputActions["Driving/Accelerate"].performed += ctx => accelerationInput = ctx.ReadValue<float>();
        inputActions["Driving/Accelerate"].canceled += ctx => accelerationInput = 0f;
        
        inputActions["Driving/Decelerate"].performed += ctx => accelerationInput += ctx.ReadValue<float>();
        inputActions["Driving/Decelerate"].canceled += ctx => accelerationInput = 0f;

        inputActions["Driving/Break"].performed += ctx => brakeInput = ctx.ReadValue<float>();
        inputActions["Driving/Break"].canceled += ctx => brakeInput = 0f;

        inputActions["Driving/Handbreak"].performed += ctx => isHandbraking = true;
        inputActions["Driving/Handbreak"].canceled += ctx => isHandbraking = false;

        inputActions.Enable();

        rb = GetComponent<Rigidbody>();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void FixedUpdate()
    {
        HandleSteering();
        HandleMotor();
        UpdateWheelPoses();
        HandleDownForce();
    }

    private void HandleDownForce()
    {
        rb.AddForce(Vector3.down * (rb.mass * 10), ForceMode.Force);
    }

    private void HandleSteering()
    {
        float targetSteerAngle = maxSteerAngle * steeringInput;
        
        // Smoothly interpolate current steer angle towards target steer angle
        currentSteerAngle = Mathf.Lerp(currentSteerAngle, targetSteerAngle, steerSpeed * Time.deltaTime);
        
        // Apply the smoothly interpolated steering angle to the wheels
        frontLeftWheel.steerAngle = currentSteerAngle;
        frontRightWheel.steerAngle = currentSteerAngle;
    }

    private void HandleMotor()
    {
        frontLeftWheel.motorTorque = accelerationInput * motorForce;
        frontRightWheel.motorTorque = accelerationInput * motorForce;

        if (isHandbraking)
        {
            rearLeftWheel.brakeTorque = brakeForce;
            rearRightWheel.brakeTorque = brakeForce;
        }
        else
        {
            rearLeftWheel.brakeTorque = brakeInput * brakeForce;
            rearRightWheel.brakeTorque = brakeInput * brakeForce;
        }
    }

    private void UpdateWheelPoses()
    {
        UpdateWheelPose(frontLeftWheel, frontLeftTransform);
        UpdateWheelPose(frontRightWheel, frontRightTransform);
        UpdateWheelPose(rearLeftWheel, rearLeftTransform);
        UpdateWheelPose(rearRightWheel, rearRightTransform);
    }

    private void UpdateWheelPose(WheelCollider collider, Transform transform)
    {
        Vector3 pos;
        Quaternion quat;
        collider.GetWorldPose(out pos, out quat);

        transform.position = pos;
        transform.rotation = quat;
    }
}
