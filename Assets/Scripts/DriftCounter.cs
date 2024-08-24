using System;
using Car;
using UnityEngine;
using Zenject;

namespace GameTools
{
    public class DriftCounter : MonoBehaviour
    {
        private const float TimeBetweenCounterUpdate = 0.1f;

        private CarController car;
        private int driftCount;
        private float lastDriftCountUpdate;

        public int DriftCount => driftCount;
        public event Action<int> OnUpdateDriftCounter;


        private void Start()
        {
            car = GetComponent<CarController>();
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
}