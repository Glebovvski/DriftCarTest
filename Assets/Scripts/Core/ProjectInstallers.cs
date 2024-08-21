using UnityEngine;
using Zenject;

namespace Core
{
    public class ProjectInstallers : MonoInstaller
    {
        [SerializeField] private AdsManager _adsManager;
        [SerializeField] private AudioManager _audioManager;

        public override void InstallBindings()
        {
            Container.Bind<PlayerData>().FromNew().AsSingle();
            Container.BindInstance(_adsManager);
            Container.BindInstance(_audioManager);
        }
    }
}