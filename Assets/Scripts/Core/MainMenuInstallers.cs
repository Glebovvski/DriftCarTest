using GameTools;
using Popup;
using UnityEngine;
using Zenject;

namespace Core
{
    public class MainMenuInstallers : MonoInstaller
    {
        [SerializeField] private SettingsPopup _settingsPopup;
        [SerializeField] private MainMenuPropsTransition _mainMenuPropsTransition;

        public override void InstallBindings()
        {
            Container.BindInstance(_settingsPopup);
            Container.BindInstance(_mainMenuPropsTransition);
        }
    }
}