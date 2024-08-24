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

        // Use a NetworkVariable to synchronize drift count
        private int driftCount = 0;

        public int DriftCount => driftCount;
        public event Action<int> OnUpdateDriftCounter;


        public void Init(CarController _car)
        {
            if (IsOwner)
            {
                car = _car;
                driftCount = 0;
                
            }
            else
            {
                Debug.LogError("DISABLE DRIFT");
                gameObject.SetActive(false);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void HandleCounterServerRpc()
        {
            if (!IsOwner)
                return;

            if (car == null)
            {
                if (!NetworkManager.Singleton.IsHost)
                    Debug.LogError("CAR IS NULL");
                return;
            }

            if (!car.IsDrifting || !car.IsControllable)
            {
                if (!NetworkManager.Singleton.IsHost)
                    Debug.LogError("CAR IS DRIFTING: " + car.IsDrifting + "\n IS CONTROL: " + car.IsControllable);
                return;
            }

            if (Time.time - lastDriftCountUpdate < TimeBetweenCounterUpdate)
            {
                if (!NetworkManager.Singleton.IsHost)
                    Debug.LogError(Time.time - lastDriftCountUpdate);
                return;
            }

            driftCount++;
            OnUpdateDriftCounter?.Invoke(driftCount);
            lastDriftCountUpdate = Time.time;
        }

        private void Update()
        {
            if (!IsOwner)
                return;

            if (car == null)
            {
                if (!NetworkManager.Singleton.IsHost)
                    Debug.LogError("CAR IS NULL");
                return;
            }

            if (!car.IsDrifting || !car.IsControllable)
            {
                if (!NetworkManager.Singleton.IsHost)
                    Debug.LogError("CAR IS DRIFTING: " + car.IsDrifting + "\n IS CONTROL: " + car.IsControllable);
                return;
            }

            if (Time.time - lastDriftCountUpdate < TimeBetweenCounterUpdate)
            {
                if (!NetworkManager.Singleton.IsHost)
                    Debug.LogError(Time.time - lastDriftCountUpdate);
                return;
            }

            driftCount++;
            OnUpdateDriftCounter?.Invoke(driftCount);
            lastDriftCountUpdate = Time.time;
        }
    }
}