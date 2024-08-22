using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using DG.Tweening;
using GameTools;
using Popup;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class MainMenuHUD : MonoBehaviour
    {
        [SerializeField] private Button garageBtn;
        [SerializeField] private Button settingsBtn;

        [SerializeField] private TMP_Text goldText;
        [SerializeField] private Button iapBtn;
        
        [SerializeField] private Button singlePlayerBtn;
        [SerializeField] private Button onlineBtn;

        [Inject] private MainMenuPropsTransition transitionManager;

        [Inject] private SettingsPopup settingsPopup;
        [Inject] private GaragePopup garagePopup;
        [Inject] private IAPPopup iapPopup;
        [Inject] private PlayerData playerData;


        private void Awake()
        {
            Subscribe();
        }

        private void Subscribe()
        {
            playerData.OnGoldAmountChanged += UpdateGold;
            UpdateGold(playerData.Gold, playerData.Gold);
            settingsBtn.onClick.AddListener(settingsPopup.Show);
            garageBtn.onClick.AddListener(OpenGarage);
            iapBtn.onClick.AddListener(iapPopup.Show);
        }
        
        private void UpdateGold(int odlValue, int newValue)
        {
            DOTween.To(() => odlValue, x => 
            {
                odlValue = x;
                goldText.text = odlValue.ToString();
            }, newValue, 2f).SetEase(Ease.InOutQuad);

            // Tween to scale the text
            goldText.transform.DOScale(1.3f, 1f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                // Scale back to normal size after the animation
                goldText.transform.DOScale(1f, 1f).SetEase(Ease.InQuad);
            });
        }

        private void OpenGarage()
        {
            transitionManager.TransitionTo(PropTypes.Garage, garagePopup.Show);
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }

        private void Unsubscribe()
        {
            playerData.OnGoldAmountChanged -= UpdateGold;
            settingsBtn.onClick.RemoveListener(settingsPopup.Show);
            garageBtn.onClick.RemoveListener(OpenGarage);
        }
    }
}