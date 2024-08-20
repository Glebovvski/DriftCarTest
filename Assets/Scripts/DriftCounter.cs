using System;
using UnityEngine;
using UnityEngine.Serialization;

public class DriftCounter : MonoBehaviour, IComponent
{
    private const float TimeBetweenCounterUpdate = 0.1f;
    
    private int driftCount;
    private float lastDriftCountUpdate;
    private CarController car;

    public int DriftCount => driftCount;
    public event Action<int> OnUpdateDriftCounter; 

    private void Start()
    {
        driftCount = 0;
    }

    private void Update()
    {
        if (!car.IsDrifting || !car.IsControllable)
            return;

        if (Time.time - lastDriftCountUpdate < TimeBetweenCounterUpdate)
            return;
        
        driftCount++;
        lastDriftCountUpdate = Time.time;
        OnUpdateDriftCounter?.Invoke(driftCount);
    }

    public void Init(MonoBehaviour behaviour)
    {
        car = (CarController)behaviour;
    }
}
