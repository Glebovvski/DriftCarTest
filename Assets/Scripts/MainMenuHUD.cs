using System;
using System.Collections;
using System.Collections.Generic;
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

        [Inject] private SettingsPopup settingsPopup;

        private void Awake()
        {
            settingsBtn.onClick.AddListener(settingsPopup.Show);
        }
    }
}