using System;
using System.Collections;
using System.Collections.Generic;
using GameTools;
using Popup;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class MainMenuHUD : MonoBehaviour
    {
        [SerializeField] private Button garageBtn;
        [SerializeField] private Button settingsBtn;

        [SerializeField] private Button singlePlayerBtn;
        [SerializeField] private Button onlineBtn;

        [Inject] private MainMenuPropsTransition transitionManager;

        [Inject] private SettingsPopup settingsPopup;
        [Inject] private GaragePopup garagePopup;


        private void Awake()
        {
            settingsBtn.onClick.AddListener(settingsPopup.Show);
            garageBtn.onClick.AddListener(OpenGarage);
        }

        private void OpenGarage()
        {
            transitionManager.TransitionTo(PropTypes.Garage, garagePopup.Show);
        }
    }
}