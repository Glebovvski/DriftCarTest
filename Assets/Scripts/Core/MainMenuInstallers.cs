using Popup;
using UnityEngine;
using Zenject;

namespace Core
{
    public class MainMenuInstallers : MonoInstaller
    {
        [SerializeField] private SettingsPopup _settingsPopup;

        public override void InstallBindings()
        {
            Container.BindInstance(_settingsPopup);
        }
    }
}