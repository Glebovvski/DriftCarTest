using System;
using Car;
using CI.QuickSave;
using UnityEngine;

namespace Core
{
    public class SaveManager
    {
        private const string SaveFileName = "PlayerData";

        private const string MetallicKey = "Metallic";
        private const string SmoothnessKey = "Smoothness";
        private const string ColorKey = "Color";
        private const string ControlTypeKey = "ControlType";
        private const string SelectedCarKey = "SelectedCar";

        private const string MusicVolumeKey = "MusicVolume";
        private const string SoundVolumeKey = "SoundVolume";

        private const string GoldKey = "Gold";
        private const string DriftPointsKey = "DriftPoints";

        #region Save

        public void Save(PlayerData data)
        {
            var writer = QuickSaveWriter.Create(SaveFileName);

            SaveCarSettings(writer, data.CarSettings);
            SaveGameSettings(writer, data.GameSettings);
            SavePlayerData(writer, data);
            writer.Commit();
        }

        public void SavePlayerData(PlayerData data)
        {
            var writer = QuickSaveWriter.Create(SaveFileName);
            SavePlayerData(writer, data);
            writer.Commit();
        }

        private void SavePlayerData(QuickSaveWriter writer, PlayerData data)
        {
            writer.Write(GoldKey, data.Gold);
            writer.Write(DriftPointsKey, data.DriftPoints);
        }

        public void SaveCarSettings(CarSettings car)
        {
            var writer = QuickSaveWriter.Create(SaveFileName);
            SaveCarSettings(writer, car);
            writer.Commit();
        }

        public void SaveGameSettings(GameSettings settings)
        {
            var writer = QuickSaveWriter.Create(SaveFileName);
            SaveGameSettings(writer, settings);
            writer.Commit();
        }

        private void SaveGameSettings(QuickSaveWriter writer, GameSettings settings)
        {
            writer.Write(MusicVolumeKey, settings.MusicVolume);
            writer.Write(SoundVolumeKey, settings.SoundVolume);
        }

        private void SaveCarSettings(QuickSaveWriter writer, CarSettings car)
        {
            writer.Write(MetallicKey, car.Metallic);
            writer.Write(SmoothnessKey, car.Smoothness);
            writer.Write(ColorKey, car.CarColor);
            writer.Write(ControlTypeKey, (int)car.ControlType);
            writer.Write(SelectedCarKey, (int)car.SelectedCar);
        }

        #endregion

        #region Load

        public void ReadPlayerData(PlayerData data)
        {
            try
            {
                var reader = QuickSaveReader.Create(SaveFileName);
                data.SetGold(reader.Read<int>(GoldKey));
                data.SetDriftPoints(reader.Read<int>(DriftPointsKey));
                ReadCarSettings(reader, data.CarSettings);
                ReadGameSettings(reader, data.GameSettings);
            }
            catch (Exception e)
            {
                Debug.Log("NO DATA TO READ");
            }
        }

        public void ReadCarSettings(QuickSaveReader reader, CarSettings settings)
        {
            settings.SetMetallic(reader.Read<float>(MetallicKey));
            settings.SetSmoothness(reader.Read<float>(SmoothnessKey));
            settings.SetCarColor(reader.Read<Vector4>(ColorKey));
            settings.SetControlType(reader.Read<int>(ControlTypeKey));
            settings.SetSelectedCar(reader.Read<int>(SelectedCarKey));
        }

        public void ReadGameSettings(QuickSaveReader reader, GameSettings settings)
        {
            settings.SetMusicVolume(reader.Read<float>(MusicVolumeKey));
            settings.SetSoundVolume(reader.Read<float>(SoundVolumeKey));
        }

        #endregion
    }
}