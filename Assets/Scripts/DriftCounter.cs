using System;
using UnityEngine;
using Zenject;

public class DriftCounter : MonoBehaviour
{
    private const float TimeBetweenCounterUpdate = 0.1f;
    
    private int driftCount;
    private float lastDriftCountUpdate;

    public int DriftCount => driftCount;
    public event Action<int> OnUpdateDriftCounter;

    [Inject] private CarController car;

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
}
