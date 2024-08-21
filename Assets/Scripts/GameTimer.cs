using System;
using UnityEngine;

namespace GameTools
{
    public class GameTimer : MonoBehaviour
    {
        [SerializeField] private float gameplayTime;

        private float timeLeft;

        public event Action OnGameplayEnd;
        public event Action<float> OnUpdateGameTimer;

        private void Awake()
        {
            timeLeft = gameplayTime * 60;
        }

        private void Update()
        {
            if (timeLeft <= 0)
                return;
            timeLeft -= Time.deltaTime;
            OnUpdateGameTimer?.Invoke(timeLeft);
            if (timeLeft <= 0)
            {
                OnUpdateGameTimer?.Invoke(0);
                OnGameplayEnd?.Invoke();
            }
        }
    }
}