using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;
using Zenject;

namespace GameTools
{
    public class CarSelector : MonoBehaviour
    {
        [SerializeField] private CarsData data;
        [SerializeField] private List<CarProp> cars;

        [Inject] private PlayerData _playerData;

        private void Awake()
        {
            data.Init();
            SelectCar();
        }

        public CarData SelectCar()
        {
            var carData = data.Cars.FirstOrDefault(x => x.CarKey == _playerData.CarSettings.SelectedCar);
            if (carData == null)
                carData = data.Cars[0];

            UpdateActiveCar(carData);

            return carData;
        }

        private void UpdateActiveCar(CarData carData)
        {
            foreach (var car in cars)
            {
                car.Toggle(car.Car == carData.CarKey);
            }
        }

        public CarData SelectPrevCar(CarKey car)
        {
            int index = data.Cars.IndexOf(data.Cars.FirstOrDefault(x => x.CarKey == car)) - 1;
            if (index < 0)
                index = data.Cars.Count - 1;
            var carData = data.Cars[index];
            UpdateActiveCar(data.Cars[index]);
            return carData;
        }

        public CarData SelectNextCar(CarKey car)
        {
            int index = data.Cars.IndexOf(data.Cars.FirstOrDefault(x => x.CarKey == car)) + 1;
            if (index >= data.Cars.Count)
                index = 0;
            var carData = data.Cars[index];
            UpdateActiveCar(data.Cars[index]);
            return carData;
        }
    }
}