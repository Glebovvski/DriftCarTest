using Car;
using Core;
using DG.Tweening;
using GameTools;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Popup
{

    public class EndGamePopup : Popup
    {

        [SerializeField] private Image goldIcon;

        [SerializeField] private TMP_Text goldText;
        [SerializeField] private Button adBtn;
        [SerializeField] private Button exitBtn;

        [SerializeField] private RectTransform driftCounterTransform;

        [Inject] private CarManager _carManager;
        [Inject] private PlayerData playerData;
        [Inject] private GameManager gameManager;
        [Inject] private AdsManager adsManager;

        private CarController car;

        private int gold;


        // protected void Awake()
        // {
        //     if (!NetworkManager.Singleton.IsHost)
        //         return;
        //     car = _carManager.GetCar();
        //     Init();
        //     base.Awake();
        // }
        

        private void Init()
        {
            Subscribe();
            Hide();
        }

        private void Subscribe()
        {
            // GameTimer.Instance.OnGameplayEnd += Show;
            // adsManager.OnRewardedAdAvailable += SetAdBtnInteractable;
            adBtn.onClick.AddListener(TryShowRewardedAd);
            exitBtn.onClick.AddListener(Exit);
        }

        private void Exit()
        {
            playerData.SetGold(gold);
            playerData.SetDriftPoints(car.DriftCounter.DriftCount);
            gameManager.Exit();
        }

        private void TryShowRewardedAd()
        {
            adsManager.ShowRewarded(GetRewardForAd);
        }

        private void GetRewardForAd()
        {
            adBtn.interactable = false;
            var sequence = DOTween.Sequence();
            for (int i = 0; i < gold; i++)
            {
                sequence.JoinCallback(UpdateGoldText);
                sequence.AppendInterval(1f / (float)gold);
            }
        }

        private void SetAdBtnInteractable(bool value)
        {
            adBtn.interactable = value;
        }

        private void Unsubscribe()
        {
            GameTimer.Instance.OnGameplayEnd -= Show;
            adsManager.OnRewardedAdAvailable -= SetAdBtnInteractable;
        }

        private void CountGold()
        {
            var sequence = DOTween.Sequence();

            // Get the positions of the drift counter and the gold text
            Vector3 startPosition = driftCounterTransform.position;
            Vector3 endPosition = goldText.transform.position;

            for (int i = 0; i < car.DriftCounter.DriftCount; i++)
            {
                // Create a dummy Image to animate from the drift counter to the gold text
                GameObject dummyObject = new GameObject("GoldDummy");
                RectTransform goldRect = dummyObject.AddComponent<RectTransform>();
                Image goldImg = dummyObject.AddComponent<Image>();

                // Set the parent to bgImg's parent or the canvas for proper positioning
                goldRect.SetParent(bgImg.transform);
                goldRect.position = startPosition;

                // Optional: Set the dummy image's appearance
                goldImg.sprite = goldIcon.sprite; // Set to a relevant sprite
                goldImg.rectTransform.sizeDelta = new Vector2(65, 65);
                goldImg.raycastTarget = false; // Adjust size if necessary

                // Animate the dummy Image from driftCounterTransform to goldText position
                float jumpPower = 90f; // Adjust the jump height
                int numJumps = 1; // Number of jumps
                sequence.Append(goldRect
                    .DOJump(endPosition, jumpPower, numJumps, (float)1 / (float)car.DriftCounter.DriftCount)
                    .SetEase(Ease.OutQuad));

                // Scale the gold text when the dummy image reaches the position
                sequence.AppendCallback(UpdateGoldText);

                // Destroy the dummy Image after the animation
                sequence.AppendCallback(() => { Destroy(dummyObject); });
            }

            // Make the ad button interactable when the entire sequence is finished
            sequence.OnComplete(() => { adBtn.interactable = true; });

            sequence.Play();
        }

        private void UpdateGoldText()
        {
            goldText.transform.DOScale(1.2f, 0.2f).SetEase(Ease.OutBounce).OnComplete(() =>
            {
                goldText.transform.DOScale(1f, 0.2f); // Scale back to normal
            });

            // Increment the gold count and update the text
            gold++;
            goldText.text = gold.ToString();
        }

        public override void Show()
        {
            OnCompleteShow = CountGold;
            base.Show();
        }


        private void OnDestroy()
        {
            Unsubscribe();
        }

        public void SetCar(CarController _car = null)
        {
            if (_car != null)
                car = _car;
            else
                car = _carManager.GetCar();
            Init();
        }
    }
}