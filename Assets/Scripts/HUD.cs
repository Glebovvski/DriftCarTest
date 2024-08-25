using System;
using System.Collections.Generic;
using Car;
using Core;
using DG.Tweening;
using GameTools;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace UI
{
    public class HUD : NetworkBehaviour
    {
        private const float BlipDuration = 1f;
        private const float BlipScale = 1.3f;

        [SerializeField] private TMP_Text driftCountText;
        [SerializeField] private TMP_Text gameplayTimerText;

        [SerializeField] private GameObject carControlBtns;

        [SerializeField] private Color warningColor;
        private Sequence gameTimerSequence;
        private Sequence driftCountSequence;

        private bool gamePlayTimerBlip = false;

        [SerializeField] private CarController car;


        private void OnEnable()
        {
            //     if (!NetworkManager.Singleton.IsHost)
            //         return;
            //     car = _carManager.GetCar();
            //     Init();
            // if (!NetworkManager.Singleton.IsHost)
            //     return;
            // Subscribe();
        }

        public void Init(CarController _car)
        {
            if (IsOwner)
            {
                car = _car;
                car.DriftCounter.OnUpdateDriftCounter += UpdateDriftCounter;
            // SetControlButtonsActive(car.PlayerData.CarSettings.ControlType == ControlType.Buttons);
            }
            else
            {
                gameObject.SetActive(false);
            }
            Subscribe();
        }

        private void Subscribe()
        {
            GameTimer.Instance.OnUpdateGameTimer += UpdateGamePlayTimer;

            // car.DriftCounter.OnUpdateDriftCounter += UpdateDriftCounter;
        }

        public void UpdateDriftCounter(int value)
        {
            driftCountSequence.Kill(true);
            driftCountSequence = DOTween.Sequence();
            driftCountSequence.Join(driftCountText.transform.DOScale(BlipScale, BlipDuration));
            driftCountSequence.Append(driftCountText.transform.DOScale(1, BlipDuration));

            driftCountSequence.Play();
            driftCountText.text = value.ToString();
        }

        public void UpdateGamePlayTimer(float value)
        {
            int minutes = Mathf.FloorToInt(value / 60);
            int remainingSeconds = Mathf.FloorToInt(value % 60);

            if (minutes == 0 && remainingSeconds <= 10)
            {
                BlipTimer();
            }

            if (minutes == 0 && remainingSeconds <= 0)
            {
                gameTimerSequence.Kill();
            }

            gameplayTimerText.text = string.Format("{0:00}:{1:00}", minutes, remainingSeconds);
        }

        private void BlipTimer()
        {
            if (gamePlayTimerBlip)
                return;

            gameTimerSequence.Kill();
            gameTimerSequence = DOTween.Sequence();
            gameTimerSequence.Append(gameplayTimerText.transform.DOScale(BlipScale, BlipDuration));
            gameTimerSequence.Join(gameplayTimerText.DOColor(warningColor, BlipDuration));
            gameTimerSequence.SetLoops(-1, LoopType.Yoyo);
            gameTimerSequence.Play();

            gamePlayTimerBlip = true;
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }

        private void Unsubscribe()
        {
            // car.DriftCounter.OnUpdateDriftCounter -= UpdateDriftCounter;
            // GameTimer.Instance.OnUpdateGameTimer -= UpdateGamePlayTimer;
        }

        public void SetControlButtonsActive(bool value)
        {
            carControlBtns.SetActive(value);
        }

        // public void SetCar(CarController _car = null)
        // {
        //     if (_car != null)
        //         car = _car;
        //     else
        //         car = _carManager.GetCar();
        //     // SetControlButtonsActive(car.PlayerData.CarSettings.ControlType == ControlType.Buttons);
        //     Init();
        // }
    }
}