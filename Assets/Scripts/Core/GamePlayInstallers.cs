using Car;
using GameTools;
using Popup;
using UI;
using UnityEngine;
using Zenject;

namespace Core
{
    public class GamePlayInstallers : MonoInstaller
    {
        [SerializeField] private DriftCounter _driftCounter;
        [SerializeField] private GameTimer _gameTimer;
        [SerializeField] private HUD _hud;
        [SerializeField] private EndGamePopup _endGamePopup;
        [SerializeField] private CarSpawner _carSpawner;

        private CarController car;

        [Inject] private PlayerData _playerData;
        [Inject] private DiContainer _diContainer;
        
        public override void InstallBindings()
        {
            Container.BindInstance(_carSpawner);
            Container.BindInstance(_gameTimer);
            Container.BindInstance(_hud);
            car = _carSpawner.SpawnSelectedCar(_playerData, _diContainer);
            Container.BindInstance(_driftCounter);
            Container.BindInstance(car).AsSingle();
            Container.BindInstance(_endGamePopup);

            car.SetIsControllable(true);
        }
    }
}