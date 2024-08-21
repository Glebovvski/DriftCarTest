using GameTools;
using Popup;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Core
{
    public class MainMenuInstallers : MonoInstaller
    {
        [SerializeField] private SettingsPopup _settingsPopup;
        [SerializeField] private GaragePopup _garagePopup;
        [SerializeField] private IAPPopup _iapPopup;
        [SerializeField] private MainMenuPropsTransition _mainMenuPropsTransition;

        public override void InstallBindings()
        {
            Container.BindInstance(_settingsPopup);
            Container.BindInstance(_mainMenuPropsTransition);
            Container.BindInstance(_garagePopup);
            Container.BindInstance(_iapPopup);
        }
    }
}