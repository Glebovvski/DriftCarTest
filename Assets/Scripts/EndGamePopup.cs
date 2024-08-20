using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGamePopup : MonoBehaviour, IComponent
{
    [SerializeField] private Image bgImg;
    [SerializeField] private Color endFadeColor;
    [SerializeField] private Image goldIcon;

    [SerializeField] private Image popup;
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private Button adBtn;

    [SerializeField] private RectTransform driftCounterTransform;

    private DriftCounter driftCounter;

    private int gold;
    
    private void CountGold()
    {
        var sequence = DOTween.Sequence();

        // Get the positions of the drift counter and the gold text
        Vector3 startPosition = driftCounterTransform.position;
        Vector3 endPosition = goldText.transform.position;

        for (int i = 0; i < driftCounter.DriftCount; i++)
        {
            // Create a dummy Image to animate from the drift counter to the gold text
            GameObject dummyObject = new GameObject("GoldDummy");
            RectTransform goldRect = dummyObject.AddComponent<RectTransform>();
            Image goldImg = dummyObject.AddComponent<Image>();

            // Set the parent to bgImg's parent or the canvas for proper positioning
            goldRect.SetParent(bgImg.transform);
            goldRect.position = startPosition;

            // Optional: Set the dummy image's appearance
            goldImg.sprite = goldIcon.sprite;  // Set to a relevant sprite
            goldImg.rectTransform.sizeDelta = new Vector2(65, 65);
            goldImg.raycastTarget = false;// Adjust size if necessary

            // Animate the dummy Image from driftCounterTransform to goldText position
            float jumpPower = 90f; // Adjust the jump height
            int numJumps = 1; // Number of jumps
            sequence.Append(goldRect.DOJump(endPosition, jumpPower, numJumps, (float)1/(float)driftCounter.DriftCount).SetEase(Ease.OutQuad));

            // Scale the gold text when the dummy image reaches the position
            sequence.AppendCallback(() =>
            {
                goldText.transform.DOScale(1.2f, 0.2f).SetEase(Ease.OutBounce).OnComplete(() =>
                {
                    goldText.transform.DOScale(1f, 0.2f); // Scale back to normal
                });

                // Increment the gold count and update the text
                gold++;
                goldText.text = gold.ToString();
            });

            // Destroy the dummy Image after the animation
            sequence.AppendCallback(() =>
            {
                Destroy(dummyObject);
            });
        }

        // Make the ad button interactable when the entire sequence is finished
        sequence.OnComplete(() =>
        {
            adBtn.interactable = true;
        });

        sequence.Play();
    }
    
    public void Show()
    {
        var showSequence = DOTween.Sequence();
        
        showSequence.Join(bgImg.transform.DOScale(1, 1));
        showSequence.Join(bgImg.DOColor(endFadeColor, 1));
        showSequence.OnComplete(() => CountGold());
        showSequence.Play();
    }

    

    public void Init(MonoBehaviour behaviour)
    {
        driftCounter = (DriftCounter)behaviour;
    }

    public void Hide()
    {
        bgImg.color = new Color(0, 0, 0, 0);
        bgImg.transform.DOScale(0, 0);
    }
}
