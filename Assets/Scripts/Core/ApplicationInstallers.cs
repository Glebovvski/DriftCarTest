using UnityEngine;
using Zenject;

public class ApplicationInstallers : MonoInstaller
{
    [SerializeField] private DriftCounter _driftCounter;
    [SerializeField] private GameTimer _gameTimer;
    [SerializeField] private HUD _hud;
    [SerializeField] private EndGamePopup _endGamePopup;
    [SerializeField] private CarController _car;

    [SerializeField] private AdsManager _adsManager;
    
    public override void InstallBindings()
    {
        Container.BindInstance(_car);
        Container.BindInstance(_gameTimer);
        Container.BindInstance(_driftCounter);
        Container.BindInstance(_endGamePopup);
        Container.BindInstance(_hud);
        Container.BindInstance(_adsManager);
        
        _car.SetIsControllable(true);
    }
}