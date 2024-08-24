using System;
using Cinemachine;
using Core;
using GameTools;
using UI;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Zenject;

namespace Car
{
    public enum ControlType
    {
        Keyboard,
        Buttons
    }

    [RequireComponent(typeof(Rigidbody))]
    public class CarController : NetworkBehaviour
    {
        private const int WHEELS_COUNT = 4;

        public InputActionAsset inputActions;

        public GameObject frontLeftWheel, frontRightWheel;
        public GameObject rearLeftWheel, rearRightWheel;

        [Space(10)] public Transform frontLeftTransform, frontRightTransform;
        public Transform rearLeftTransform, rearRightTransform;

        [SerializeField] private HUD hud;
        [SerializeField] private DriftCounter driftCounter;

        [SerializeField] private float maxSteerAngle = 30f;
        [SerializeField] private float motorForce = 50f;
        [SerializeField] private float brakeForce = 100f;
        [SerializeField] private float steerSpeed = 5f;
        [SerializeField] private float driftTolerance = 0.5f;
        [SerializeField] private float driftFactor = 0.95f; // Lower for more drift
        [SerializeField] private float driftControl = 1.5f; // Control drift amount

        [Space(10)] [Header("Wheels")] [Range(0, 1)] [SerializeField]
        private float suspensionDistance;

        [Range(0, 1)] [SerializeField] private float suspensionOffset;
        [Range(0, 1)] [SerializeField] private float sidewaysFriction;
        [Range(0, 1)] [SerializeField] private float forwardFriction;

        private float steeringInput;
        private float currentSteerAngle;
        private float accelerationInput;
        private float brakeInput;
        private bool isHandbraking;

        private Rigidbody rb;
        private WheelCollider[] Wheels;

        public bool IsDrifting => isDriftingApplied && rb.velocity.sqrMagnitude > driftTolerance * driftTolerance;
        private bool isDriftingApplied = false;

        private bool autoGas = false;

        public bool IsControllable { get; private set; } = false;
        public DriftCounter DriftCounter => driftCounter;

        private CinemachineVirtualCamera _camera;

        // [Inject] private GameTimer _gameTimer;
        private PlayerData playerData;

        private float lastDriftCountUpdate;
        public PlayerData PlayerData => playerData;


        private void Start()
        {
            Wheels = new WheelCollider[WHEELS_COUNT];
            SetupWheels();
            rb = GetComponent<Rigidbody>();
            // if (!IsOwner)
            //     return;
            // _gameTimer.OnGameplayEnd += () => SetIsControllable(false);

            // UpdateControlType();
        }
        

        public override void OnNetworkSpawn()
        {
            this.enabled = true;
            base.OnNetworkSpawn();
            SetIsControllable(true);
            _camera = GetComponentInChildren<CinemachineVirtualCamera>();

            driftCounter.Init(this);
            hud.Init(this);
            if (IsOwner)
            {
                driftCounter.enabled = true;
                hud.enabled = true;
                _camera.Priority = 10;
            }
            else
            {
                _camera.Priority = 0;
            }
        }

        public void SetCharacteristics(CarData data)
        {
            motorForce = data.MotorForce;
            steerSpeed = data.SteerSpeed;
            maxSteerAngle = data.MaxSteerAngle;
        }

        // private void UpdateControlType()
        // {
        //     autoGas = _playerData.CarSettings.ControlType == ControlType.Buttons;
        // }

        private void OnEnable()
        {
            if (!IsOwner) return;

            inputActions["Driving/Steer"].performed += ctx => steeringInput = ctx.ReadValue<float>();
            inputActions["Driving/Steer"].canceled += ctx => steeringInput = 0f;

            inputActions["Driving/Accelerate"].performed += ctx => accelerationInput = ctx.ReadValue<float>();
            inputActions["Driving/Accelerate"].canceled += ctx => accelerationInput = 0f;


            inputActions["Driving/Handbreak"].performed += ctx => isHandbraking = true;
            inputActions["Driving/Handbreak"].canceled += ctx => isHandbraking = false;

            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        private void FixedUpdate()
        {
            if (!IsOwner)
                return;
            HandleDownForce();
            if (!IsControllable)
                return;

            HandleSteering();
            HandleMotor();
            ApplyDrift();
            UpdateWheelPoses();
        }

        private void HandleDownForce()
        {
            rb.AddForce(Vector3.down * (rb.mass * 10), ForceMode.Force);
        }

        private void HandleSteering()
        {
            float targetSteerAngle = (IsDrifting ? currentSteerAngle : maxSteerAngle) * steeringInput;
            currentSteerAngle =
                IsDrifting
                    ? currentSteerAngle
                    : Mathf.Lerp(currentSteerAngle, targetSteerAngle, steerSpeed * Time.deltaTime);
            Wheels[0].steerAngle = currentSteerAngle;
            Wheels[1].steerAngle = currentSteerAngle;
        }

        private void HandleMotor()
        {
            accelerationInput = autoGas ? GetAccelerationValueWithAutoGas() : accelerationInput;
            Wheels[0].motorTorque = accelerationInput * motorForce * (isHandbraking ? 0 : 1);
            Wheels[1].motorTorque = accelerationInput * motorForce * (isHandbraking ? 0 : 1);

            if (isHandbraking)
            {
                Wheels[2].brakeTorque = brakeForce;
                Wheels[3].brakeTorque = brakeForce;
            }
            else
            {
                Wheels[2].brakeTorque = brakeInput * brakeForce;
                Wheels[3].brakeTorque = brakeInput * brakeForce;
            }
        }

        private float GetAccelerationValueWithAutoGas()
        {
            if (accelerationInput < 0)
                return accelerationInput;
            return isHandbraking ? Mathf.Min(accelerationInput, 0) : 1;
        }

        private void UpdateWheelPoses()
        {
            UpdateWheelPose(Wheels[0], frontLeftTransform);
            UpdateWheelPose(Wheels[1], frontRightTransform);
            UpdateWheelPose(Wheels[2], rearLeftTransform);
            UpdateWheelPose(Wheels[3], rearRightTransform);
        }

        private void UpdateWheelPose(WheelCollider collider, Transform transform)
        {
            Vector3 pos;
            Quaternion quat;
            collider.GetWorldPose(out pos, out quat);

            transform.position = pos;
            transform.rotation = quat;
        }


        private void ApplyDrift()
        {
            WheelFrictionCurve sidewaysFriction = Wheels[2].sidewaysFriction;
            if (isHandbraking)
            {
                sidewaysFriction.extremumValue = driftFactor;
                sidewaysFriction.asymptoteValue = driftFactor;
            }
            else
            {
                sidewaysFriction.extremumValue = 1f;
                sidewaysFriction.asymptoteValue = 1f;
            }

            Wheels[2].sidewaysFriction = sidewaysFriction;
            Wheels[3].sidewaysFriction = sidewaysFriction;

            if (isHandbraking)
            {
                Vector3 driftForce = transform.right * (rb.velocity.magnitude * driftControl * -steeringInput);
                rb.AddForce(driftForce, ForceMode.Acceleration);
            }

            isDriftingApplied = (rb.velocity.normalized - transform.forward).sqrMagnitude * accelerationInput >
                                (rb.velocity.normalized - transform.right * (-steeringInput)).sqrMagnitude *
                                accelerationInput; // Mathf.Abs(steeringInput)>driftTolerance;
        }
        
        private void SetupWheels()
        {
            SetupWheelColliders(frontLeftWheel, 0);
            SetupWheelColliders(frontRightWheel, 1);
            SetupWheelColliders(rearLeftWheel, 2);
            SetupWheelColliders(rearRightWheel, 3);
        }

        private void SetupWheelColliders(GameObject wheel, int index)
        {
            WheelCollider collider = wheel.AddComponent<WheelCollider>();
            WheelFrictionCurve curve = collider.forwardFriction;
            JointSpring spring = collider.suspensionSpring;

            Wheels[index] = collider;

            curve.asymptoteValue = forwardFriction;
            collider.forwardFriction = curve;
            curve.asymptoteValue = sidewaysFriction;
            curve.stiffness = 0.4f;
            collider.sidewaysFriction = curve;

            spring.spring = 75000;
            collider.suspensionSpring = spring;

            collider.radius = 0.37f;
            collider.suspensionDistance = suspensionDistance;
            collider.forceAppPointDistance = 0.05f;
            collider.center = new Vector3(0, suspensionOffset, 0);
        }

        public void SetIsControllable(bool value)
        {
            IsControllable = value;

            if (!IsControllable)
            {
                Wheels[0].motorTorque = 0;
                Wheels[1].motorTorque = 0;
                Wheels[2].brakeTorque = brakeForce;
                Wheels[3].brakeTorque = brakeForce;
            }
        }

        public void SetIsAutoGas(bool value)
        {
            autoGas = value;
        }

        public void SetPlayerData(PlayerData _playerData)
        {
            playerData = _playerData;
        }
    }
}