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
            Container.Bind<PlayerData>().FromNew().AsSingle();
            Container.BindInstance(_iapManager);
            Container.BindInstance(_adsManager);
            Container.BindInstance(_audioManager);
        }
    }
}