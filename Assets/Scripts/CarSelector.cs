using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;
using Zenject;

public class CarSelector : MonoBehaviour
{
    [SerializeField] private CarsData data;
    [SerializeField] private List<CarProp> cars;

    [Inject] private PlayerData _playerData;

    public CarData SelectCar()
    {
        var carData = data.Cars.FirstOrDefault(x=> x.Car == _playerData.CarSettings.SelectedCar);
        if (carData == null)
            carData = data.Cars[0];
        
        foreach (var car in cars)
        {
            car.Toggle(car.Car == carData.Car);
        }

        return carData;
    }
}
