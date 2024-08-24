using Car;
using GameTools;
using Popup;
using UI;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Core
{
    public class GamePlayInstallers : MonoInstaller
    {
        [SerializeField] private HUD _hud;
        [SerializeField] private EndGamePopup _endGamePopup;

        [FormerlySerializedAs("_carSpawner")] [SerializeField]
        private CarManager carManager;

        private CarController car;

        [Inject] private PlayerData _playerData;
        [Inject] private DiContainer _diContainer;

        public override void InstallBindings()
        {
            // car = carManager.SpawnSelectedCar(_playerData, _diContainer);
            Container.BindInstance(carManager);
            Container.BindInstance(_hud);
            Container.BindInstance(_endGamePopup);
            // if (NetworkManager.Singleton.IsHost)
            // {
            //     _hud.SetCar(car);
            //     _endGamePopup.SetCar(car);
            //     _playerData.SetCar(carManager, car);
            // }
        }
    }
}