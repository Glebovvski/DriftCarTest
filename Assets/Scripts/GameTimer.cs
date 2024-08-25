using System;
using Unity.Netcode;
using UnityEngine;

namespace GameTools
{
    public class GameTimer : NetworkBehaviour
    {
        public static GameTimer Instance { get; private set; }

        [SerializeField] private float gameplayTime;

        private float timeLeft;

        public event Action OnGameplayEnd;
        public event Action<float> OnUpdateGameTimer;

        protected void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(this);
            }

            Init();
        }

        public void Init()
        {
            timeLeft = gameplayTime * 60;
        }

        private void Update()
        {
            if (timeLeft <= 0)
                return;
            if (NetworkManager.Singleton.IsHost)
            {
                timeLeft -= Time.deltaTime;
                SyncTimeClientRpc(timeLeft);
                OnUpdateGameTimer?.Invoke(timeLeft);
                if (timeLeft <= 0)
                {
                    OnUpdateGameTimer?.Invoke(0);
                    OnGameplayEnd?.Invoke();
                }
            }
            else
            {
                OnUpdateGameTimer?.Invoke(timeLeft);
                if (timeLeft <= 0)
                {
                    OnUpdateGameTimer?.Invoke(0);
                    OnGameplayEnd?.Invoke();
                }
            }
        }

        [ClientRpc]
        private void SyncTimeClientRpc(float hostTime)
        {
            if (!NetworkManager.Singleton.IsHost)
            {
                timeLeft = hostTime;
                if (timeLeft <= 0)
                {
                    OnUpdateGameTimer?.Invoke(0);
                    OnGameplayEnd?.Invoke();
                }
            }
        }
    }
}