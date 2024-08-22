using System.Collections;
using System.Collections.Generic;
using Core;
using DG.Tweening;
using GameTools;
using TMPro;
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
        [SerializeField] private TMP_Text priceText;
        [SerializeField] private Button selectBtn;
        [SerializeField] private Color affordableColor;
        [SerializeField] private Color notAffordableColor;
        [SerializeField] private CarSelector carSelector;

        [SerializeField] private Slider motorSlider;
        [SerializeField] private Slider angleSlider;
        [SerializeField] private Slider steeringSlider;


        [Inject] private MainMenuPropsTransition transitionManager;
        [Inject] private PlayerData playerData;
        [Inject] private IAPPopup iapPopup;

        private CarData selectedCarData;

        protected void Awake()
        {
            base.Awake();
            Init();
        }

        protected override void Hide()
        {
            selectedCarData = carSelector.SelectCar();
            UpdateCar();
            base.Hide();
        }

        private void Init()
        {
            selectedCarData = carSelector.SelectCar();
            UpdateCar();

            SetDefaultValues();
            backBtn.onClick.AddListener(Back);
            colorPicker.onColorChange.AddListener(OnColorChange);
            metallicSlider.onValueChanged.AddListener(OnMetallicChange);
            smoothnessSlider.onValueChanged.AddListener(OnSmoothnessChange);
            nextCarBtn.onClick.AddListener(OnNextCarClick);
            prevCarBtn.onClick.AddListener(OnPrevCarClick);
            buyBtn.onClick.AddListener(PurchaseCarBtnClick);
            selectBtn.onClick.AddListener(SelectCar);
        }

        private void SelectCar()
        {
            playerData.CarSettings.SetSelectedCar(selectedCarData.Car);
            UpdateCarInfo();
        }

        private void PurchaseCarBtnClick()
        {
            audio.Play(Sounds.BtnClick);
            if (playerData.TryPurchase(selectedCarData.Price))
            {
                selectedCarData.SetIsBought(true);
            }
            else
            {
                iapPopup.Show();
            }

            UpdateCarInfo();
        }

        private void UpdateCar()
        {
            audio.Play(Sounds.BtnClick);
            SetCarTextures();
            UpdateCarInfo();
        }

        private void UpdateCarInfo()
        {
            priceText.text = selectedCarData.Price.ToString();
            buyBtn.image.color = playerData.CanPurchase(selectedCarData.Price) ? affordableColor : notAffordableColor;

            buyBtn.gameObject.SetActive(!selectedCarData.IsBought);
            selectBtn.gameObject.SetActive(selectedCarData.IsBought &&
                                           playerData.CarSettings.SelectedCar != selectedCarData.Car);

            UpdateSliderValue(motorSlider, selectedCarData.MotorForce);
            UpdateSliderValue(angleSlider, selectedCarData.MaxSteerAngle);
            UpdateSliderValue(steeringSlider, selectedCarData.SteerSpeed);
        }

        private void UpdateSliderValue(Slider slider, float value)
        {
            DOTween.To(() => slider.value, x => 
            {
                slider.value = x;
            }, value, 1f).SetEase(Ease.InOutQuad).Play();
        }

        private void OnPrevCarClick()
        {
            selectedCarData = carSelector.SelectPrevCar(selectedCarData.Car);
            UpdateCar();
        }

        private void OnNextCarClick()
        {
            selectedCarData = carSelector.SelectNextCar(selectedCarData.Car);
            UpdateCar();
        }

        private void SetDefaultValues()
        {
            colorPicker.SetColor(playerData.CarSettings.CarColor);
            metallicSlider.SetValueWithoutNotify(playerData.CarSettings.Metallic);
            smoothnessSlider.SetValueWithoutNotify(playerData.CarSettings.Smoothness);
        }

        private void SetCarTextures()
        {
            playerData.CarSettings.SetTexture(selectedCarData.CarTexture, TextureType.Albedo);
            playerData.CarSettings.SetTexture(selectedCarData.CarMetallic, TextureType.Metallic);
            playerData.CarSettings.SetTexture(selectedCarData.CarNormal, TextureType.Normal);
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
            transitionManager.TransitionTo(PropTypes.Main, Hide);
        }
    }
}