using System;
using Car;
using Unity.Netcode;
using UnityEngine;
using Zenject;

namespace GameTools
{
    public class DriftCounter : NetworkBehaviour
    {
        private const float TimeBetweenCounterUpdate = 0.1f;

        private CarController car;
        private float lastDriftCountUpdate;

        private int driftCount = 0;

        public int DriftCount => driftCount;
        public event Action<int> OnUpdateDriftCounter;


        public void Init(CarController _car)
        {
            gameObject.SetActive(IsOwner);
            if (IsOwner)
            {
                car = _car;
                driftCount = 0;
            }
        }

        private void Update()
        {
            if (!IsOwner)
                return;

            if (car == null)
                return;

            if (!car.IsDrifting || !car.IsControllable)
                return;

            if (Time.time - lastDriftCountUpdate < TimeBetweenCounterUpdate)
                return;

            driftCount++;
            OnUpdateDriftCounter?.Invoke(driftCount);
            lastDriftCountUpdate = Time.time;
        }
    }
}