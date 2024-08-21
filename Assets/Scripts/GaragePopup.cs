using System.Collections;
using System.Collections.Generic;
using Core;
using GameTools;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Popup
{
    public class GaragePopup : Popup
    {
        [SerializeField] private Button backBtn;
        [SerializeField] private FlexibleColorPicker colorPicker;
        [SerializeField] private Slider metallicSlider;
        [SerializeField] private Slider smoothnessSlider;

        [SerializeField] private Button nextCarBtn;
        [SerializeField] private Button prevCarBtn;
        [SerializeField] private Button buyBtn;
        [SerializeField] private Button selectBtn;
        [SerializeField] private Color affordableColor;
        [SerializeField] private Color notAffordableColor;
        [SerializeField] private CarSelector carSelector;
        
        
        [Inject] private MainMenuPropsTransition transitionManager;
        [Inject] private PlayerData playerData;

        private CarData selectedCarData;
        
        protected void Awake()
        {
            base.Awake();
            Init();
        }

        private void Init()
        {
            selectedCarData = carSelector.SelectCar();
            SetDefaultValues();
            backBtn.onClick.AddListener(Back);
            colorPicker.onColorChange.AddListener(OnColorChange);
            metallicSlider.onValueChanged.AddListener(OnMetallicChange);
            smoothnessSlider.onValueChanged.AddListener(OnSmoothnessChange);
        }

        private void SetDefaultValues()
        {
            colorPicker.SetColor(playerData.CarSettings.CarColor);
            metallicSlider.SetValueWithoutNotify(playerData.CarSettings.Metallic);
            smoothnessSlider.SetValueWithoutNotify(playerData.CarSettings.Smoothness);
        }

        private void OnSmoothnessChange(float value)
        {
            audio.Play(Sounds.BtnDrag);
            playerData.SetSmoothness(value);
        }

        private void OnMetallicChange(float value)
        {
            audio.Play(Sounds.BtnDrag);
            playerData.SetMetallic(value);
        }

        private void OnColorChange(Color value)
        {
            audio.Play(Sounds.BtnDrag);
            playerData.SetCarColor(value);
        }

        private void Back()
        {
            transitionManager.TransitionTo(PropTypes.Main,Hide);
        }
    }
}