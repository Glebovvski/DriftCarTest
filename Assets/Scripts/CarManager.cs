using System;
using System.Collections.Generic;
using System.Linq;
using Car;
using Core;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

[Serializable]
public class PlayerCarData
{
    public ulong ClientID;
    public CarController Car;
}

public class CarManager : NetworkBehaviour // MonoBehaviour
{
    [SerializeField] private CarsData data;
    [SerializeField] private Transform hostSpawnPos;

    [SerializeField] private List<Transform> clientSpawnPositions;

    // private PlayerData _playerData;
    [Inject] private DiContainer _diContainer;

    [SerializeField] private List<PlayerCarData> PlayerCarData;

    private void Awake()
    {
        PlayerCarData = new();
    }

    public void SpawnClientCar(ulong id, PlayerData playerData)
    {
        // if (_playerData == null || _diContainer == null)
        //     return null;
        var car = SpawnCar(id, playerData);
        PlayerCarData.Add(new PlayerCarData()
        {
            ClientID = id,
            Car = car
        });
    }

    private CarController SpawnCar(ulong id, PlayerData playerData)
    {
        var carKey = playerData.CarSettings.SelectedCar;
        var carData = data.Cars.FirstOrDefault(x => x.CarKey == carKey);

        int index = Mathf.Clamp(PlayerCarData.Count, 0, clientSpawnPositions.Count);
        Vector3 spawnPos = clientSpawnPositions[index].position;

        var car = Instantiate(carData.Car, spawnPos, Quaternion.identity);
        var carNetwork = car.GetComponent<NetworkObject>();

        carNetwork.SpawnWithOwnership(id, true);
        car.SetCharacteristics(carData);
        return car;
    }

    public CarController GetClientCar(ulong id) => PlayerCarData.Find(x => x.ClientID == id).Car;

    public CarController GetCar()
    {
        return GetClientCar(NetworkManager.Singleton.LocalClient.ClientId);
    }

    public Vector3? GetPosition()
    {
        int index = Mathf.Clamp(PlayerCarData.Count, 0, clientSpawnPositions.Count - 1);
        return clientSpawnPositions[index].position;
    }

    public Vector3? GetPosition(int index)
    {
        return clientSpawnPositions[index].position;
    }
}