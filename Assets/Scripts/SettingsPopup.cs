using System;
using System.Collections.Generic;
using Car;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace Popup
{
    public class SettingsPopup : Popup
    {
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider soundSlider;
        [SerializeField] private TMP_Dropdown controlTypes;

        [FormerlySerializedAs("clsoeBtn")] [SerializeField]
        private Button closeBtn;

        [Inject] private PlayerData playerData;

        private void Awake()
        {
            InitSettings();
            base.Awake();
        }

        private void InitSettings()
        {
            closeBtn.onClick.AddListener(Hide);
            SetInput();
            SetMusic();
            SetSound();
        }

        private void SetMusic()
        {
            musicSlider.onValueChanged.AddListener(UpdateMusicVolume);
        }

        private void SetSound()
        {
            soundSlider.onValueChanged.AddListener(UpdateSoundVolume);
        }

        private void UpdateSoundVolume(float value)
        {
            audio.Play(Sounds.BtnDrag);
            playerData.SetSoundVolume(value);
        }

        private void UpdateMusicVolume(float value)
        {
            audio.Play(Sounds.BtnDrag);
            playerData.SetMusicVolume(value);
        }

        private void SetInput()
        {
            controlTypes.options = new();
            foreach (var type in (ControlType[])Enum.GetValues(typeof(ControlType)))
            {
                controlTypes.options.Add(new(type.ToString()));
            }

            controlTypes.onValueChanged.AddListener(UpdateControlType);
        }

        private void UpdateControlType(int value)
        {
            audio.Play(Sounds.BtnClick);
            ControlType controlType = ((ControlType[])Enum.GetValues(typeof(ControlType)))[value];
            playerData.SetControlType(controlType);
        }

        public override void Show()
        {
            OnCompleteShow = SetDefaultValues;
            base.Show();
        }

        private void SetDefaultValues()
        {
            controlTypes.SetValueWithoutNotify((int)playerData.CarSettings.ControlType);
            soundSlider.SetValueWithoutNotify(playerData.GameSettings.SoundVolume);
            musicSlider.SetValueWithoutNotify(playerData.GameSettings.MusicVolume);
        }
    }
}