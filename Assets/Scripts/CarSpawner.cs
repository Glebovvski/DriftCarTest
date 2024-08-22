using System.Linq;
using Car;
using Core;
using UnityEngine;
using Zenject;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] private CarsData data;
    [SerializeField] private Transform spawnPos;

    private PlayerData _playerData;
    private DiContainer _diContainer;

    // [Inject]
    public CarController SpawnSelectedCar(PlayerData playerData, DiContainer diContainer)
    {
        _playerData = playerData;
        _diContainer = diContainer;
        if (_playerData == null)
            Debug.LogError("PLAYER DATA IS NULL");
        var carKey = _playerData.CarSettings.SelectedCar;
        var carData = data.Cars.FirstOrDefault(x => x.CarKey == carKey);

        var car = _diContainer.InstantiatePrefab(carData.Car,spawnPos.position, Quaternion.identity, null).GetComponent<CarController>();
        car.SetCharacteristics(carData);
        return car;
    }
}