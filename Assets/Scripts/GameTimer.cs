using System;
using Car;
using Unity.Netcode;
using UnityEngine;
using Zenject;

namespace GameTools
{
    public class GameTimer : MonoBehaviour
    {
        public static GameTimer Instance { get; private set; }
        
        [SerializeField] private float gameplayTime;

        private float timeLeft;

        public event Action OnGameplayEnd;
        public event Action<float> OnUpdateGameTimer;
        
        [Inject] private CarManager _carManager;

        private CarController car;

        protected void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
            
            if (!NetworkManager.Singleton.IsHost)
                return;
            Init();
        }

        private void Init()
        {
            if(_carManager == null)
                Debug.LogFormat("NO CAR MANAGER");
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
                car.SetIsControllable(false);
                OnUpdateGameTimer?.Invoke(0);
                OnGameplayEnd?.Invoke();
            }
        }
        
        public void SetCar(CarController _car = null)
        {
            if (_car != null)
                car = _car;
            else
                car = _carManager.GetCar();
            Init();
        }
    }
}