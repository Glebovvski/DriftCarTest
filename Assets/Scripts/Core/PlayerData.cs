using System;
using Car;
using UnityEngine;
using Zenject;

namespace Core
{
    public enum TextureType
    {
        Albedo = 0,
        Metallic = 1,
        Normal = 2,
    }

    public interface ISaveable
    {
        void Save();
    }

    public class GameSettings : ISaveable
    {
        public event Action OnSoundVolumeChanged;
        public event Action OnMusicVolumeChanged;
        private SaveManager _saveManager;

        public GameSettings(SaveManager saveManager)
        {
            _saveManager = saveManager;
        }

        public float MusicVolume
        {
            get => musicVolume;
            private set
            {
                musicVolume = value;
                OnMusicVolumeChanged?.Invoke();
                Save();
            }
        }

        public float SoundVolume
        {
            get => soundVolume;
            private set
            {
                soundVolume = value;
                OnSoundVolumeChanged?.Invoke();
                Save();
            }
        }

        private float musicVolume;
        private float soundVolume;

        public void SetMusicVolume(float value)
        {
            MusicVolume = value;
        }

        public void SetSoundVolume(float value)
        {
            SoundVolume = value;
        }

        public void Save()
        {
            _saveManager.SaveGameSettings(this);
        }
    }

    public class CarSettings : ISaveable
    {
        private CarKey selectedCar = CarKey.Free;
        private Material carMaterial;
        private Texture carTexture;
        private Texture carNormalTexture;
        private Texture carMetallicTexture;
        private Texture carRoughnessTexture;

        private SaveManager _saveManager;

        public CarSettings(SaveManager saveManager)
        {
            _saveManager = saveManager;
            carMaterial = Resources.Load("Car") as Material;
        }

        public ControlType ControlType
        {
            get => controlType;
            private set
            {
                controlType = value;
                Save();
            }
        }

        public CarKey SelectedCar
        {
            get => selectedCar;
            private set
            {
                selectedCar = value;
                Save();
            }
        }

        //car prop
        public Color CarColor
        {
            get => carMaterial.color;
            private set
            {
                carColor = value;
                carMaterial.color = value;
                Save();
            }
        }

        public float Metallic
        {
            get => carMaterial.GetFloat(MetallicID);
            private set
            {
                metallic = value;
                carMaterial.SetFloat(MetallicID, value);
                Save();
            }
        }

        public float Smoothness
        {
            get => carMaterial.GetFloat(Glossiness);
            private set
            {
                smoothness = value;
                carMaterial.SetFloat(Glossiness, value);
                Save();
            }
        }
        //end car prop

        private ControlType controlType;
        private Color carColor;
        private float metallic;
        private float smoothness;
        private static readonly int MetallicID = Shader.PropertyToID("_Metallic");
        private static readonly int Glossiness = Shader.PropertyToID("_Glossiness");
        private static readonly int MainTex = Shader.PropertyToID("_MainTex");
        private static readonly int MetallicGlossMap = Shader.PropertyToID("_MetallicGlossMap");
        private static readonly int BumpMap = Shader.PropertyToID("_BumpMap");

        public void SetCarColor(Color value)
        {
            CarColor = value;
        }

        public void SetMetallic(float value)
        {
            Metallic = value;
        }

        public void SetSmoothness(float value)
        {
            Smoothness = value;
        }

        public void SetControlType(ControlType value)
        {
            ControlType = value;
        }

        public void SetControlType(int value)
        {
            ControlType = (ControlType)value;
        }

        public void SetSelectedCar(CarKey value)
        {
            SelectedCar = value;
        }

        public void SetSelectedCar(int value)
        {
            SelectedCar = (CarKey)value;
        }

        public void SetTexture(Texture value, TextureType type)
        {
            switch (type)
            {
                case TextureType.Albedo:
                    carMaterial.SetTexture(MainTex, value);
                    break;
                case TextureType.Metallic:
                    carMaterial.SetTexture(MetallicGlossMap, value);
                    break;
                case TextureType.Normal:
                    carMaterial.SetTexture(BumpMap, value);
                    break;
            }
        }

        public void Save()
        {
            _saveManager.SaveCarSettings(this);
        }
    }

    public class PlayerData : ISaveable
    {
        private SaveManager _saveManager;

        private CarController car;

        [Inject]
        public PlayerData(SaveManager saveManager)
        {
            _saveManager = saveManager;
            CarSettings = new(_saveManager);
            GameSettings = new(_saveManager);
            _saveManager.ReadPlayerData(this);
        }

        #region Data

        public CarSettings CarSettings { get; private set; }
        public GameSettings GameSettings { get; private set; }

        public int Gold
        {
            get => gold;
            private set
            {
                OnGoldAmountChanged?.Invoke(gold, value);
                gold = value;
                Save();
            }
        }

        public int DriftPoints
        {
            get => driftPoints;
            private set
            {
                driftPoints = value;
                Save();
            }
        }

        private int gold;
        private int driftPoints;

        #endregion

        public event Action<int, int> OnGoldAmountChanged;

        public void SetGold(int value)
        {
            Gold = value;
        }

        public void SetDriftPoints(int value)
        {
            DriftPoints = value;
        }

        public void SetCarColor(Color value)
        {
            CarSettings.SetCarColor(value);
        }

        public void SetMetallic(float value)
        {
            CarSettings.SetMetallic(value);
        }

        public void SetSmoothness(float value)
        {
            CarSettings.SetSmoothness(value);
        }

        public void SetSelectedCar(CarKey value) => CarSettings.SetSelectedCar(value);


        public void SetControlType(ControlType value)
        {
            CarSettings.SetControlType(value);
        }

        public void SetMusicVolume(float value) => GameSettings.SetMusicVolume(value);
        public void SetSoundVolume(float value) => GameSettings.SetSoundVolume(value);

        public void Save()
        {
            _saveManager.Save(this);
        }

        public bool TryPurchase(int price)
        {
            if (Gold >= price)
            {
                Gold -= price;
                return true;
            }

            return false;
        }

        public bool CanPurchase(int price)
        {
            return Gold >= price;
        }

        public void AddGold(int value)
        {
            Gold += value;
        }

        public void SetCar(CarManager manager, CarController _car = null)
        {
            if (_car != null)
                car = _car;
            else
                car = manager.GetCar();
            car.SetIsAutoGas(CarSettings.ControlType == ControlType.Buttons);
        }
    }
}