using System;
using System.Collections;
using System.Collections.Generic;
using Car;
using Core;
using UnityEngine;

public enum CarKey
{
    Free,
    Alfa,
    Zis,
    Opel
}

[CreateAssetMenu(fileName = "Cars Data", menuName = "Cars/Cars Data", order = 1)]
public class CarsData : ScriptableObject, ISaveable
{
    public List<CarData> Cars;
    public void Save()
    {
        
    }

    public void Init()
    {
        foreach (var car in Cars)
        {
            car.OnPurchase += Save;
        }
    }
}

[Serializable]
public class CarData
{
    [field: SerializeField] public CarKey CarKey { get; private set; }
    [field: SerializeField] public bool IsBought { get; private set; }
    [field: SerializeField] public float MotorForce { get; private set; }
    [field: SerializeField] public float MaxSteerAngle { get; private set; }
    [field: SerializeField] public float SteerSpeed { get; private set; }
    [field: SerializeField] public int Price { get; private set; }
    [field: SerializeField] public Texture CarTexture { get; private set; }
    [field: SerializeField] public Texture CarMetallic { get; private set; }
    [field: SerializeField] public Texture CarNormal { get; private set; }
    [field: SerializeField] public CarController Car;

    public event Action OnPurchase;
    
    public void SetIsBought(bool value)
    {
        IsBought = value;
        OnPurchase?.Invoke();
    }
}