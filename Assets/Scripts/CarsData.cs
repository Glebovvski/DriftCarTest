using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CarKey
{
    Free,
    Alfa,
    Zis,
    Opel
}

[CreateAssetMenu(fileName = "Audio Data", menuName = "Audio/AudioData", order = 1)]
public class CarsData : ScriptableObject
{
    public List<CarData> Cars;
}

[Serializable]
public class CarData
{
    [field: SerializeField] public CarKey Car { get; private set; }
    [field: SerializeField] public bool IsBought { get; private set; }
    [field: SerializeField] public float MotorForce { get; private set; }
    [field: SerializeField] public float MaxSteerAngle { get; private set; }
    [field: SerializeField] public float SteerSpeed { get; private set; }
}