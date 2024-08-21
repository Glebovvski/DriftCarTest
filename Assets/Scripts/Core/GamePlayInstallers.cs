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
        [SerializeField] private CarController _car;

        public override void InstallBindings()
        {
            Container.BindInstance(_car);
            Container.BindInstance(_gameTimer);
            Container.BindInstance(_driftCounter);
            Container.BindInstance(_endGamePopup);
            Container.BindInstance(_hud);

            _car.SetIsControllable(true);
        }
    }
}