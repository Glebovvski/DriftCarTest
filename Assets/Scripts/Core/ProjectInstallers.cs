using Unity.Netcode;
using UnityEngine;
using Zenject;

namespace Core
{
    public class ProjectInstallers : MonoInstaller
    {
        [SerializeField] private AdsManager _adsManager;
        [SerializeField] private AudioManager _audioManager;
        [SerializeField] private IAPManager _iapManager;

        public override void InstallBindings()
        {
            Container.Bind<SaveManager>().AsSingle();
            Container.Bind<PlayerData>().AsSingle();
            Container.Bind<GameManager>().AsSingle();
            Container.BindInstance(_iapManager);
            Container.BindInstance(_adsManager);
            Container.BindInstance(_audioManager);
        }
    }
}