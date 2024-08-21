using System;
using Car;
using UnityEngine;

namespace Core
{
    public interface ISaveable
    {
        void Save();
    }

    public class GameSettings : ISaveable
    {
        public event Action OnSoundVolumeChanged;
        public event Action OnMusicVolumeChanged;

        public float MusicVolume
        {
            get => musicVolume;
            private set
            {
                musicVolume = value;
                OnMusicVolumeChanged?.Invoke();
            }
        }

        public float SoundVolume
        {
            get => soundVolume;
            private set
            {
                soundVolume = value;
                OnSoundVolumeChanged?.Invoke();
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
        }
    }

    public class CarSettings : ISaveable
    {
        private CarKey selectedCar = CarKey.Free;
        private Material carMaterial;
        private Texture carTexture;
        public CarSettings()
        {
            carMaterial = Resources.Load("Car") as Material;
        }
        
        public ControlType ControlType
        {
            get => controlType;
            private set { controlType = value; }
        }

        public CarKey SelectedCar
        {
            get => selectedCar;
            private set
            {
                selectedCar = value;
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
            }
        }

        public float Metallic
        {
            get => carMaterial.GetFloat(MetallicID);
            private set
            {
                metallic = value;
                carMaterial.SetFloat(MetallicID, value);
            }
        }
        
        public float Smoothness
        {
            get => carMaterial.GetFloat(Glossiness);
            private set
            {
                smoothness = value;
                carMaterial.SetFloat(Glossiness, value);
            }
        }
        //end car prop

        private ControlType controlType;
        private Color carColor;
        private float metallic;
        private float smoothness;
        private static readonly int MetallicID = Shader.PropertyToID("_Metallic");
        private static readonly int Glossiness = Shader.PropertyToID("_Glossiness");

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

        public void SetSelectedCar(CarKey value)
        {
            SelectedCar = value;
        }
        
        public void Save()
        {
        }
    }

    public class PlayerData : ISaveable
    {
        private readonly string goldKey = "gold";
        private readonly string driftKey = "drift";
        private readonly string carColorKey = "carColor";
        private readonly string metallicKey = "metallic";

        public PlayerData()
        {
            CarSettings = new();
            GameSettings = new();
        }

        #region Data

        public CarSettings CarSettings { get; private set; }
        public GameSettings GameSettings { get; private set; }

        public int Gold
        {
            get => gold;
            private set { gold = value; }
        }

        public int DriftPoints
        {
            get => driftPoints;
            private set { driftPoints = value; }
        }

        private int gold;
        private int driftPoints;

        #endregion

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
        }
    }
}