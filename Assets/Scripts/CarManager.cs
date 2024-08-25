using System;
using System.Collections.Generic;
using System.Linq;
using Car;
using Core;
using Unity.Netcode;
using UnityEngine;

public class PlayerCarData 
{
    public ulong ClientID;
    public CarController Car;
    public ControlType ControlType;
}

public class CarManager : NetworkBehaviour
{
    [SerializeField] private CarsData data;
    [SerializeField] private List<Transform> clientSpawnPositions;

    private List<PlayerCarData> PlayerCarData;


    private void Awake()
    {
        PlayerCarData = new();
    }

    public CarController SpawnClientCar(ulong id, PlayerData playerData, ControlType type)
    {
        var car = SpawnCar(id, playerData,type);
        
        return car;
    }

    private CarController SpawnCar(ulong id, PlayerData playerData, ControlType type)
    {
        var carKey =  playerData.CarSettings.SelectedCar;
        var carData = data.Cars.FirstOrDefault(x => x.CarKey == carKey);

        int index = Mathf.Clamp(PlayerCarData.Count, 0, clientSpawnPositions.Count);
        Vector3 spawnPos = clientSpawnPositions[index].position;

        var car = Instantiate(carData.Car, spawnPos, Quaternion.identity);
        PlayerCarData.Add(new PlayerCarData()
        {
            ClientID = id,
            Car = car,
            ControlType = type
        });
        // car.SetPlayerData(playerData);
        car.Init(this);
        var carNetwork = car.GetComponent<NetworkObject>();
        carNetwork.SpawnWithOwnership(id);
        car.SetCharacteristics(carData);
        return car;
    }

    private PlayerCarData GetClientCar(ulong id) => PlayerCarData.Find(x => x.ClientID == id);

    public CarController GetCar()
    {
        return GetClientCar(NetworkManager.Singleton.LocalClient.ClientId).Car;
    }

    public Vector3? GetPosition(int index)
    {
        index = Mathf.Clamp(index, 0, clientSpawnPositions.Count-1);
        return clientSpawnPositions[index].position;
    }

    public ControlType GetControlType(ulong id)
    {
        return GetClientCar(id).ControlType;
    }
}